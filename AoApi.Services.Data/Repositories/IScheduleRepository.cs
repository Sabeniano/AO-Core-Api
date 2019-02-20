using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the Schedule model/table
    /// </summary>
    public interface IScheduleRepository : IRepositoryBase<Schedule>
    {
        /// <summary>
        /// Gets a sorted and paged list of all the schedules belonging to an employee
        /// </summary>
        /// <param name="orderBy">property to order by</param>
        /// <param name="searchQuery">find certain ones by a search</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">how many in each page</param>
        /// <param name="mapping">the mapping of the properties</param>
        /// <param name="employeeId">Id of the employee to find schedule for</param>
        /// <returns>A Task that eventually resolves to a paged list of schedules</returns>
        Task<PagedList<Schedule>> GetSchedulesAsync(string orderBy, string searchQuery, int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping, Guid employeeId);
    }
}
