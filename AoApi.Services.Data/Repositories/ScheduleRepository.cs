using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Implementation of the data access layer for the Schedule model/table
    /// </summary>
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AOContext context) : base(context)
        {
        }

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
        public async Task<PagedList<Schedule>> GetSchedulesAsync(
            string orderBy, string searchQuery,
            int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping, Guid employeeId)
        {
            var collectionBeforePagination = _context.Schedules.Where(x => x.EmployeeId == employeeId).Applysort(orderBy, mapping);

            //if (!string.IsNullOrEmpty(searchQuery))
            //{
            //    var searchQueryForWhereClause = searchQuery.Trim().ToLowerInvariant();

            //    //  find by date maybe
            //    //collectionBeforePagination = collectionBeforePagination.Where(x => x.n.ToLowerInvariant().Contains(searchQueryForWhereClause));
            //}

            return PagedList<Schedule>.Create(await collectionBeforePagination.ToListAsync(), pageNumber, pageSize);
        }
    }
}
