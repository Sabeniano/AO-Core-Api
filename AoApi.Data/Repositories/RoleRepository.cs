using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>
    {
        public RoleRepository(AOContext context) : base(context)
        {
        }
    }
}
