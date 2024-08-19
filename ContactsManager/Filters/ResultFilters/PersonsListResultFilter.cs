using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            this._logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method before",
                nameof(PersonsListResultFilter), nameof(this.OnResultExecutionAsync));
            context.HttpContext.Response.Headers["Last-Modifed"] = DateTime.Now.ToString("dd-MM-yyyy-hh-mm");

            await next();

            this._logger.LogInformation("{FilterName}.{MethodName} method after",
                nameof(PersonsListResultFilter), nameof(this.OnResultExecutionAsync));          
        }
    }
}
