using Microsoft.AspNetCore.Mvc;
using WaterlilyLabs.Models.Data;
using WaterlilyLabs.Services;

namespace WaterlilyLabs.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;


        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all employees for Index page.");
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,JobPosition")] Employee employee) // Bind only relevant fields
        {
            if (ModelState.IsValid)
            {
                await _employeeService.CreateEmployeeAsync(employee);
                _logger.LogInformation($"Employee {employee.Name} created with ID {employee.Id}.");
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,JobPosition")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _employeeService.UpdateEmployeeAsync(employee);
                    if (!success) return NotFound();
                    _logger.LogInformation($"Employee {employee.Name} (ID: {employee.Id}) updated.");
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, $"Error updating employee ID {employee.Id}");
                    // Optionally: Check if employee exists if Update throws due to concurrency
                    // var exists = (await _employeeService.GetEmployeeByIdAsync(employee.Id)) != null;
                    // if (!exists) return NotFound(); else throw;
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, see your system administrator.");
                    return View(employee);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Attempted to delete non-existent employee ID {id}.");
                return NotFound(); 
            }
            _logger.LogInformation($"Employee ID {id} deleted.");
            return RedirectToAction(nameof(Index));
        }
    }
}
