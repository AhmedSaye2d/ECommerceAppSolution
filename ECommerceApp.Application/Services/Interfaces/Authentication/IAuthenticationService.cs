using ECommerceApp.Application.Dto;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Product;
namespace ECommerceApp.Application.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUser user);
        Task<LoginResponse> LoginUser(LoginUser user);
        Task<LoginResponse> ReviveToken(string refreshToken);

    }
}
