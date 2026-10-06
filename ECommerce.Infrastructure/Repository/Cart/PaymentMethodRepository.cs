using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interface.Cart;
using Microsoft.EntityFrameworkCore;
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
