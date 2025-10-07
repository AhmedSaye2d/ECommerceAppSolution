using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Text.Json;
namespace ECommerce.Infrastructure.MiddleWare
{
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
                context.Response.ContentType = "application/json";

                if (ex.InnerException is SqlException innerException)
                {
                    switch (innerException.Number)
                    {
                        case 2627: 
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Unique constraint violation"
                            }));
                            break;

                        case 515: // Cannot insert null
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Cannot insert null value"
                            }));
                            break;

                        case 547: // Foreign key constraint violation
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "An error occurred while processing your request (foreign key violation)"
                            }));
                            break;

                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "A database error occurred"
                            }));
                            break;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "An unexpected database error occurred"
                    }));
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = $"An error occurred: {ex.Message}"
                }));
            }
        }
    }
}

