using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the User model/table
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
