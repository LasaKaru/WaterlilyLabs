using WaterlilyLabs.Repositories;

namespace WaterlilyLabs.Services
{
    public class WorkingDaysService : IWorkingDaysService
    {
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly ICachingService _cachingService;

        public WorkingDaysService(IPublicHolidayRepository publicHolidayRepository, ICachingService cachingService)
        {
            _publicHolidayRepository = publicHolidayRepository;
            _cachingService = cachingService;
        }

        public async Task<(int WorkingDays, string? ErrorMessage)> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate)
        {
            // Normalize dates to avoid time component issues for input parameters
            DateTime startDateNormalized = startDate.Date;
            DateTime endDateNormalized = endDate.Date;

            if (startDateNormalized > endDateNormalized)
            {
                return (0, "Start date cannot be after end date.");
            }

            if (startDateNormalized.DayOfWeek == DayOfWeek.Saturday || startDateNormalized.DayOfWeek == DayOfWeek.Sunday)
            {
                return (0, "Starting date must be a weekday (Monday-Friday).");
            }

            string holidaysCacheKey = $"PublicHolidays_{startDateNormalized.Year}_{endDateNormalized.Year}";
            var publicHolidays = await _cachingService.CachedAsync(
                holidaysCacheKey,
                // Pass the original DateTime objects to the repository method,
                // it will handle conversion to DateOnly internally if needed for its query.
                async () => (await _publicHolidayRepository.GetHolidaysInRangeAsync(startDateNormalized, endDateNormalized)).ToList(),
                TimeSpan.FromHours(1)
            );

            var holidayDatesSet = new HashSet<DateOnly>(publicHolidays.Select(h => h.HolidayDate));
            int workingDays = 0;
            DateTime currentDate = startDateNormalized;

            while (currentDate <= endDateNormalized)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Convert the current iterating DateTime to DateOnly for checking against the HashSet.
                    DateOnly currentDateAsDateOnly = DateOnly.FromDateTime(currentDate);
                    if (!holidayDatesSet.Contains(currentDateAsDateOnly))
                    {
                        workingDays++;
                    }
                }
                currentDate = currentDate.AddDays(1);
            }
            return (workingDays, null);
        }
    }
}
