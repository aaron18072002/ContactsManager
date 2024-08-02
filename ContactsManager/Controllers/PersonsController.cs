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

            return View(allPersons);
        }
    }
}
