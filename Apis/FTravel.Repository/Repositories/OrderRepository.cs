using FTravel.Repository.Commons.Filter;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FTravel.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly FtravelLiteContext _context;

        public OrderRepository(FtravelLiteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Transactions)
                .Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Pagination<OrderDetail>> GetAllOrderAsync(PaginationParameter paginationParameter, OrderFilter orderFilter)
        {
            var query = _context.OrderDetails.Include(x => x.Order)
                                                .Include(x => x.Order.User)
                                                .Include(x => x.Ticket.Trip.Route).AsQueryable();

            // apply filter
            query = ApplyOrderDetailFiltering(query, orderFilter);

            var itemCount = await query.CountAsync();
            var items = await query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<OrderDetail>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        private IQueryable<OrderDetail> ApplyOrderDetailFiltering(IQueryable<OrderDetail> query, OrderFilter orderFilter)
        {
            if (!string.IsNullOrWhiteSpace(orderFilter.SortBy))
            {
                switch (orderFilter.SortBy.ToLower())
                {
                    case "createdate":
                        query = orderFilter.Dir?.ToLower() == "asc" ? query.OrderBy(s => s.CreateDate) : query.OrderByDescending(s => s.CreateDate);
                        break;
                    case "tripname":
                        query = orderFilter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.Ticket.Trip.Name) : query.OrderBy(s => s.Ticket.Trip.Name);
                        break;
                    case "totalprice":
                        query = orderFilter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.Order.TotalPrice) : query.OrderBy(s => s.Order.TotalPrice);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id);
                        break;
                }
            }

            return query;
        }
        public async Task<List<OrderDetail>> GetOrderDetailByIdAsync(int id)
        {
            var result = await _context.OrderDetails.Include(x => x.Ticket)
                                            .ThenInclude(x => x.ServiceTickets)
                                            .ThenInclude(x => x.Service)
                                            .Include(x => x.Ticket.TicketType)
                                            .Include(x => x.Ticket.Trip)
                                            .Include(x => x.Ticket.Trip.Route)
                                            .Include(x => x.Ticket.Trip.Route.StartPointNavigation)
                                            .Include(x => x.Ticket.Trip.Route.EndPointNavigation)
                                            .Include(x => x.Order)
                                            .Include(x => x.Order.Transactions)
                                            .ThenInclude(x => x.Wallet)
                                            .Include(x => x.Order.User)
                                            .Where(x => x.OrderId == id)
                                            .ToListAsync();
            return result;

        }


        public async Task<List<OrderDetail>> StatisticForDashBoard()
        {
            var result = await _context.OrderDetails.Include(x => x.Ticket)
                                            .Include(x => x.Ticket.Trip)
                                            .Include(x => x.Ticket.Trip.Route)
                                            .Include(x => x.Ticket.Trip.Route.StartPointNavigation)
                                            .Include(x => x.Ticket.Trip.Route.EndPointNavigation)
                                            .Include(x => x.Order)
                                            .Include(x => x.Order.User)
                                            .ToListAsync();
            return result;
        }

        public async Task<Order> GetOrderByTicketId(int ticketId)
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.OrderDetails.Any(od => od.TicketId == ticketId));
        }
    }
}
