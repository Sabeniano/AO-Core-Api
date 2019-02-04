using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;


namespace AoApi.Services.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(AOContext context) : base(context)
        {
        }
    }
}
