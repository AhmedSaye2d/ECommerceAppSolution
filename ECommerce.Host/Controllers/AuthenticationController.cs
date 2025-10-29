using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(CreateUser user)
        {
            var res = await _authenticationService.CreateUser(user);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginUser user)
        {
            var res = await _authenticationService.LoginUser(user);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var res = await _authenticationService.ReviveToken(refreshToken);
            return res.Success ? Ok(res) : BadRequest(res);
        }
    }
}
