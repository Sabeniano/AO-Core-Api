using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.DataRepositories
{
    public class WorkhoursRepository : RepositoryBase<Workhours>
    {
        public WorkhoursRepository(AOContext context) : base(context)
        {
        }
    }
}
