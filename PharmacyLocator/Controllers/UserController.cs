using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PharmacyLocator.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
