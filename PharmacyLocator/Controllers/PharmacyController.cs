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
        private readonly IRequestService _requestService;
        public PharmacyController(IMedicineService medicineService,IPharmacyService pharmacyService,IRecordService recordService,IWebHostEnvironment webHostEnvironment,ILocationService locationService,IUserService userService,IStoreService storeService,IRequestService requestService) { 
            _medservice = medicineService;
            _pharmaservice = pharmacyService;
            _recordservice = recordService;
            _locservice = locationService;
            _storeService = storeService;
            _userservice = userService;
            _webHostEnvironment = webHostEnvironment;
            _requestService = requestService;
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

        [HttpPost]
        public async Task<string> AddStore(long id) {
           try
            {
                _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
                List<Store> checkStore = (await _storeService.GetAllAsync()).Where((store) => store.MedicineId == id && store.PharmacyId == _pharmaId).ToList();
                if (checkStore.Count == 0)
                {
                    Store store = new Store();
                    store.PharmacyId = _pharmaId;
                    store.MedicineId = id;
                    await _storeService.AddAsync(store);
                    return "success";
                }else
                {
                    return "medExist";
                }
            }catch (Exception e)
            {
                return "failure";
            }
        }
        [HttpDelete]
        public async Task<string> RemoveStore(long id)
        {
            try
            {
                if ((await _storeService.GetByIdAsync(id)) != null)
                {
                    await _storeService.DeleteAsync(id);
                    return "success";
                }
                else
                {
                    return "unknownId";
                }
            }catch (Exception e )
            {
                return "failure: " + e;
            }
        }
        public async Task<IActionResult> LeadBoard()
        {
            IEnumerable<Record> record = await _recordservice.getRecords();
            return View(record);
        }
        public IActionResult Requests()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> RequestMedicine(String name)
        {
            try { 
                if (!(await _requestService.NameExist(name)))
                {
                    Requests request = new Requests();
                    request.Name = name;
                    await _requestService.AddAsync(request);
                    return "success";
                }
                else
                {
                    return "NameExist";
                }
            }
            catch (Exception e)
            {
                return "failure";
            }
        }
        public async Task<IActionResult> ChangePassword()
        {
            _pharmaId = await _pharmaservice.getIdFromEmail(User.Claims.ToList()[0].Value);
            return View();
        }

        [HttpPost]
        public async Task<string> ChangePassword(string oldPass, string newPass)
        {
            try
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
            catch (Exception)
            {
                return "error";
            }
        }
        
    }
}
