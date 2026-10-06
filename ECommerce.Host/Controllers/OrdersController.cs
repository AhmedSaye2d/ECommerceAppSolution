using ECommerceApp.Application.Dto.Order;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated");

            var result = await orderService.CreateOrderAsync(orderDto, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated");

            var result = await orderService.GetUserOrdersAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated");

            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found");

            // Allow the user who placed the order OR an Admin to view it
            var isAdmin = User.IsInRole("Admin");
            if (order.UserId != userId && !isAdmin)
                return Forbid();

            return Ok(order);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated");

            var result = await orderService.CancelOrderAsync(id, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
                return BadRequest("Status cannot be empty");

            var result = await orderService.UpdateOrderStatusAsync(id, status);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
