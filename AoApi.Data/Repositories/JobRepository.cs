using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class JobRepository : RepositoryBase<Job>
    {
        public JobRepository(AOContext context) : base(context)
        {
        }
    }
}
