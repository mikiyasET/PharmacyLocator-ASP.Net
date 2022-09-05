using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Data;


namespace PharmacyLocator.Controllers
{
    [Authorize(Roles = "pharmacy")]
    public class PharmacyController : Controller
    {
        private long _pharmaId;
        private readonly IMedicineService _medservice;
        private readonly IPharmacyService _pharmaservice;
        private readonly IRecordService _recordservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILocationService _locservice;
        private readonly IUserService _userservice;
        private readonly IStoreService _storeService;
        public PharmacyController(IMedicineService medicineService,IPharmacyService pharmacyService,IRecordService recordService,IWebHostEnvironment webHostEnvironment,ILocationService locationService,IUserService userService,IStoreService storeService) { 
            _medservice = medicineService;
            _pharmaservice = pharmacyService;
            _recordservice = recordService;
            _locservice = locationService;
            _storeService = storeService;
            _userservice = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
            ViewBag.medCount = await _medservice.CountAsync();
            ViewBag.locCount = await _locservice.CountAsync();
            ViewBag.pharmaCount = await _pharmaservice.CountAsync();
            ViewBag.userCount = await _userservice.CountAsync();
            return View("Index");
        }

        public async Task<IActionResult> Dashboard()
        {
            _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
            ViewBag.medCount = await _medservice.CountAsync();
            ViewBag.locCount = await _locservice.CountAsync();
            ViewBag.pharmaCount = await _pharmaservice.CountAsync();
            ViewBag.userCount = await _userservice.CountAsync();
            return View();
        }

        public async Task<IActionResult> Store()
        {
            _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
            var data = Request.Query["func"];
            long id = string.IsNullOrEmpty(Request.Query["id"]) ? 0 : Int64.Parse(Request.Query["id"]);
            ViewBag.data = data;
            ViewBag.id = id;
            if (data == "add") {
                IEnumerable<Medicine> medicines = await _storeService.GetMedNotInStore(_pharmaId);
                return View(medicines);
            }
            else
            {
                IEnumerable<Store> store = (await _storeService.GetAllAsync()).Where((stores) => stores.PharmacyId == _pharmaId);
                return View(store);
            }
        }

        public async Task<IActionResult> LeadBoard()
        {
            IEnumerable<Record> record = await _recordservice.GetAllAsync();
            return View(record);
        }

        public async Task<IActionResult> ChangePassword()
        {
            _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
            return View();
        }

        [HttpPost]
        public async Task<string> ChangePassword(string oldPass, string newPass, string submit)
        {
            try
            {
                if (submit == "changePassword")
                {
                    _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
                    Pharmacy pharma = await _pharmaservice.GetByIdAsync(_pharmaId);
                    if (pharma != null)
                    {
                        if (pharma.Password == oldPass)
                        {
                            if (newPass.Length >= 8)
                            {
                                pharma.Password = newPass;
                                await _pharmaservice.UpdateAsync(pharma);
                                return "success";
                            }
                            else
                            {
                                return "passwordInvalid";
                            }
                        }
                        else
                        {
                            return "currentError";
                        }
                    }
                    else
                    {
                        return "error";
                    }
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception e)
            {
                return "Error: " + e;
            }
        }
        
    }
}
