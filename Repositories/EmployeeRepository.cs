using WaterlilyLabs.Data;
using WaterlilyLabs.Models.Data;

namespace WaterlilyLabs.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(WaterlilyDbContext context) : base(context)
        {
        }
    }
}
