using ECommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.MiddleWare
{
    /// <summary>
    /// Centralized exception interceptor mapping low-level SQL Server and EF Core errors to standardized HTTP responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
                context.Response.ContentType = "application/json";

                if (ex.InnerException is SqlException innerException)
                {
                    logger.LogError(innerException, "SQL Server Database Exception Occurred");

                    switch (innerException.Number)
                    {
                        case 2627: // Unique Constraint / Primary Key Violation
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Unique constraint violation. The resource already exists."
                            }));
                            break;

                        case 515: // Null Value Violation
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "A required field was null."
                            }));
                            break;

                        case 547: // Foreign Key Constraint Violation
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Foreign key reference violation. Referenced resource does not exist."
                            }));
                            break;

                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "A database error occurred while processing your request."
                            }));
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Entity Framework Core Exception Occurred");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "An unexpected database update error occurred."
                    }));
                }
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
                logger.LogError(ex, "Unhandled Application Exception Occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = $"An unexpected error occurred: {ex.Message}"
                }));
            }
        }
    }
}
