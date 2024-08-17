using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            this._logger.LogInformation("OnActionExecuted method of PersonsListActionFilter");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger.LogInformation("OnActionExecuting method of PersonsListActionFilter");
        }
    }
}
