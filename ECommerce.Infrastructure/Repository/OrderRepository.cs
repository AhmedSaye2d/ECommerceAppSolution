using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities.Order;
using ECommerceApp.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            await context.Orders.AddAsync(order);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order == null) return 0;
            order.Status = status;
            return await context.SaveChangesAsync();
        }

        public async Task<int> CancelOrderAsync(Guid orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order == null) return 0;
            order.Status = OrderStatus.Cancelled;
            return await context.SaveChangesAsync();
        }
    }
}
