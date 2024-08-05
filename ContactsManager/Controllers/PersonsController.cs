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
        private ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            this._personsService = personsService;
            this._countriesService = countriesService;
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

        [HttpGet]
        [Route("persons/create")]
        public IActionResult Create()
        {
            var countries = this._countriesService.GetAllCountries();

            ViewBag.Countries = countries;

            return View();
        }

        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create([FromForm] PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                var countries = this._countriesService.GetAllCountries();

                ViewBag.Countries = countries;
                ViewBag.Errors =  
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("create");  
            }

            this._personsService.AddPerson(personAddRequest);

            return RedirectToAction("index", "persons");
        }
    }
}
