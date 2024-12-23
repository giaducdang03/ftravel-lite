﻿using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class OrderedTicketRepository : GenericRepository<Order>, IOrderedTicketRepository
    {
        private readonly FtravelLiteContext _context;
        private readonly IUserRepository _userRepository;

        public OrderedTicketRepository(FtravelLiteContext context, IUserRepository userRepository) : base(context)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<User> GetCustomerByTicketBoughtId(int ticketId)
        {
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(x => x.TicketId == ticketId);
            if (orderDetail != null)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderDetail.OrderId);
                if (order != null)
                {
                    var customer = await _userRepository.GetByIdAsync(order.UserId.Value);
                    if (customer != null)
                    {
                        return customer;
                    }
                }
            }
            return null;
        }

        public async Task<List<OrderDetail>> GetHistoryOfTripsTakenByCustomerId(int customer)
        {
            var data = await _context.OrderDetails

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .ThenInclude(r => r.StartPointNavigation)

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .ThenInclude(r => r.EndPointNavigation)

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .Where(o => o.Order.UserId == customer)

        .ToListAsync();

            return data;
        }
        public async Task<List<Order>> GetOrderedTicketListByCustomerId(int customer)
        {
            var data = await _context.Orders.Where(x => x.UserId == customer).ToListAsync();
            return data;
        }

        public async Task<Pagination<OrderDetail>> GetOrderedTicketsByCustomer(int customerId, PaginationParameter paginationParameter)
        {
            var orders = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .ThenInclude(r => r.StartPointNavigation)

                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .ThenInclude(r => r.EndPointNavigation)

                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .Where(o => o.Order.UserId == customerId).OrderByDescending(o => o.CreateDate)
                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .AsNoTracking()
            .ToListAsync();

            var itemCount = await _context.OrderDetails.Where(o => o.Order.UserId == customerId).CountAsync();
            var result = new Pagination<OrderDetail>(orders, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
