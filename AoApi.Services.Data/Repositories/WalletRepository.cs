using AoApi.Data;
using AoApi.Data.Models;
using AoApi.Data.Repositories;

namespace AoApi.Services.Data.Repositories
{
    /// <summary>
    /// Implementation of the data access layer for the Wallet model/table
    /// </summary>
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(AOContext context) : base(context)
        {
        }
    }
}
