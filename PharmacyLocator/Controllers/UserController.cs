using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Data;

namespace PharmacyLocator.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        private long _UserId;
        private readonly IMedicineService _medservice;
        private readonly IStoreService _storeservice;
        private readonly IPharmacyService _pharmaservice;
        private readonly ILocationService _locservice;
        private readonly IUserService _userservice;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IMedicineService medservice, IStoreService storeService,IPharmacyService pharmacyService, ILocationService locationService,IUserService userservice, IWebHostEnvironment webHostEnvironment)
        {
            _medservice = medservice;
            _storeservice = storeService;
            _pharmaservice = pharmacyService;
            _locservice = locationService;
            _userservice = userservice;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SearchPage()
        {
            return View();
        }
        public async Task<string> medicinesHint(string q)
        {
            IEnumerable<Medicine> medicines =  await _medservice.getLikeName(q);
            string response = "";
            foreach (Medicine medicine in medicines)
            {
                response += "<option value='"+ medicine.Name +"'></option>";
            }
            return response;
        }

        public async Task<string> Search(string query)
        {
            Medicine m = new Medicine();
            m.Name = query;
            string response = "";

            if (await _medservice.NameExist(m))
            {
                long id = await _medservice.getIdByName(query);
                IEnumerable<Store> medicines = await _storeservice.getByMedID(id);
                if (medicines.Count() != 0)
                {
                    foreach (Store item in medicines)
                    {
                        Medicine medicine = await _medservice.GetByIdAsync(long.Parse(item.MedicineId.ToString()));
                        Pharmacy pharmacy = await _pharmaservice.GetByIdAsync(long.Parse(item.PharmacyId.ToString()));
                        Location location = await _locservice.GetByIdAsync(pharmacy.LocationId);

                        response += "<div class=\"col-md-4\">\r\n                <div class=\"card bg-new text-white\" title=\"This pharmacy have the medicine you're looking for.\">\r\n                    <img src=\"images/pharmacies/" + pharmacy.Image + "\" class=\"card-img-top\" alt=\"" + medicine.Name + " Image\" height='300px'>\r\n                    <div class=\"card-body\">\r\n                        <h5 class=\"card-title\">" + pharmacy.Name + "</h5>\r\n                        <h6 class=\"card-subtitle mb-2 text-muted\">" + location.Name + "</h6>\r\n                        <p class=\"card-text\">" + medicine.Description + "</p>\r\n                        <a class='showmap card-link text-decoration-none' data-bs-toggle=\"modal\" data-bs-target=\"#info\" data-link='" + pharmacy.MapLink + "'>Google Map</a>\r\n                    </div>\r\n                </div>\r\n            </div>";
                    }
                    response += "<div class=\"modal fade\" id=\"info\" tabindex=\"-1\" aria-labelledby=\"detailsModalLabel\" aria-hidden=\"true\">\r\n    <div class=\"modal-dialog modal-lg\">\r\n        <div class=\"modal-content\">\r\n            <div class=\"modal-header\">\r\n                <h5 class=\"modal-title text-center\" id=\"detailsModalLabel\">Pharmacy Location</h5>\r\n                <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>\r\n            </div>\r\n            <div class=\"modal-body\">\r\n                <iframe id=\"map-link\" src=\"\" width=\"100%\" height=\"450\" style=\"border:0;\" allowfullscreen=\"\" loading=\"lazy\"></iframe>\r\n            </div>\r\n            <div class=\"modal-footer\">\r\n                <button type=\"button\" class=\"btn btn-secondary\" data-bs-dismiss=\"modal\">Close</button>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<script>\r\n    $('.showmap').on('click', (e) => {\r\n        $(\"#map-link\").attr('src',e.currentTarget.attributes['data-link'].nodeValue)\r\n    })\r\n</script>";
                }
                else
                {
                    return "<center><h2>Medicine can not be located anywhere</h2></center>";
                }
            } else
            {
                return "<center><h2>Unknown Medicine</h2></center>";
            }
           
            return response;
        }

        public async Task<IActionResult> ChangePassword()
        {
            _UserId = await _userservice.getIdFromUsername(User.Claims.ToList()[0].Value);
            return View();
        }

        [HttpPost]
        public async Task<string> ChangePassword(string oldPass, string newPass)
        {
            try
            {
                _UserId = await _userservice.getIdFromUsername(User.Claims.ToList()[0].Value);
                User user = await _userservice.GetByIdAsync(_UserId);
                if (user != null)
                {
                    if (user.Password == oldPass)
                    {
                        if (newPass.Length >= 8)
                        {
                            user.Password = newPass;
                            await _userservice.UpdateAsync(user);
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
