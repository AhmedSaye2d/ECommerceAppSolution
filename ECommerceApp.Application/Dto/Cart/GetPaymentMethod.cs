using System;
namespace ECommerceApp.Application.Dto.Cart
{
    public class GetPaymentMethod
    {
        public required Guid Id { get; set; }   
        public required string Name { get; set; }
    }
}
