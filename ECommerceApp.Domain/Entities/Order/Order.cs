using System;
using System.Collections.Generic;
using ECommerceApp.Domain.Entities.Identity;

namespace ECommerceApp.Domain.Entities.Order
{
    /// <summary>
    /// Represents a customer order in the e-commerce system.
    /// Tracks financial totals, delivery contact, and lifecycle status.
    /// </summary>
    public class Order
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public AppUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Guid PaymentMethodId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
