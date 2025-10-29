using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interface.Cart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repository.Cart
{
    public class PaymentMethodRepository(AppDbContext context) : IPaymentMethod
    {
        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
        {
            return await context.PaymentMethods.AsNoTracking().ToListAsync();
        }
    }
}
