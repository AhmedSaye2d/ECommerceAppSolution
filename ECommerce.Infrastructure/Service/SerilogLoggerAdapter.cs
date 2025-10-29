using ECommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Service
{
    public class SerilogLoggerAdapter<T>(ILogger<T> logger) : IAppLogger<T>
    {
        public void LogError(Exception Ex,string message)=>logger.LogError(Ex,message);
        public void LogInformation(string message)=>logger.LogInformation(message);
        public void LogWarning(string message)=>logger.LogWarning(message);
    }
}
