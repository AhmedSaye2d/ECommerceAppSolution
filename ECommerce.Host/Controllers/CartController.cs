using ECommerceApp.Application.Dto.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckOut checkOut)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Get userId from the authenticated user context
            var userId = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User is not authenticated.");

            var result = await cartService.CheckOut(checkOut);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveChekout(IEnumerable<CreateAchieve> createAchieves)
        {
            var result=await cartService.SaveCheckoutHistory(createAchieves);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
