using Microsoft.AspNetCore.Mvc;
using WaterlilyLabs.Models.ViewModels;
using WaterlilyLabs.Services;

namespace WaterlilyLabs.Controllers
{
    public class WorkingDaysController : Controller
    {
        private readonly IWorkingDaysService _workingDaysService;
        private readonly ILogger<WorkingDaysController> _logger;

        public WorkingDaysController(IWorkingDaysService workingDaysService, ILogger<WorkingDaysController> logger)
        {
            _workingDaysService = workingDaysService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Calculator()
        {
            var model = new WorkingDaysRequestViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculator(WorkingDaysRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Calculating working days between {model.StartDate:yyyy-MM-dd} and {model.EndDate:yyyy-MM-dd}");
                var (days, error) = await _workingDaysService.CalculateWorkingDaysAsync(model.StartDate, model.EndDate);
                if (error != null)
                {
                    model.ErrorMessage = error;
                    _logger.LogWarning($"Error calculating working days: {error}");
                }
                else
                {
                    model.CalculatedWorkingDays = days;
                    _logger.LogInformation($"Calculated working days: {days}");
                }
            }
            return View(model);
        }
    }
}
