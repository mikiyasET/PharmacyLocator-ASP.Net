using Microsoft.AspNetCore.Mvc;

namespace PharmacyLocator.Controllers
{
    public class PharmacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
