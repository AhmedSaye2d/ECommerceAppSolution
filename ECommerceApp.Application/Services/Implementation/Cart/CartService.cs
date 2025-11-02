using AutoMapper;
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interface;
using ECommerceApp.Domain.Interface.Cart;

namespace ECommerceApp.Application.Services.Implementation.Cart
{
    public class CartService(ICart cartinterface, IMapper mapper,IGeneric<Product> productinterface,IPaymentMethodService paymentMethodService,IPaymentService paymentService) : ICartService
    {
        public async Task<ServiceResponse> CheckOut(CheckOut checkOut)
        {
            var (product, totalAmount) = await Gettotalamount(checkOut.Carts);
            var paymentmethod = await paymentMethodService.GetPaymentMethods();
            if (paymentmethod.Any(m => m.Id == checkOut.PaymentMethodId))
                return await paymentService.Pay(totalAmount, product, checkOut.Carts);
            else return new ServiceResponse(false, "Invalid payment method");
        }

        public Task<ServiceResponse> GetAchieve(CheckOut checkOut)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateAchieve> achieves)
        {
            var mappedData = mapper.Map<IEnumerable<Achieve>>(achieves);
            var result = await cartinterface.SaveCheckoutHistory(mappedData);
            return result > 0 ? new ServiceResponse(true, "checkout achieved") :
                new ServiceResponse(false, "Error occured in saving");

        }
        private async Task<(IEnumerable<Product>, decimal)> Gettotalamount(IEnumerable<ProcessCart> carts)
        { 
            if(!carts.Any()) return ([],0);
           var products=await productinterface.GetAllAsync();   
            if(!products.Any()) return ([],0);
            var cartproducts = carts
                .Select(cartitem => products.FirstOrDefault(e => e.Id == cartitem.ProductId))
                .Where(product=>product!=null).ToList();
            var totalAmount = carts
                .Where(cartitem=>cartproducts.Any(e=>e.Id == cartitem.ProductId))
                .Sum(cartitem=>cartitem.Quantity*cartproducts.First(e=>e.Id==cartitem.ProductId)!.Price);
            return (products, totalAmount);

        }
    }
}
