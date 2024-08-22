using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ContactsManager.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExeptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionHandlingMiddleware> _logger;

        public ExeptionHandlingMiddleware(RequestDelegate next, ILogger<ExeptionHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            } catch(Exception ex)
            {
                if(ex.InnerException is not null)
                {
                    this._logger.LogError("{ExceptionType}.{ExceptionMessage}",
                        ex.InnerException.GetType().ToString(), ex.InnerException.Message);                    
                } else
                {
                    this._logger.LogError("{ExceptionType}.{ExceptionMessage}",
                        ex.GetType().ToString(), ex.Message);
                }
            }
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync("Error occured");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExeptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExeptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExeptionHandlingMiddleware>();
        }
    }
}
