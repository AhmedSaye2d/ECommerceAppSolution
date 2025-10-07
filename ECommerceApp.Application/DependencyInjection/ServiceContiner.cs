using ECommerceApp.Application.Mapping;
using ECommerceApp.Application.Services.Implementation;
using ECommerceApp.Application.Services.Interfaces;
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
            return services;    
        }

    }
}
