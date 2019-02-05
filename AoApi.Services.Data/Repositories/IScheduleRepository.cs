using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    public interface IScheduleRepository : IRepositoryBase<Schedule>
    {
        Task<PagedList<Schedule>> GetSchedulesAsync(string orderBy, string searchQuery, int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping, Guid employeeId);
    }
}
