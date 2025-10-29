using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Services.Interfaces.Cart
{
    public interface IPaymentService
    {
        Task<ServiceResponse> Pay(decimal totalamount, IEnumerable<Product> cartproducts, IEnumerable<ProcessCart> carts);
    }
}
