using ECommerceApp.Application.Dto.Order;
using ECommerceApp.Application.Dto.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse> CreateOrderAsync(CreateOrderDto orderDto, string userId);
        Task<IEnumerable<GetOrderDto>> GetUserOrdersAsync(string userId);
        Task<GetOrderDto?> GetOrderByIdAsync(Guid id);
        Task<ServiceResponse> CancelOrderAsync(Guid id, string userId);
        Task<ServiceResponse> UpdateOrderStatusAsync(Guid id, string status);
    }
}
