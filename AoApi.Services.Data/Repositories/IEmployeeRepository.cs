using AoApi.Data.Models;
using AoApi.Data.Repositories;
using AoApi.Services.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Services.Data.Repositories
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetEmployeesAsync(string orderBy, string searchQuery, int pageNumber, int pageSize, IDictionary<string, IEnumerable<string>> mapping);
    }
}
