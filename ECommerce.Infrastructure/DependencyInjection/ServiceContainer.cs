using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.MiddleWare;
using ECommerce.Infrastructure.Repository;
using ECommerce.Infrastructure.Repository.Authentication;
using ECommerce.Infrastructure.Repository.Cart;
using ECommerce.Infrastructure.Service;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interface;
using ECommerceApp.Domain.Interface.Authentication;
using ECommerceApp.Domain.Interface.Authentication.ECommerceApp.Domain.Interface.Authentication;
using ECommerceApp.Domain.Interface.Cart;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            string connectionName = "Default";
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString(connectionName),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure();
                    }).UseExceptionProcessor(),

                ServiceLifetime.Scoped);
            services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
            services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
            services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));
            services.AddDefaultIdentity<AppUser> (options=>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider=TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    RequireAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagement>();
            services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
            services.AddScoped<IPaymentService,StripePaymentService>();
            Stripe.StripeConfiguration.ApiKey = config["Stripe:Secretkey"];

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

    }
}
