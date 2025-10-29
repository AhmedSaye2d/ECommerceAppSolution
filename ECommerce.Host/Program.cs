using ECommerce.Infrastructure.DependencyInjection;
using ECommerceApp.Application.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                        .AllowAnyOrigin(); // استخدم AllowAnyOrigin لو front والback نفس الجهاز
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

                // ✅ مهم جدًا علشان يقدر يقرأ ملفات الواجهة الأمامية (index.html و css و js)
                app.UseDefaultFiles();  // بيدور تلقائيًا على index.html
                app.UseStaticFiles();   // يخلي السيرفر يقدّم ملفات wwwroot

                app.UseHttpsRedirection();
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
