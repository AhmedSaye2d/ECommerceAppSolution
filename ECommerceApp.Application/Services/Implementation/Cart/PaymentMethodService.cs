using AutoMapper;
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Interface.Cart;
namespace ECommerceApp.Application.Services.Implementation.Cart
{
    public class PaymentMethodService(IPaymentMethod paymentMethod,IMapper mapper) : IPaymentMethodService
    {
        public async Task<IEnumerable<GetPaymentMethod>> GetPaymentMethods()
        {
            var methods = await paymentMethod.GetPaymentMethods();
            if(!methods.Any()) return [];

            return mapper.Map<IEnumerable<GetPaymentMethod>>(methods);
        }
    }
}
