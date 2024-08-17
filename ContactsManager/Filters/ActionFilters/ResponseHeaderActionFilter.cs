using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IActionFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;
        public ResponseHeaderActionFilter
            (ILogger<ResponseHeaderActionFilter> logger, string key, string value)
        {
            this._logger = logger;
            this._key = key;
            this._value = value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method", 
                nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecuted));
            context.HttpContext.Response.Headers[this._key] = this._value;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method",
                nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecuting));
        }
    }
}
