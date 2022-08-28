using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyLocator.Models.Services;

namespace PharmacyLocator.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private long _userId;
        private readonly IAdminService _service;
        private readonly IMedicineService _medservice;
        private readonly ILocationService _locservice;
        private readonly IPharmacyService _pharmaservice;
        private readonly IUserService _userservice;
        public AdminController(IAdminService service, IMedicineService medsevice, ILocationService locsevice,IPharmacyService pharmaService,IUserService userserivce)
        {
            _service = service;
            _medservice = medsevice;
            _locservice = locsevice;
            _pharmaservice = pharmaService;
            _userservice = userserivce;
        }
        public async Task<IActionResult> Index()
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
            ViewBag.medCount = await _medservice.CountAsync();
            ViewBag.locCount = await _locservice.CountAsync();
            ViewBag.pharmaCount = await _pharmaservice.CountAsync();
            ViewBag.userCount = await _userservice.CountAsync();
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
            ViewBag.medCount = await _medservice.CountAsync();
            ViewBag.locCount = await _locservice.CountAsync();
            ViewBag.pharmaCount = await _pharmaservice.CountAsync();
            ViewBag.userCount = await _userservice.CountAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Medicine()
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
            var data = Request.Query["func"];
            var id = Request.Query["id"];
            ViewBag.data = data;
            ViewBag.id = id;
            if (data == "add") { return View(); }
            else if (data == "edit") { return View(); }
            else if (data == "remove") { return View(); }
            else
            {
                var medData = await _medservice.GetAllAsync();
                return View(medData);
            }
        }
    }
}
