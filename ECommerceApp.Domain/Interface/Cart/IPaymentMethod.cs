using ECommerceApp.Domain.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Domain.Interface.Cart
{
    public interface IPaymentMethod
    {
        Task<IEnumerable<PaymentMethod>> GetPaymentMethods();
    }
}
