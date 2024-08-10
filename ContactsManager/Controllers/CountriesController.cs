using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
    }
}
