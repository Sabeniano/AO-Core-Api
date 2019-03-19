using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Data access layer for the Wallet model/table
    /// </summary>
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
    }
}
