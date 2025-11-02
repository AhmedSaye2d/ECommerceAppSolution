using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Product;
namespace ECommerceApp.Application.Services.Interfaces.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateAchieve> checkouts);
        Task<ServiceResponse> CheckOut(CheckOut checkOut);
        Task<ServiceResponse> GetAchieve(CheckOut checkOut);
        
    }
}
