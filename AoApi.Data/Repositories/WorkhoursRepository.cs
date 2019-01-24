using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class WorkhoursRepository : RepositoryBase<Workhours>
    {
        public WorkhoursRepository(AOContext context) : base(context)
        {
        }
    }
}
