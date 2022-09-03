using Microsoft.AspNetCore.Mvc;

namespace PharmacyLocator.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
