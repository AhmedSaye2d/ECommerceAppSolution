using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Dto.Product;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Service
{
    /// <summary>
    /// Integrates with Stripe Hosted Checkout API to generate secure payment sessions.
    /// Converts decimal currency amounts into cents and creates customizable line items.
    /// </summary>
    public class StripePaymentService : IPaymentService
    {
        public async Task<ServiceResponse> Pay(decimal totalamount, IEnumerable<Product> cartproducts, IEnumerable<ProcessCart> carts)
        {
            try
            {
                var lineItems = new List<SessionLineItemOptions>();

                foreach (var item in cartproducts)
                {
                    var pQuantity = carts.FirstOrDefault(e => e.ProductId == item.Id);
                    if (pQuantity == null) continue;

                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name ?? "Product",
                                Description = item.Description,
                            },
                            // Stripe requires unit amount in the smallest currency unit (cents for USD)
                            UnitAmount = (long)(item.Price * 100)
                        },
                        Quantity = pQuantity.Quantity,
                    });
                }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "https://localhost:44353/payment-success",
                    CancelUrl = "https://localhost:44353/payment-cancel"
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return new ServiceResponse(true, session.Url);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
