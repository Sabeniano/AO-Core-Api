using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the Employee model/table
    /// </summary>
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        /// <summary>
        /// Gets a sorted and paged list of all the employees
        /// </summary>
        /// <param name="orderBy">property to order by</param>
        /// <param name="searchQuery">find certain ones by a search</param>
        /// <param name="pageNumber">page number to get</param>
        /// <param name="pageSize">how many in each page</param>
        /// <param name="mapping">the mapping of the properties</param>
        /// <returns>A Task that eventually resolves to a paged list of employees</returns>
        Task<PagedList<Employee>> GetEmployeesAsync(string orderBy, string searchQuery, int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping);
    }
}
