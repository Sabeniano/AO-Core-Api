using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;


namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Implementation of the data access layer for the Role model/table
    /// </summary>
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(AOContext context) : base(context)
        {
        }
    }
}
