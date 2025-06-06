using WaterlilyLabs.Models.Data;

namespace WaterlilyLabs.Repositories
{
    public interface IPublicHolidayRepository : IRepository<PublicHoliday>
    {
        Task<IEnumerable<PublicHoliday>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate);
    }
}
