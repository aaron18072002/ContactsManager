using ContactsManager.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.AuthorizationFilters
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<TokenAuthorizationFilter> _logger;
        public TokenAuthorizationFilter(ILogger<TokenAuthorizationFilter> logger)
        {
            this._logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method",
                nameof(TokenAuthorizationFilter), nameof(this.OnAuthorization));
            if(context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            if (context.HttpContext.Request.Cookies["Auth-Key"] == "Auth-Value")
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
        }
    }
}
