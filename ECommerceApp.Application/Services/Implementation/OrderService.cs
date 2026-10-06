using AutoMapper;
using ECommerceApp.Application.Dto.Order;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Order;
using ECommerceApp.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Services.Implementation
{
    public class OrderService(IOrderRepository orderRepository, IGeneric<Product> productRepository, IMapper mapper) : IOrderService
    {
        public async Task<ServiceResponse> CreateOrderAsync(CreateOrderDto orderDto, string userId)
        {
            var products = await productRepository.GetAllAsync();
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in orderDto.CartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    return new ServiceResponse(false, $"Product with ID {item.ProductId} not found");
                }

                orderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                totalAmount += product.Price * item.Quantity;
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                ShippingAddress = orderDto.ShippingAddress,
                PhoneNumber = orderDto.PhoneNumber,
                Status = OrderStatus.Pending,
                PaymentMethodId = orderDto.PaymentMethodId,
                OrderItems = orderItems
            };

            var result = await orderRepository.CreateOrderAsync(order);
            return result > 0 
                ? new ServiceResponse(true, "Order placed successfully") 
                : new ServiceResponse(false, "Failed to place order");
        }

        public async Task<IEnumerable<GetOrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await orderRepository.GetByUserIdAsync(userId);
            return mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null) return null;
            return mapper.Map<GetOrderDto>(order);
        }

        public async Task<ServiceResponse> CancelOrderAsync(Guid id, string userId)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return new ServiceResponse(false, "Order not found");
            }

            if (order.UserId != userId)
            {
                return new ServiceResponse(false, "Unauthorized to cancel this order");
            }

            if (order.Status != OrderStatus.Pending)
            {
                return new ServiceResponse(false, "Order cannot be cancelled as it is already processed or shipped");
            }

            var result = await orderRepository.CancelOrderAsync(id);
            return result > 0 
                ? new ServiceResponse(true, "Order cancelled successfully") 
                : new ServiceResponse(false, "Failed to cancel order");
        }

        public async Task<ServiceResponse> UpdateOrderStatusAsync(Guid id, string status)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                return new ServiceResponse(false, "Invalid order status");
            }

            var result = await orderRepository.UpdateOrderStatusAsync(id, orderStatus);
            return result > 0 
                ? new ServiceResponse(true, "Order status updated successfully") 
                : new ServiceResponse(false, "Failed to update order status or order not found");
        }
    }
}
