using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ActionFilters
{
    public class ResponseHeaderFilterUsingFactory : Attribute, IFilterFactory
    {
        private readonly string _key;
        private readonly string _value;
        private int _order;
        public bool IsReusable => false;
        public ResponseHeaderFilterUsingFactory(string key, string value, int order)
        {
            this._key = key;
            this._value = value;
            this._order = order;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = this._key;
            filter.Value = this._value;
            filter.Order = this._order; 

            return filter;
        }
    }
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string Key { get; set; } = "Default-Key";
        public string Value { get; set; } = "Default-Value";
        public int Order { get; set; }

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            this._logger = logger;
        }

        //public ResponseHeaderActionFilter
        //    (ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        //{
        //    this._logger = logger;
        //    this._key = key;
        //    this._value = value;
        //    this.Order = order;
        //}        

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
            context.HttpContext.Response.Headers[this.Key] = this.Value;
        }
    }
}
