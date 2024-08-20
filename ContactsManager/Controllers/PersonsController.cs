using ContactsManager.Filters.ActionFilters;
using ContactsManager.Filters.AuthorizationFilters;
using ContactsManager.Filters.ResultFilters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServicesContracts.DTOs;
using ServicesContracts.Enums;
using ServicesContracts.Interfaces;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]
    {
        "My-Key-From-Controller", "My-Value-From-Controller", 3
    }, Order = 3)]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController
            (IPersonsService personsService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            this._personsService = personsService;
            this._countriesService = countriesService;
            this._logger = logger;
        }


        [HttpGet]
        [Route("/")]
        [Route("[action]")]
        [TypeFilter(typeof(PersonsListActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]
        {
            "My-Key-From-Method", "My-Value-From-Method", 1
        }, Order = 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        public async Task<IActionResult> Index
            ([FromQuery] string searchBy, [FromQuery] string? searchString,
             [FromQuery] string sortBy, [FromQuery] SortOrderOptions sortOrderOption)
        {
            this._logger.LogInformation("Index action method of PersonsController");

            this._logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}");
            this._logger.LogDebug($"sortBy: {sortBy}, sortOrderOption: {sortOrderOption}");

            ViewBag.SearchOptions = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.Address), "Address" }
            };
            var persons = await this._personsService.GetFilteredPersons(searchBy, searchString);

            //ViewBag.CurrentSearchString = searchString;
            //ViewBag.CurrentSearchBy = searchBy;

            var sortedPersons = this._personsService.GetSortedPersons(persons, sortBy, sortOrderOption);

            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrderOption = sortOrderOption.ToString();

            return View(sortedPersons);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            var countries = await this._countriesService.GetAllCountries();

            ViewBag.Countries = countries;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter), Arguments = new string[]
        {
            "personAddRequest"
        })]
        public async Task<IActionResult> Create([FromForm] PersonAddRequest personAddRequest)
        {
            //if(!ModelState.IsValid)
            //{
            //    var countries = await this._countriesService.GetAllCountries();

            //    ViewBag.Countries = countries;
            //    ViewBag.Errors =  
            //        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            //    return View("create");  
            //}

            await this._personsService.AddPerson(personAddRequest);

            return RedirectToAction("index", "persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit([FromRoute] Guid personId)
        {
            var person = await this._personsService.GetPersonByPersonId(personId);
            if (person is null)
            {
                return RedirectToAction("Index", "Persons");
            }

            var personUpdateRequest = person.ToPersonUpdateRequest();

            var countries = await this._countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter), Arguments = new string[]
        {
            "personUpdateRequest"
        })]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit([FromForm] PersonUpdateRequest personUpdateRequest)
        {
            var person = await this._personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (person is null)
            {
                return RedirectToAction("Index", "Persons");
            }


            var personResponse = await this._personsService.UpdatePerson(personUpdateRequest);

            return RedirectToAction("Index", "Persons");

            //else
            //{
            //    var countries = await this._countriesService.GetAllCountries();
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            //    ViewBag.Countries = countries;
            //    ViewBag.Errors = errors;    

            //    return View(personUpdateRequest);
            //}
        }

        [HttpGet]
        [Route("[action]/{personId?}")]
        public async Task<IActionResult> Delete([FromRoute] Guid? personId)
        {
            var personResponse = await this._personsService.GetPersonByPersonId(personId);
            if (personResponse is null)
            {
                return RedirectToAction("Index", "Persons");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId?}")]
        public async Task<IActionResult> Delete([FromForm] PersonUpdateRequest personUpdateResult)
        {
            var personResponse = await this._personsService.GetPersonByPersonId(personUpdateResult.PersonId);

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            await this._personsService.DeletePerson(personResponse.PersonId);

            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            var persons = await this._personsService.GetAllPersons();

            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20,
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            var memoryStream = await this._personsService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            var memoryStream = await this._personsService.GetPersonsExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
