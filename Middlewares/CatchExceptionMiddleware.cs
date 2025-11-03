using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Resumai.Exceptions;

namespace Resumai.Middlewares
{
    public class CatchExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CatchExceptionMiddleware> _logger;

        public CatchExceptionMiddleware(RequestDelegate next, ILogger<CatchExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogWarning(nfEx, "Resource not found.");

                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = nfEx.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (DomainException dEx)
            {
                _logger.LogWarning(dEx, "Domain error occurred.");

                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = dEx.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (ConflictException cEx)
            {
                _logger.LogWarning(cEx, "Conflict occurred.");

                context.Response.StatusCode = 409;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = cEx.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (UnauthorizedAccessException uEx)
            {
                _logger.LogWarning(uEx, "Unauthorized access.");

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = uEx.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
        
    }
}