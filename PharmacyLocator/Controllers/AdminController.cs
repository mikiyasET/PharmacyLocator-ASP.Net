using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Net.Http.Headers;
using System.Security.Cryptography;

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
            long id = string.IsNullOrEmpty(Request.Query["id"]) ? 0 : Int64.Parse(Request.Query["id"]);
            ViewBag.data = data;
            ViewBag.id = id;
            if (data == "add") { return View(); }
            else if (data == "edit")
            {
                var medData = await _medservice.GetAllAsync();
                return View(medData); 
            }
            else if (data == "remove") {
                var medData = await _medservice.GetAllAsync();
                return View(medData);
            }
            else
            {
                var medData = await _medservice.GetAllAsync();
                return View(medData);
            }
        }


        [HttpPost]
        public async Task<string> CreateMedicine(Medicine medicine, IFormFile Image)
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);

            if (string.IsNullOrEmpty(medicine.Name))
            {
                return "NameEmpty";
            }
            else
            {
                var submit = this.Request.Form.ToList()[0].Key;
                var task = this.Request.Form.ToList()[0].Value.ToString();
                if (submit == "submit")
                {
                    switch (task)
                    {
                        case "addMedicine":
                            if (Image == null)
                            {
                                return "imageError";
                            }
                            else
                            {
                                Random random = new Random();
                                int randomNum = random.Next(10000, 99999);
                                var filename = "[" + randomNum + "] " + ContentDispositionHeaderValue.Parse(Image.ContentDisposition).FileName.Trim('"');
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "medicines", "[" + randomNum + "] " + Image.FileName);
                                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                                {
                                    await Image.CopyToAsync(stream);
                                }
                                medicine.Image = filename;
                                medicine.AddBy = _userId;

                                await _medservice.AddAsync(medicine);
                                return "success";
                            }
                            break;
                        case "editMedicine":
                            if (Image == null)
                            {
                                Medicine mymedicine = await _medservice.GetByIdAsync(medicine.Id);
                                medicine.Image = mymedicine.Image;
                                medicine.AddBy = _userId;
                                await _medservice.UpdateAsync(medicine.Id, medicine);
                                return "editsuccess";
                            }
                            else
                            {
                                Random random = new Random();
                                int randomNum = random.Next(10000, 99999);
                                _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
                                var filename = "[" + randomNum + "] " + ContentDispositionHeaderValue.Parse(Image.ContentDisposition).FileName.Trim('"');
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "medicines", "[" + randomNum + "] " + Image.FileName);
                                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                                {
                                    await Image.CopyToAsync(stream);
                                }
                                medicine.Image = filename;
                                medicine.AddBy = _userId;
                                await _medservice.UpdateAsync(medicine.Id, medicine);
                                return "editsuccess";
                            }
                            break;
                        default:
                            return "unknownTask";
                            break;
                    }
                    
                }else
                {
                    return "error";
                }
            }
        }

        [HttpDelete]
        public async Task<string> RemoveThem(long id,string submit)
        {
            try
            {
                switch(submit)
                {
                    case "removeMedicine":
                        if (await _medservice.GetByIdAsync(id) != null)
                        {
                            await _medservice.DeleteAsync(id);
                            return "success";
                        }
                        else
                        {
                            return "unknownID";
                        }
                        break;
                    default:
                        return "failure";
                }
            }catch
            {
                return "failure";
            }
        }
    }
}
