using WaterlilyLabs.Models.Data;
using WaterlilyLabs.Services.Delegates;

namespace WaterlilyLabs.Services
{
    public interface IEmployeeService
    {
        event EntityChangedNotification? OnEmployeeChanged;

        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
