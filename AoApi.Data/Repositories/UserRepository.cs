using AoApi.Data.Models;

namespace AoApi.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(AOContext context) : base(context)
        {
        }
    }
}
