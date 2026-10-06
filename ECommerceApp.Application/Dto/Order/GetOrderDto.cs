using System;
using System.Collections.Generic;

namespace ECommerceApp.Application.Dto.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid PaymentMethodId { get; set; }
        public IEnumerable<GetOrderItemDto> OrderItems { get; set; } = new List<GetOrderItemDto>();
    }
}
