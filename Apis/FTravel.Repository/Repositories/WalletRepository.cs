using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Asn1;
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

        public async Task<Pagination<Wallet>> GetWalletPaginationAsync(PaginationParameter paginationParameter)
        {
            var query = _context.Wallets.Include(x => x.User).Include(x => x.Transactions).AsQueryable();

            var itemCount = await query.CountAsync();
            var items = await query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Wallet>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
