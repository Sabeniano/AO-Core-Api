using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.DataRepositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(AOContext context) : base(context)
        {
        }
    }
}
