using WaterlilyLabs.Models.Data;
using WaterlilyLabs.Repositories;
using WaterlilyLabs.Services.Delegates;

namespace WaterlilyLabs.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICachingService _cachingService;
        private readonly ILogger<EmployeeService> _logger; 

        public event EntityChangedNotification? OnEmployeeChanged; 

        private const string AllEmployeesCacheKey = "AllEmployees";
        private string EmployeeByIdCacheKey(int id) => $"Employee_{id}";


        public EmployeeService(IEmployeeRepository employeeRepository, ICachingService cachingService, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _cachingService = cachingService;
            _logger = logger;

            // Example: Subscribe a logger to the delegate event
            OnEmployeeChanged += (entityType, action, entityId) =>
            {
                _logger.LogInformation($"Delegate Notified: {entityType} {action} - ID: {entityId?.ToString() ?? "N/A"}");
            };
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            // Using the "Cached" method (5 min expiry) for list of employees
            return await _cachingService.CachedAsync(AllEmployeesCacheKey,
                async () => await _employeeRepository.GetAllAsync(),
                TimeSpan.FromMinutes(5));
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            // Using "CachedLong" for individual employees
            return await _cachingService.CachedLongAsync(EmployeeByIdCacheKey(id),
                async () => await _employeeRepository.GetByIdAsync(id));
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            _cachingService.Remove(AllEmployeesCacheKey);
            OnEmployeeChanged?.Invoke(nameof(Employee), "Created", employee.Id); 
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
            if (existingEmployee == null) return false;

            // EF Core might need specific update logic if not all fields are mapped from viewmodel
            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.JobPosition = employee.JobPosition;
            // _employeeRepository.Update(existingEmployee); 

            await _employeeRepository.SaveChangesAsync();
            _cachingService.Remove(AllEmployeesCacheKey);
            _cachingService.Remove(EmployeeByIdCacheKey(employee.Id));
            OnEmployeeChanged?.Invoke(nameof(Employee), "Updated", employee.Id);
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            _employeeRepository.Remove(employee);
            await _employeeRepository.SaveChangesAsync();
            _cachingService.Remove(AllEmployeesCacheKey);
            _cachingService.Remove(EmployeeByIdCacheKey(id));
            OnEmployeeChanged?.Invoke(nameof(Employee), "Deleted", id);
            return true;
        }
    }
}
