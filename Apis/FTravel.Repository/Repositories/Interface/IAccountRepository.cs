using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task<Pagination<User>> GetAllUserAccount(PaginationParameter paginationParameter);
        public Task<List<string>> GetListOfUser();
    }
}
