using Microsoft.AspNetCore.Mvc.Filters;
using ServicesContracts.DTOs;

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
            if(context.ActionArguments.ContainsKey("searchBy"))
            {
                var searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                var searchOptions = new List<string>()
                {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.Address),
                };
                if(!searchOptions.Any(p => p == searchBy))
                {
                    this._logger.LogInformation($"Actual searchBy value: {searchBy}");
                    context.ActionArguments["seachBy"] = nameof(PersonResponse.PersonName);
                    this._logger.LogInformation($"Updated searchBy value: {context.ActionArguments["seachBy"]}");
                }
            }
        }
    }
}
