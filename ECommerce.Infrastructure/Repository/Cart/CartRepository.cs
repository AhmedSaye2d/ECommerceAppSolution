using ECommerce.Infrastructure.Data;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interface.Cart;
namespace ECommerce.Infrastructure.Repository.Cart
{
    public class CartRepository(AppDbContext context) : ICart
    {
        public async Task<int> SaveCheckoutHistory(IEnumerable<Achieve> checkout)
        {
            context.CheckoutAchieves.AddRange(checkout);   
            return await context.SaveChangesAsync();
        }
    }
}
