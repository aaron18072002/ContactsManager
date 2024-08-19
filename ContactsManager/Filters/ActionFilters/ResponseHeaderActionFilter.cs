using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;
        public int Order { get; set; }
        public ResponseHeaderActionFilter
            (ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        {
            this._logger = logger;
            this._key = key;
            this._value = value;
            this.Order = order;
        }        

        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    this._logger.LogInformation("{FilterName}.{MethodName} method", 
        //        nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecuted));
        //    context.HttpContext.Response.Headers[this._key] = this._value;
        //}

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    this._logger.LogInformation("{FilterName}.{MethodName} method",
        //        nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecuting));
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method executing",
                nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecutionAsync));

            await next();

            this._logger.LogInformation("{FilterName}.{MethodName} method executed",
                nameof(ResponseHeaderActionFilter), nameof(this.OnActionExecutionAsync));
            context.HttpContext.Response.Headers[this._key] = this._value;
        }
    }
}
