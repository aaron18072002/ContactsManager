using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ExceptionFilters
{
    public class HandleExceptionMethod : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionMethod> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public HandleExceptionMethod
            (ILogger<HandleExceptionMethod> logger, IHostEnvironment hostEnvironment)
        {
            this._logger = logger;
            this._hostEnvironment = hostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            this._logger.LogError("{FilterName}.{MethodName}.{ExceptionType}.{ExceptionMessage} method",
                nameof(HandleExceptionMethod), nameof(this.OnException), 
                context.Exception.GetType().ToString(), nameof(context.Exception.Message));

            if(!this._hostEnvironment.IsDevelopment())
            {
                return ;
            }

            context.Result = new ContentResult()
            {
                Content = context.Exception.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
