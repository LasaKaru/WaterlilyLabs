namespace WaterlilyLabs.Services
{
    public interface IWorkingDaysService
    {
        Task<(int WorkingDays, string? ErrorMessage)> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate);
    }
}
