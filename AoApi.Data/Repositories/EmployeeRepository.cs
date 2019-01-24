using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AOContext context) : base(context)
        {
        }
    }
}
