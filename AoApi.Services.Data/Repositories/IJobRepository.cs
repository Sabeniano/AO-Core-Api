using AoApi.Data.Models;
using AoApi.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the Job model/table
    /// </summary>
    public interface IJobRepository : IRepositoryBase<Job>
    {
        Task<Job> GetJobByEmployeeId(Guid id);
    }
}
