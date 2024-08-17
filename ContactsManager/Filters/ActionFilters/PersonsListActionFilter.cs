using ContactsManager.Controllers;
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
            this._logger.LogInformation("{FilterName}.{MethodName} method", 
                nameof(PersonsListActionFilter), nameof(this.OnActionExecuted));

            var personsController = (PersonsController)context.Controller;
            var parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if(parameters is not null)
            {
                if(parameters.ContainsKey("searchBy"))
                {
                    personsController.ViewBag.CurrentSearchBy = Convert.ToString(parameters["searchBy"]);
                }
                if (parameters.ContainsKey("searchString"))
                {
                    personsController.ViewBag.CurrentSearchBy = Convert.ToString(parameters["searchString"]);
                }
                if (parameters.ContainsKey("sortBy"))
                {
                    personsController.ViewBag.CurrentSortBy = Convert.ToString(parameters["sortBy"]);
                }
                if (parameters.ContainsKey("sortOrderOption"))
                {
                    personsController.ViewBag.CurrentSortOrderOption = Convert.ToString(parameters["sortOrderOption"]);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method",
                nameof(PersonsListActionFilter), nameof(this.OnActionExecuting));

            context.HttpContext.Items["arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
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
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    this._logger.LogInformation($"Updated searchBy value: {context.ActionArguments["searchBy"]}");
                }
            }
        }
    }
}
