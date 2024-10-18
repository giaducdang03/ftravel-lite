using AutoMapper;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.AccountModels;
using FTravel.Service.BusinessModels.BuscompanyModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.BusinessModels.PaymentModels;
using FTravel.Service.BusinessModels.RouteModels;
using FTravel.Service.BusinessModels.ServiceModels;
using FTravel.Service.BusinessModels.StationModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.BusinessModels.TripModels;
using FTravel.Service.Services;
using FTravel.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace FTravel.Service.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            // add mapper model

            // user

            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<Pagination<User>, Pagination<UserModel>>().ConvertUsing<PaginationConverter<User, UserModel>>();

            CreateMap<Wallet, WalletModel>().ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<Pagination<Wallet>, Pagination<WalletModel>>().ConvertUsing<PaginationConverter<Wallet, WalletModel>>();
            CreateMap<Transaction, TransactionModel>();

            CreateMap<UpdateAccountModel, User>()
                .ForMember(dest => dest.UnsignFullName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.FullName)));

            CreateMap<CreateAccountModel, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // route

            CreateMap<City, CityModel>().ReverseMap();

            CreateMap<CreateRouteModel, Route>().ReverseMap();

            CreateMap<Route, UpdateRouteModel>().ForMember(dest => dest.Status, opt => opt.Ignore()).ReverseMap();

            CreateMap<Route, RouteModel>()
                .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.StartPointNavigation.Name))
                .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.EndPointNavigation.Name));
            CreateMap<Pagination<Route>, Pagination<RouteModel>>().ConvertUsing<PaginationConverter<Route, RouteModel>>();

            CreateMap<RouteStation, RouteStationModel>();

            // station

            CreateMap<Station, StationModel>()
                 .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));
            CreateMap<Pagination<Station>, Pagination<StationModel>>().ConvertUsing<PaginationConverter<Station, StationModel>>();

            // service

            CreateMap<Repository.EntityModels.Service, ServiceModel>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.Name : string.Empty))
                .ForMember(dest => dest.StationName, opt => opt.MapFrom(src => src.Station != null ? src.Station.Name : string.Empty))
                .ReverseMap();

            CreateMap<CreateServiceModel, Repository.EntityModels.Service>()
                .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)));

            CreateMap<UpdateServiceModel, Repository.EntityModels.Service>()
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)));

            // ticket - ticket-type

            CreateMap<CreateTicketTypeModel, TicketType>().ReverseMap();

            CreateMap<UpdateTicketTypeModel, TicketType>()
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<TicketType, TicketTypeModel>();

            // trip

            CreateMap<Trip, TripModel>()
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(src => src.Route != null ? src.Route.Name : string.Empty))
                .ForMember(dest => dest.LowestPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore())
                .ForMember(dest => dest.Services, opt => opt.Ignore());

            CreateMap<CreateTripModel, Trip>()
                .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)))
                .ForMember(dest => dest.TripServices, opt => opt.Ignore())
                .ForMember(dest => dest.TripTicketTypes, opt => opt.Ignore());

            CreateMap<UpdateTripModel, Trip>()
                .ForMember(dest => dest.UnsignName, opt => opt.MapFrom(src => StringUtils.ConvertToUnSign(src.Name)))
                .ForMember(dest => dest.TripServices, opt => opt.Ignore())
                .ForMember(dest => dest.TripTicketTypes, opt => opt.Ignore());

            CreateMap<Ticket, TicketModel>()
                .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TicketType.Price))
                .ReverseMap();

            CreateMap<CreateTicketTripModel, Ticket>()
                .ForMember(dest => dest.TripId, opt => opt.Ignore());

             CreateMap<TicketServiceModel, ServiceTicket>().ReverseMap();

             CreateMap<CreateTicketModel, Ticket>().ReverseMap();

            CreateMap<Repository.EntityModels.TripService, TripServiceModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServiceId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Service.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Service.ShortDescription))
               .ForMember(dest => dest.ServicePrice, opt => opt.MapFrom(src => src.ServicePrice))
               .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Service.ImgUrl));



            CreateMap<OrderModel, Order>().ReverseMap();
            CreateMap<Order, ResponseOrderModel>();

            CreateMap<OrderDetail, OrderViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order.User.FullName)).ReverseMap();

            // CreateMap<OrderDetail, OrderDetailModel>()
            //      .ForMember(dest => dest.BusCompanyName, opt => opt.MapFrom(src => src.Ticket.Trip.Route.BusCompany.Name))
            //      .ForMember(dest => dest.TripStartDate, opt => opt.MapFrom(src => src.Ticket.Trip.EstimatedStartDate))
            //      .ForMember(dest => dest.TripEndDate, opt => opt.MapFrom(src => src.Ticket.Trip.EstimatedEndDate))
            //      .ForMember(dest => dest.TripName, opt => opt.MapFrom(src => src.Ticket.Trip.Name))
            //      .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.Ticket.Trip.Route.StartPointNavigation.Name))
            //      .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.Ticket.Trip.Route.EndPointNavigation.Name))
            //      .ForMember(dest => dest.SeatCode, opt => opt.MapFrom(src => src.Ticket.SeatCode))
            //      .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName));




            // // for route


            CreateMap<Transaction, OrderTransactionModel>()
                .ForMember(dest => dest.AccountBalance, opt => opt.MapFrom(src => src.Wallet.AccountBalance))
                .ReverseMap();

            CreateMap<OrderDetail, GetAllOrderModel>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.Order.Code))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.Order.CreateDate))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.Order.PaymentDate))
                .ForMember(dest => dest.PaymentOrderStatus, opt => opt.MapFrom(src => src.Order.PaymentStatus))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order.User.FullName))
                .ForMember(dest => dest.TripName, opt => opt.MapFrom(src => src.Ticket.Trip.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Order.User.PhoneNumber))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Order.TotalPrice))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Order.User.Email));

            // // my ticket
            // CreateMap<ServiceTicket, ServiceTicketModel>()
            //     .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
            //     .ForMember(dest => dest.StationId, opt => opt.MapFrom(src => src.Service.StationId))
            //     .ForMember(dest => dest.StationName, opt => opt.MapFrom(src => src.Service.Station.Name));
        }

        public class PaginationConverter<TSource, TDestination> : ITypeConverter<Pagination<TSource>, Pagination<TDestination>>
        {
            public Pagination<TDestination> Convert(Pagination<TSource> source, Pagination<TDestination> destination, ResolutionContext context)
            {
                var mappedItems = context.Mapper.Map<List<TDestination>>(source);
                return new Pagination<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
            }
        }
    }
}