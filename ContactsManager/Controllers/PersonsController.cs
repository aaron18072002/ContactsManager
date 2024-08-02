using Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;

namespace ContactsManager.Controllers
{
    public class PersonsController : Controller
    {
        private IPersonsService _personsService;
        public PersonsController(IPersonsService personsService)
        {
            this._personsService = personsService;
        }

        [Route("/")]
        [Route("persons/index")]
        public IActionResult Index
            ([FromQuery]string searchBy, [FromQuery]string? searchString,
             [FromQuery]string sortBy, [FromQuery]SortOrderOptions sortOrderOption)
        {
            ViewBag.SearchOptions = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.Address), "Address" }
            };
            var persons = this._personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSearchBy = searchBy;

            var sortedPersons = this._personsService.GetSortedPersons(persons, sortBy, sortOrderOption);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrderOption = sortOrderOption.ToString();

            return View(sortedPersons);
        }
    }
}
