﻿using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ICityService
    {
        public Task<Pagination<CityModel>> GetListCityAsync(PaginationParameter paginationParameter);

        //public Task<CityModel> UpdateCityAsync(CityModel updateCityModel, int id);
        public Task<int> CreateCityAsync(int cityCode, string cityName);
        public Task<bool> RemoveSoftCityAsync(int deleteCity);
    }
}
