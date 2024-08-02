using Entities;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var allPersons = this._personsService.GetAllPersons();
            ViewBag.SearchOptions = new Dictionary<string, string>()
            {
                { nameof(Person.PersonName), "Person Name" },
                { nameof(Person.Email), "Email" },
                { nameof(Person.DateOfBirth), "Date of birth" },
                { nameof(Person.Gender), "Gender" },
                { nameof(Person.Address), "Address" }
            };

            return View(allPersons);
        }
    }
}
