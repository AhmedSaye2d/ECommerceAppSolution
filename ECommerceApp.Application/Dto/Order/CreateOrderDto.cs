using ECommerceApp.Application.Dto.Cart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.Dto.Order
{
    public class CreateOrderDto
    {
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public Guid PaymentMethodId { get; set; }

        [Required]
        public IEnumerable<ProcessCart> CartItems { get; set; } = new List<ProcessCart>();
    }
}
