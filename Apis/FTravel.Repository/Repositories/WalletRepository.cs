using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly FtravelLiteContext _context;

        public WalletRepository(FtravelLiteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Wallet> GetWalletByUserId(int userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Wallet> GetWalletByIdAsync(int walletId)
        {
            return await _context.Wallets.Include(x => x.Transactions).FirstOrDefaultAsync(x => x.Id == walletId);
        }
    }
}
