using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class ScheduleRepository : RepositoryBase<Schedule>
    {
        public ScheduleRepository(AOContext context) : base(context)
        {
        }
    }
}
