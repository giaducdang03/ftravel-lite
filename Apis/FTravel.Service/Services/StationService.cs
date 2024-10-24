using AutoMapper;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.RouteModels;
using FTravel.Service.BusinessModels.StationModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly ICityRepository _cityRepository;

        //private readonly IBusCompanyRepository _busCompanyRepository;
        private readonly IMapper _mapper;

        public StationService(IStationRepository stationRepository,
            ICityRepository cityRepository,
            //IBusCompanyRepository busCompanyRepository,
            IMapper mapper)
        {
            _stationRepository = stationRepository;
            _cityRepository = cityRepository;
            //_busCompanyRepository = busCompanyRepository;
            _mapper = mapper;
        }

        public async Task<RouteModel> CreateRoute(RouteModel route)
        {
            try
            {
                var map = _mapper.Map<Route>(route);
                var createRoute = await _stationRepository.CreateRoute(map);
                var resutl = _mapper.Map<RouteModel>(createRoute);
                return resutl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<StationModel> CreateStationService(CreateStationModel stationModel)
        {
            var stationUnsign = StringUtils.ConvertToUnSign(stationModel.Name);
            var stations = await _stationRepository.GetAllAsync();
            var existStation = stations.Where(x => x.UnsignName.ToLower() == stationUnsign.ToLower());

            var cities = await _cityRepository.GetAllAsync();
            var stationCity = cities.First(x => x.Code == stationModel.CityCode);
            if (stationCity == null)
            {
                throw new Exception("Tỉnh / thành phố không tồn tại");
            }

            if (!existStation.Any())
            {
                var newStation = new Station
                {
                    Name = stationModel.Name,
                    UnsignName = stationUnsign,
                    Status = CommonStatus.ACTIVE.ToString(),
                    Address = stationModel.Address,
                    CityId = stationCity.Id,
                };
                var createStation = await _stationRepository.AddAsync(newStation);
                var result = _mapper.Map<StationModel>(createStation);
                return result;
            }
            else
            {
                throw new Exception("Trạm đã tồn tại.");
            }
        }


        public async Task<Pagination<StationModel>> GetAllStationService(PaginationParameter paginationParameter)
        {
            var stations = await _stationRepository.GetAllStation(paginationParameter);
            if (!stations.Any())
            {
                return null;
            }

            var stationModels = _mapper.Map<Pagination<StationModel>>(stations);

            return new Pagination<StationModel>(stationModels,
                stations.TotalCount,
                stations.CurrentPage,
                stations.PageSize);

        }

        public async Task<Station> GetStationServiceDetailById(int id)
        {
            var data = await _stationRepository.GetStationById(id);
            return data;
        }
        public async Task<int> UpdateStationService(UpdateStationModel updateStation, int stationId)
        {
            var oldStation = await _stationRepository.GetStationById(stationId);

            var cities = await _cityRepository.GetAllAsync();
            var stationCity = cities.First(x => x.Code == updateStation.CityCode);
            if (stationCity == null)
            {
                throw new Exception("Tỉnh / thành phố không tồn tại");
            }

            if (oldStation == null)
            {
                return -1;
            }
            else
            {
                oldStation.Name = updateStation.Name;
                oldStation.Status = updateStation.Status.ToString();
                oldStation.UnsignName = StringUtils.ConvertToUnSign(updateStation.Name);
                oldStation.Address = updateStation.Address;
                oldStation.CityId = stationCity.Id;
            }
            var result = await _stationRepository.UpdateAsync(oldStation);
            return result;
        }
        public async Task<bool> DeleteStationService(int stationId)
        {
            var deleteStation = await _stationRepository.GetStationById(stationId);
            var routeStation = await _stationRepository.GetRouteStationById(stationId);
            if (deleteStation == null)
            {
                return false;
            }
            else
            {
                if (routeStation.Count > 0)
                {
                    return false;
                }
                else
                {
                    deleteStation.Status = CommonStatus.INACTIVE.ToString();
                    var result = await _stationRepository.SoftDeleteAsync(deleteStation);
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        //public async Task<List<StationModel>> GetStationByBusCompanyId(int id)
        //{
        //    var listStation = await _stationRepository.GetStationByBusCompanyId(id);
        //    var listStationModel = _mapper.Map<List<StationModel>>(listStation);
        //    return listStationModel;
        //}

    }
}