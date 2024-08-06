using Entities;
using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
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
        [Route("[action]")]
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
        [Route("[action]")]
        public IActionResult Create()
        {
            var countries = this._countriesService.GetAllCountries();

            ViewBag.Countries = countries;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
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

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Edit([FromRoute]Guid personId) 
        {
            var person = this._personsService.GetPersonByPersonId(personId);
            if(person is null)
            {
                return RedirectToAction("Index", "Persons");
            }
            
            var personUpdateRequest = person.ToPersonUpdateRequest();

            var countries = this._countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Edit([FromForm]PersonUpdateRequest personUpdateRequest)
        {
            var person = this._personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if(person is null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if(ModelState.IsValid)
            {
                var personResponse = this._personsService.UpdatePerson(personUpdateRequest);

                return RedirectToAction("Index", "Persons");
            }
            else
            {
                var countries = this._countriesService.GetAllCountries();
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                ViewBag.Countries = countries;
                ViewBag.Errors = errors;    

                return View(personUpdateRequest);
            }
        }

        [HttpGet]
        [Route("[action]/{personId?}")]
        public IActionResult Delete([FromRoute]Guid? personId)
        {
            var personResponse = this._personsService.GetPersonByPersonId(personId);
            if(personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId?}")]
        public IActionResult Delete([FromForm]PersonUpdateRequest personUpdateResult)
        {
            var personResponse = this._personsService.GetPersonByPersonId(personUpdateResult.PersonId);

            if(personResponse is null)
            {
                return RedirectToAction("Index");
            }

            this._personsService.DeletePerson(personResponse.PersonId);

            return RedirectToAction("Index","Persons");
        }
    }
}
