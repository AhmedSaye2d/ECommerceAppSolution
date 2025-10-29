
using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentMethodService paymentMethodService ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GetPaymentMethod>>> GetPaymentMethod()
        {
            var method=await paymentMethodService.GetPaymentMethods();
            if (!method.Any())
            {
                return NotFound();  
            }
            else
            {
                return Ok(method);
            }
        }   
    }
}
