using ECommerceApp.Application.Mapping;
using ECommerceApp.Application.Services.Implementation;
using ECommerceApp.Application.Services.Implementation.Authentication;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using ECommerceApp.Application.Validation;
using ECommerceApp.Application.Validation.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace ECommerceApp.Application.DependencyInjection
{
    public static class ServiceContiner
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IproductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();


            return services;    

        }

    }
}
