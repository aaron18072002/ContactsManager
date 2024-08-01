using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Controllers
{
    public class PersonsController : Controller
    {
        [Route("/")]
        [Route("persons/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
