using ECommerceApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApp.Domain.Interface
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<int> CreateOrderAsync(Order order);
        Task<int> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<int> CancelOrderAsync(Guid orderId);
    }
}
