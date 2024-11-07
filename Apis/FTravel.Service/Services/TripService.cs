using AutoMapper;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.BusinessModels.TripModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TripService(ITripRepository repository, ITicketRepository ticketRepository, IRouteRepository routeRepository,
            IUserRepository userRepository, IMapper mapper)
        {
            _tripRepository = repository;
            _ticketRepository = ticketRepository;
            _routeRepository = routeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<TripModel>> GetAllTripAsync(PaginationParameter paginationParameter, TripFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = StringUtils.ConvertToUnSign(filter.Search);
            }
            if (!string.IsNullOrWhiteSpace(filter.TripStatus) && !Enum.TryParse(typeof(TripStatus), filter.TripStatus, true, out var _))
            {
                throw new ArgumentException("Trạng thái không hợp lệ");
            }
            var trips = await _tripRepository.GetAllTrips(paginationParameter, filter);
            if (!trips.Any())
            {
                return null;
            }

            var tripModels = _mapper.Map<List<TripModel>>(trips);
            foreach (var trip in tripModels)
            {
                var tickets = await _ticketRepository.GetAllByTripId(trip.Id);
                if (tickets.Any())
                {
                    trip.LowestPrice = tickets.Min(t => t.TicketType.Price.Value);
                }
                else
                {
                    trip.LowestPrice = 0;
                }
            }
            return new Pagination<TripModel>(tripModels,
                trips.TotalCount,
                trips.CurrentPage,
                trips.PageSize);
        }

        public async Task<TripModel> GetTripByIdAsync(int id)
        {
            var trip = await _tripRepository.GetTripById(id);
            var tickets = await _ticketRepository.GetAllByTripId(id);
            var services = await _tripRepository.GetServiceByTripId(id);
            if (trip == null)
            {
                return null;
            }
            var tripModel = _mapper.Map<TripModel>(trip);
            tripModel.Tickets = _mapper.Map<List<TicketModel>>(tickets);
            tripModel.Services = _mapper.Map<List<TripServiceModel>>(services);
            return tripModel;
        }
        public async Task<bool> CreateTripAsync(CreateTripModel tripModel)
        {
            try
            {
                //check route
                var route = await _routeRepository.GetRouteDetailByRouteIdAsync(tripModel.RouteId);
                if (route == null)
                {
                    throw new Exception($"Không tìm thấy tuyến xe có id: {tripModel.RouteId}");
                }
                if (route.Status != CommonStatus.ACTIVE.ToString())
                {
                    throw new Exception("Tuyến xe không hoạt động");
                }
                var newTrip = _mapper.Map<Trip>(tripModel);

                //check driver
                var driver = await _userRepository.GetByIdAsync(newTrip.DriverId.Value);
                if (driver == null)
                {
                    throw new Exception("Không tìm thấy tài xế");
                }

                if (driver.Role != RoleEnums.STAFF.ToString())
                {
                    throw new Exception("Người dùng không phải là tài xế");
                }
                var hasOverlappingTrip = await _tripRepository.HasOverlappingTrip(newTrip.DriverId.Value, newTrip.EstimatedStartDate, newTrip.EstimatedEndDate);
                if (hasOverlappingTrip)
                {
                    throw new Exception("Tài xế đã có chuyến đi trùng thời gian");
                }
                //status logic
                if (tripModel.OpenTicketDate.Date.Hour > TimeUtils.GetTimeVietNam().Date.Hour)
                {
                    newTrip.Status = TripStatus.PENDING.ToString();
                }
                else if (tripModel.OpenTicketDate.Date.Hour == TimeUtils.GetTimeVietNam().Date.Hour)
                {
                    newTrip.Status = TripStatus.OPENING.ToString();
                }

                newTrip.IsTemplate = false;
                // Add valid newTrip services
                foreach (var tripService in tripModel.TripServices)
                {
                    var service = route.Services.FirstOrDefault(s => s.Id == tripService.ServiceId);
                    if (service != null)
                    {
                        newTrip.TripServices.Add(new Repository.EntityModels.TripService
                        {
                            Service = service,
                            ServicePrice = tripService.Price
                        });
                    }
                    else
                    {
                        throw new Exception($"không tìm thấy dịch vụ {tripService.ServiceId} trong tuyến xe {route.Name}");
                    }
                }

                foreach (var ticketTypeId in tripModel.TicketTypeIds)
                {
                    var ticketType = route.TicketTypes.FirstOrDefault(t => t.Id == ticketTypeId);
                    if (ticketType != null)
                    {
                        newTrip.TripTicketTypes.Add(new TripTicketType { TicketType = ticketType, TicketTypeId = ticketType.Id });
                    }
                    else
                    {
                        throw new Exception($"không tìm thấy loại vé {ticketTypeId} trong tuyến xe {route.Name}");
                    }
                }

                var ticketList = _mapper.Map<List<Ticket>>(tripModel.TripTickets);
                foreach (var ticket in ticketList)
                {
                    ticket.TripId = 0;
                    if (newTrip.TripTicketTypes.Any(tt => tt.TicketTypeId == ticket.TicketTypeId))
                    {
                        newTrip.Tickets.Add(ticket);
                    }
                }

                // Add newTrip to the repository
                await _tripRepository.AddAsync(newTrip);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateTripAsync(UpdateTripModel tripModel)
        {
            // Validate status
            if (!Enum.TryParse(typeof(TripStatus), tripModel.Status, true, out _))
            {
                throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
            }

            var existingTrip = await _tripRepository.GetTripById(tripModel.Id);
            if (existingTrip == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chuyến xe có id: {tripModel.Id}.");
            }
            int routeId = existingTrip.RouteId == null ? default(int) : existingTrip.RouteId.Value;

            var route = await _routeRepository.GetRouteDetailByRouteIdAsync(routeId);
            if (route == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tuyến xe.");
            }

            _mapper.Map(tripModel, existingTrip);

            existingTrip.TripServices.Clear();
            foreach (var tripServiceModel in tripModel.TripServices)
            {
                var service = route.Services.FirstOrDefault(s => s.Id == tripServiceModel.ServiceId);
                if (service != null)
                {
                    existingTrip.TripServices.Add(new Repository.EntityModels.TripService { Service = service, ServicePrice = tripServiceModel.Price });
                }
            }

            existingTrip.TripTicketTypes.Clear();
            foreach (var ticketTypeId in tripModel.TicketTypeIds)
            {
                var ticketType = route.TicketTypes.FirstOrDefault(t => t.Id == ticketTypeId);
                if (ticketType != null)
                {
                    existingTrip.TripTicketTypes.Add(new TripTicketType { TicketType = ticketType });
                }
            }

            return await _tripRepository.UpdateTripAsync(existingTrip);
        }
        public async Task<bool> UpdateTripStatusAsync(int id, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    throw new ArgumentException("Trạng thái mới không thể rỗng!");
                }
                // Validate status
                if (!Enum.TryParse(typeof(TripStatus), status, true, out _))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
                }

                var trip = await _tripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy chuyến xe.");
                }
                if (!IsValidStatusTransition(status, trip.Status))
                {
                    throw new ArgumentException($"Chuyển trạng thái không hợp lệ. Trạng thái của chuyến đi không thể chuyển từ {trip.Status} sang {status}");
                }

                if (status == "DEPARTED")
                {
                    trip.ActualStartDate = DateTime.UtcNow;
                }
                // Update actual end date if the new status is "COMPLETED"
                else if (status == "COMPLETED")
                {
                    trip.ActualEndDate = DateTime.UtcNow;
                }
                trip.Status = status;

                await _tripRepository.UpdateAsync(trip);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CancelTripAsync(int id, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    throw new ArgumentException("Trạng thái mới không thể rỗng!");
                }
                // Validate status
                if (!Enum.TryParse(typeof(TripStatus), status, true, out _))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ. Trạng thái có thể là: {string.Join(", ", Enum.GetNames(typeof(TripStatus)))}.");
                }

                var trip = await _tripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy chuyến xe.");
                }
                if (!IsValidStatusTransition(status, trip.Status))
                {
                    throw new ArgumentException($"Chuyển trạng thái không hợp lệ. Trạng thái của chuyến đi không thể chuyển từ {trip.Status} sang {status}");
                }

                trip.Status = status;

                await _tripRepository.SoftDeleteAsync(trip);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<TripModel> GetTemplateTripAsync()
        {
            var trip = await _tripRepository.GetTemplateTrip();
            if (trip == null)
            {
                return null;
            }
            var tripModel = _mapper.Map<TripModel>(trip);
            tripModel.Tickets = _mapper.Map<List<TicketModel>>(trip.Tickets);
            return tripModel;
        }

        private bool IsValidStatusTransition(string newStatus, string currentStatus)
        {
            currentStatus = currentStatus?.ToUpper();
            newStatus = newStatus?.ToUpper();

            if (!Enum.TryParse<TripStatus>(currentStatus, true, out TripStatus current))
            {
                return false;
            }

            if (!Enum.TryParse<TripStatus>(newStatus, true, out TripStatus next))
            {
                return false;
            }
            switch (current)
            {
                case TripStatus.PENDING:
                    return next == TripStatus.OPENING || next == TripStatus.CANCELED;
                case TripStatus.OPENING:
                    return next == TripStatus.DEPARTED || next == TripStatus.COMPLETED;
                case TripStatus.DEPARTED:
                    return next == TripStatus.COMPLETED;
                case TripStatus.COMPLETED:
                    // Once the trip is in "DONE" status, no further status changes are allowed
                    return false;
                default:
                    // If the current status is not recognized, disallow any status change
                    return false;
            }
        }

        public async Task<bool> UpdateTripStatusAsyncV2(UpdateTripStatusModel updateTripStatus)
        {
            try
            {
                var trip = await _tripRepository.GetByIdAsync(updateTripStatus.Id);

                if (trip == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy chuyến xe.");
                }

                if (!IsValidStatusTransitionV2(updateTripStatus.Status, Enum.Parse<TripStatus>(trip.Status)))
                {
                    throw new ArgumentException($"Chuyển trạng thái không hợp lệ. Trạng thái của chuyến đi không thể chuyển từ {trip.Status} sang {updateTripStatus.Status.ToString()}");
                }

                // Set the current date if status is DEPARTED or COMPLETED
                if (updateTripStatus.Status == TripStatus.DEPARTED)
                {
                    trip.ActualStartDate = TimeUtils.GetTimeVietNam();
                }

                if (updateTripStatus.Status == TripStatus.COMPLETED)
                {
                    trip.ActualEndDate = TimeUtils.GetTimeVietNam();
                }

                trip.Status = updateTripStatus.Status.ToString();

                await _tripRepository.UpdateAsync(trip);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsValidStatusTransitionV2(TripStatus newStatus, TripStatus currentStatus)
        {
            switch (currentStatus)
            {
                case TripStatus.PENDING:
                    return newStatus == TripStatus.OPENING || newStatus == TripStatus.CANCELED;
                case TripStatus.OPENING:
                    return newStatus == TripStatus.DEPARTED || newStatus == TripStatus.CANCELED;
                case TripStatus.DEPARTED:
                    return newStatus == TripStatus.COMPLETED;
                case TripStatus.COMPLETED:
                    // Once the trip is in "COMPLETED" status, no further status changes are allowed
                    return false;
                default:
                    // If the current status is not recognized, disallow any status change
                    return false;
            }
        }

        public async Task<Pagination<TripModel>> GetTripStaffAsync(PaginationParameter paginationParameter, TripFilter filter, string email)
        {
            var loginUser = await _userRepository.GetUserByEmailAsync(email);
            if (loginUser == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = StringUtils.ConvertToUnSign(filter.Search);
            }
            if (!string.IsNullOrWhiteSpace(filter.TripStatus) && !Enum.TryParse(typeof(TripStatus), filter.TripStatus, true, out var _))
            {
                throw new ArgumentException("Trạng thái không hợp lệ");
            }
            var trips = await _tripRepository.GetAllTripStaff(paginationParameter, filter, loginUser.Id);
            if (!trips.Any())
            {
                return null;
            }

            var tripModels = _mapper.Map<List<TripModel>>(trips);
            foreach (var trip in tripModels)
            {
                var tickets = await _ticketRepository.GetAllByTripId(trip.Id);
                if (tickets.Any())
                {
                    trip.LowestPrice = tickets.Min(t => t.TicketType.Price.Value);
                }
                else
                {
                    trip.LowestPrice = 0;
                }
            }
            return new Pagination<TripModel>(tripModels,
                trips.TotalCount,
                trips.CurrentPage,
                trips.PageSize);
        }
    }
}
