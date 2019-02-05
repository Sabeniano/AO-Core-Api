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
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AOContext context) : base(context)
        {
        }

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
