using ECommerceApp.Application.Dto.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Services.Interfaces.Cart
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<GetPaymentMethod>> GetPaymentMethods();
    }
}
