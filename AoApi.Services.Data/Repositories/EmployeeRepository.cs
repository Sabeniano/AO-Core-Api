using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Implementation of the data access layer for the Employee model/table
    /// </summary>
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AOContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets a sorted and paged list of all the employees
        /// </summary>
        /// <param name="orderBy">property to order by</param>
        /// <param name="searchQuery">find certain ones by a search</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">how many in each page</param>
        /// <param name="mapping">the mapping of the properties</param>
        /// <returns>A Task that eventually resolves to a PagedList<T></returns>
        public async Task<PagedList<Employee>> GetEmployeesAsync(
            string orderBy, string searchQuery,
            int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping)
        {
            var collectionBeforePagination = _context.Employees.Applysort(orderBy, mapping);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var searchQueryForWhereClause = searchQuery.Trim().ToLowerInvariant();

                collectionBeforePagination = collectionBeforePagination.Where(x => x.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Employee>.Create(await collectionBeforePagination.ToListAsync(), pageNumber, pageSize);
        }
    }
}
