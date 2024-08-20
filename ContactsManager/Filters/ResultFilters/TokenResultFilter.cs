using ContactsManager.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ResultFilters
{
    public class TokenResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<TokenResultFilter> _logger;
        public TokenResultFilter(ILogger<TokenResultFilter> logger)
        {
            this._logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method before",
                nameof(TokenResultFilter), nameof(this.OnResultExecutionAsync));
            context.HttpContext.Response.Cookies.Append("Auth-Key", "Auth-Value");

            await next();

            this._logger.LogInformation("{FilterName}.{MethodName} method after",
                nameof(TokenResultFilter), nameof(this.OnResultExecutionAsync));           
        }
    }
}
