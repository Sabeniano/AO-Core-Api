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
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AOContext context) : base(context)
        {
        }

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
