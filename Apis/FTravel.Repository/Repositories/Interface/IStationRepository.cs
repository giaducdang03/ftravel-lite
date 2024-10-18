﻿using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IStationRepository : IGenericRepository<Station>
    {
        Task<Pagination<Station>> GetAllStation(PaginationParameter paginationParameter);


        public Task<Station> GetStationById(int id);

        public Task<Route> CreateRoute(Route route);

        public Task<Station> CreateStation(Station station);
        public Task<List<RouteStation>> GetRouteStationById(int id);
        //public Task<List<Station>> GetStationByBusCompanyId(int id);

    }
}
