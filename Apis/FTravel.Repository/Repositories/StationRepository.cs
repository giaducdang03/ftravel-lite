﻿using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class StationRepository : GenericRepository<Station>, IStationRepository
    {
        private readonly FtravelLiteContext _context;

        public StationRepository(FtravelLiteContext context) : base(context)
        {
            {
                _context = context;
            }
        }

        public async Task<Route> CreateRoute(Route route)
        {
            _context.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<Station> CreateStation(Station station)
        {
            _context.Add(station);
            await _context.SaveChangesAsync();
            return station;
        }



        public async Task<Pagination<Station>> GetAllStation(PaginationParameter paginationParameter)
        {
            var query = _context.Stations.Include(s => s.City).AsQueryable();

            var totalCount = await query.CountAsync();
            var paginatedQuery = query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                      .Take(paginationParameter.PageSize);

            var stations = await paginatedQuery.ToListAsync();

            return new Pagination<Station>(stations, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<Station> GetStationById(int id)
        {
            var station = await _context.Stations.Include(s => s.City).FirstOrDefaultAsync(x => x.Id == id);
            return station;
        }
        public async Task<List<RouteStation>> GetRouteStationById(int id)
        {
            var station = await _context.RouteStations.Where(x => x.StationId == id).ToListAsync();
            return station;
        }

    }
}
