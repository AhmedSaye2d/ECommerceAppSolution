using ECommerceApp.Application.Dto.Product;
using System;
namespace ECommerceApp.Application.Dto.Category
{
    public class GetCategory:CategoryBase
    {
        public Guid Id { get; set; }
        public ICollection<GetProduct>? Products { get; set; }
    }
}
