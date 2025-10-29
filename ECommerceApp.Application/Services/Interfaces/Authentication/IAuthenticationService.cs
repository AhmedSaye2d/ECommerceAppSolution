using ECommerceApp.Application.Dto;
using ECommerceApp.Application.Dto.Identity;
using ECommerceApp.Application.Dto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUser user);
        Task<LoginResponse> LoginUser(LoginUser user);
        Task<LoginResponse> ReviveToken(string refreshToken);

    }
}
