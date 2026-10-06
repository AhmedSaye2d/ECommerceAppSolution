using ECommerce.Infrastructure.DependencyInjection;
using ECommerceApp.Application.DependencyInjection;
using Serilog;

namespace ECommerce.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();
            Log.Logger.Information("Application is building .......");

            // Add essential services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Custom dependency injections
            builder.Services.AddInfrastructureService(builder.Configuration);
            builder.Services.AddApplicationService();

            // Enable CORS for frontend
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            try
            {
                var app = builder.Build();

                app.UseCors();
                app.UseSerilogRequestLogging();

                app.UseInfrastructure();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

               
                app.UseDefaultFiles(); 
                app.UseStaticFiles(); 

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                Log.Logger.Information("Application is Running .......");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Application failed to start......");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
