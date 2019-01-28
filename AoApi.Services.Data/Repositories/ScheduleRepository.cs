using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.DataRepositories
{
    public class ScheduleRepository : RepositoryBase<Schedule>
    {
        public ScheduleRepository(AOContext context) : base(context)
        {
        }
    }
}
