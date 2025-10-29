using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Implementation.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace ECommerce.Infrastructure.Service
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<ServiceResponse> Pay(decimal totalamount, IEnumerable<Product> cartproducts, IEnumerable<ProcessCart> carts)
        {
            try
            {
                var lineitem = new List<SessionLineItemOptions>();
                foreach (var item in cartproducts)
                {
                    var pQuantity = carts.FirstOrDefault(e => e.ProductId == item.Id);
                    lineitem.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                                Description = item.Description,
                            },
                            UnitAmount = (long)(item.Price * 100)
                        },
                        Quantity = pQuantity!.Quantity,
                    });
                }
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = ["usd"],
                    LineItems = lineitem,
                    Mode = "payment",
                    SuccessUrl = "https://localhost:44353/payment-success",
                    CancelUrl = "https://localhost:44353/payment-cancel"
                };
                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                return new ServiceResponse(true, session.Url);
            }catch(Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}

