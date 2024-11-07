using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Services.Interface;
using FTravel.Repository.Repositories.Interface;
using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Service.BusinessModels.OrderModels;
using System.Net.Sockets;
using FTravel.Service.Enums;
using FTravel.Service.Utils;

namespace FTravel.Service.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IWalletService _walletService;
        private readonly INotificationService _notificationService;
        private readonly IServiceTicketRepository _serviceTicketRepository;
        private ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public TicketService(ITicketRepository ticketRepository,
            IMapper mapper,
            IOrderRepository orderRepository,
            ITripRepository tripRepository,
            IWalletService walletService,
            INotificationService notificationService,
            IServiceTicketRepository serviceTicketRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _tripRepository = tripRepository;
            _walletService = walletService;
            _notificationService = notificationService;
            _serviceTicketRepository = serviceTicketRepository;
        }

        public async Task<TicketModel> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTripDetailById(id);
            TicketModel ticketModel = _mapper.Map<TicketModel>(ticket);
            return ticketModel;
        }

        public async Task<bool> CancelTicketAsync(int ticketId)
        {
            using (var ticketTransaction = await _ticketRepository.BeginTransactionAsync())
            {
                try
                {
                    var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
                    if (ticket == null)
                    {
                        throw new Exception("Không tìm thấy vé");
                    }

                    var order = await _orderRepository.GetOrderByTicketId(ticketId);
                    if (order == null)
                    {
                        throw new Exception("Không tìm thấy đơn hàng");
                    }

                    if (order.PaymentStatus == PaymentOrderStatus.FAILED.ToString())
                    {
                        throw new Exception("Đơn hàng chưa được thanh toán, không thể hủy vé.");
                    }

                    if (order.PaymentStatus == PaymentOrderStatus.REFUNDED.ToString())
                    {
                        throw new Exception("Đã hoàn tiền vé và dịch vụ cho đơn hàng này.");
                    }

                    var trip = await _tripRepository.GetTripById(ticket.TripId.Value);
                    if (trip == null)
                    {
                        throw new Exception("Không tìm thấy chuyến xe");
                    }

                    // check trip
                    if (trip.Status != TripStatus.OPENING.ToString())
                    {
                        throw new Exception("Chỉ có thể hủy vé khi chuyến xe chưa bắt đầu chạy.");
                    }

                    // calculate refund value

                    var orderValue = order.TotalPrice.Value;
                    int valueRefund = 0;
                    int percentRefund = 0;

                    var currentTime = TimeUtils.GetTimeVietNam();
                    var timeUntilStart = trip.EstimatedStartDate - currentTime;

                    if (timeUntilStart.Value.TotalHours >= 24)
                    {
                        valueRefund = orderValue;
                        percentRefund = 100;
                    }
                    else if (timeUntilStart.Value.TotalHours >= 12)
                    {
                        valueRefund = (int)(orderValue * 0.70);
                        percentRefund = 70;
                    }
                    else if (timeUntilStart.Value.TotalHours >= 6)
                    {
                        valueRefund = (int)(orderValue * 0.50);
                        percentRefund = 50;
                    }
                    else
                    {
                        valueRefund = 0;
                    }
                    

                    // refund to wallet
                    if (valueRefund > 0)
                    {
                        string messageRefund = $"Hoàn trả {percentRefund}% giá vé và dịch vụ của chuyến đi {trip.Name} xuất phát lúc " +
                            $"{NumberUtils.ConvertDateToCustomFormat(trip.EstimatedStartDate.Value)} " +
                            $"từ {trip.Route.StartPointNavigation.Name} đến {trip.Route.EndPointNavigation.Name}";

                        var wallet = await _walletService.GetWalletByCustomerIdAsync(order.UserId.Value);

                        var updateWallet = await _walletService.RefundToWalletAsync(wallet.Id, valueRefund, messageRefund);
                        if (updateWallet)
                        {
                            string messageNoti = $"Bạn đã hủy thành công vé của chuyến đi '{trip.Name}' xuất phát lúc " +
                                $"{NumberUtils.ConvertDateToCustomFormat(trip.EstimatedStartDate.Value)} " +
                                $"từ {trip.Route.StartPointNavigation.Name} đến {trip.Route.EndPointNavigation.Name}";

                            var noti = new Notification
                            {
                                Type = NotificationType.ORDER.ToString(),
                                EntityId = order.Id,
                                UserId = order.UserId.Value,
                                Title = "Hủy vé thành công",
                                Message = messageNoti,
                            };

                            await _notificationService.AddNotificationByCustomerId(order.UserId.Value, noti);

                            // update ticket
                            ticket.Status = TicketStatus.AVAILABLE.ToString();

                            var servicesTicket = await _serviceTicketRepository.GetServiceTicketByTicketId(ticketId);
                            if (servicesTicket.Count > 0)
                            {
                                await _serviceTicketRepository.PermanentDeletedListAsync(servicesTicket);
                            }

                            await _ticketRepository.UpdateAsync(ticket);

                            // update order
                            order.PaymentStatus = PaymentOrderStatus.REFUNDED.ToString();
                            await _orderRepository.UpdateAsync(order);

                            await ticketTransaction.CommitAsync();
                            return true;
                        }
                        else
                        {
                            await ticketTransaction.RollbackAsync();
                            return false;
                        }
                    }
                    else
                    {
                        string messageNoti = $"Bạn đã hủy thành công vé của chuyến đi '{trip.Name}' xuất phát lúc " +
                                $"{NumberUtils.ConvertDateToCustomFormat(trip.EstimatedStartDate.Value)} " +
                                $"từ {trip.Route.StartPointNavigation.Name} đến {trip.Route.EndPointNavigation.Name}";

                        var noti = new Notification
                        {
                            Type = NotificationType.ORDER.ToString(),
                            EntityId = order.Id,
                            UserId = order.UserId.Value,
                            Title = "Hủy vé thành công",
                            Message = messageNoti,
                        };

                        await _notificationService.AddNotificationByCustomerId(order.UserId.Value, noti);

                        // update ticket
                        ticket.Status = TicketStatus.AVAILABLE.ToString();

                        var servicesTicket = await _serviceTicketRepository.GetServiceTicketByTicketId(ticketId);
                        if (servicesTicket.Count > 0)
                        {
                            await _serviceTicketRepository.PermanentDeletedListAsync(servicesTicket);
                        }

                        await _ticketRepository.UpdateAsync(ticket);

                        // update order
                        order.PaymentStatus = PaymentOrderStatus.REFUNDED.ToString();
                        await _orderRepository.UpdateAsync(order);

                        await ticketTransaction.CommitAsync();
                        return true;
                    }
                }
                catch
                {
                    await ticketTransaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
