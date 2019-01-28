using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;


namespace AoApi.Services.DataRepositories
{
    public class RoleRepository : RepositoryBase<Role>
    {
        public RoleRepository(AOContext context) : base(context)
        {
        }
    }
}
