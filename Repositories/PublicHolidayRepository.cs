using WaterlilyLabs.Data;
using WaterlilyLabs.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace WaterlilyLabs.Repositories
{
    public class PublicHolidayRepository : Repository<PublicHoliday>, IPublicHolidayRepository
    {
        public PublicHolidayRepository(WaterlilyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PublicHoliday>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Convert the DateTime input parameters to DateOnly for comparison
            // with the PublicHoliday.HolidayDate property (which is DateOnly).
            var startRangeDateOnly = DateOnly.FromDateTime(startDate);
            var endRangeDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(h => h.HolidayDate >= startRangeDateOnly && h.HolidayDate <= endRangeDateOnly)
                .ToListAsync();
        }
    }
}
