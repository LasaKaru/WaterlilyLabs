using System.ComponentModel.DataAnnotations;

namespace WaterlilyLabs.Models.ViewModels
{
    public class WorkingDaysRequestViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

        public int? CalculatedWorkingDays { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
