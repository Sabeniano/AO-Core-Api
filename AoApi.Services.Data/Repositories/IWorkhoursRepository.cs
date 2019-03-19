using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the Workhour model/table
    /// </summary>
    public interface IWorkhoursRepository : IRepositoryBase<Workhours>
    {
    }
}
