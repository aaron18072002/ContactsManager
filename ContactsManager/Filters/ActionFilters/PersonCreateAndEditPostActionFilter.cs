using ContactsManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServicesContracts.Interfaces;

namespace ContactsManager.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;
        private readonly ICountriesService _countriesService;
        private string _personRequestName;
        public PersonCreateAndEditPostActionFilter
            (ILogger<PersonCreateAndEditPostActionFilter> logger,ICountriesService countriesService ,string personRequestName)
        {
            this._logger = logger;
            this._countriesService = countriesService;
            this._personRequestName = personRequestName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._logger.LogInformation("{FilterName}.{MethodName} method before",
                nameof(PersonsListActionFilter), nameof(this.OnActionExecutionAsync));
            if(context.Controller is PersonsController)
            {
                var personsController = context.Controller as PersonsController;
                if(personsController is not null && !personsController.ModelState.IsValid)
                {
                    var countries = await this._countriesService.GetAllCountries();

                    personsController.ViewBag.Countries = countries;
                    personsController.ViewBag.Errors = personsController.ModelState.Values
                        .SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    var personRequest = context.ActionArguments[this._personRequestName];

                    context.Result = personsController.View(personRequest);
                } else
                {
                    await next();
                }
            } else
            {
                await next();
            }

            this._logger.LogInformation("{FilterName}.{MethodName} method after",
                nameof(PersonsListActionFilter), nameof(this.OnActionExecutionAsync));
        }
    }
}
