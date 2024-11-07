using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.TripModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ITripService
    {
        public Task<Pagination<TripModel>> GetAllTripAsync(PaginationParameter paginationParameter, TripFilter filter);
        public Task<TripModel> GetTripByIdAsync(int id);
        public Task<bool> CreateTripAsync(CreateTripModel trip);
        public Task<bool> UpdateTripAsync(UpdateTripModel tripModel);
        public Task<bool> UpdateTripStatusAsync(int id, string status);
        public Task<bool> UpdateTripStatusAsyncV2(UpdateTripStatusModel updateTripStatus);
        public Task<bool> CancelTripAsync(int id, string status);
        public Task<TripModel> GetTemplateTripAsync();
        public Task<Pagination<TripModel>> GetTripStaffAsync(PaginationParameter paginationParameter, TripFilter filter, string email);
    }
}
