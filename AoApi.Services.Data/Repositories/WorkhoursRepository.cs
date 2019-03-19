using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Implementation of the data access layer for the Workhours model/table
    /// </summary>
    public class WorkhoursRepository : RepositoryBase<Workhours>, IWorkhoursRepository
    {
        public WorkhoursRepository(AOContext context) : base(context)
        {
        }
    }
}
