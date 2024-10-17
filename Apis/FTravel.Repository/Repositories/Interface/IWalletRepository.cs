﻿using FTravel.Repository.EntityModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        public Task<Wallet> GetWalletByUserId(int userId);

        public Task<Wallet> GetWalletByIdAsync(int walletId);

    }
}
