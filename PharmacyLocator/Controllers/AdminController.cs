﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Xml.Linq;

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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IAdminService service, IMedicineService medsevice, ILocationService locsevice,IPharmacyService pharmaService,IUserService userserivce,IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _medservice = medsevice;
            _locservice = locsevice;
            _pharmaservice = pharmaService;
            _userservice = userserivce;
            _webHostEnvironment = webHostEnvironment;
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
        
        public async Task<IActionResult> Dashboard()
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
            ViewBag.medCount = await _medservice.CountAsync();
            ViewBag.locCount = await _locservice.CountAsync();
            ViewBag.pharmaCount = await _pharmaservice.CountAsync();
            ViewBag.userCount = await _userservice.CountAsync();
            return View();
        }

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

        public async Task<IActionResult> Location()
        {
            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);
            var data = Request.Query["func"];
            long id = string.IsNullOrEmpty(Request.Query["id"]) ? 0 : Int64.Parse(Request.Query["id"]);
            ViewBag.data = data;
            ViewBag.id = id;
            if (data == "add") { return View(); }
            else if (data == "edit")
            {
                var locData = await _locservice.GetAllAsync();
                return View(locData);
            }
            else if (data == "remove")
            {
                var locData = await _locservice.GetAllAsync();
                return View(locData);
            }
            else
            {
                var locData = await _locservice.GetAllAsync();
                return View(locData);
            }
        }

        [HttpPost]
        public async Task<string> ModMedicine(Medicine medicine,IFormFile Image, string submit)
        {

            _userId = await _service.getIdFromUsername(User.Claims.ToList()[0].Value);

            if (string.IsNullOrEmpty(medicine.Name))
            {
                return "NameEmpty";
            }
            else
            {
                switch (submit)
                {
                    case "addMedicine":
                        if (Image == null)
                        {
                            return "imageError";
                        }
                        else
                        {
                            if (!(await _medservice.NameExist(medicine)))
                            {
                                string folder = "images/medicines/";
                                string file = Guid.NewGuid().ToString() + Image.FileName;
                                folder += file;
                                medicine.Image = file;
                                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                                await Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                                medicine.AddBy = _userId;
                                await _medservice.AddAsync(medicine);
                                return "success";
                            }
                            else
                            {
                                return "nameExist";
                            }
                        }
                        case "editMedicine":
                            Medicine mymedicine = await _medservice.GetByIdAsync(medicine.Id);
                            if (await _medservice.NameExist(medicine, true)) { 
                                if (Image == null)
                                {
                                    medicine.Image = mymedicine.Image;
                                    medicine.AddBy = _userId;
                                    await _medservice.UpdateAsync(medicine);
                                    return "editsuccess";
                                }
                                else
                                {
                                    string folder = "images/medicines/";
                                    string file = "[" + Guid.NewGuid().ToString() + "] " + Image.FileName;
                                    folder += file;
                                    medicine.Image = file;
                                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                                    await Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                                    medicine.AddBy = _userId;
                                    var fullPath = _webHostEnvironment.WebRootPath + "\\images\\medicines\\" + mymedicine.Image;
                                    if (System.IO.File.Exists(fullPath))
                                    {
                                        System.GC.Collect();
                                        System.GC.WaitForPendingFinalizers();
                                        System.IO.File.Delete(fullPath);
                                    }
                                    await _medservice.UpdateAsync(medicine);
                                    return "editsuccess";
                                }
                            }else {
                                return "nameExist";
                            }
                        default:
                            return "unknownTask";
                    }
            }
        }

        [HttpPost]
        public async Task<string> ModLocation(Location location,string submit)
        {
            try {
                switch (submit)
                {
                    case "addLocation":
                        if (!(await _locservice.NameExist(location)))
                        {
                            await _locservice.AddAsync(location);
                            return "locationSuccess";
                        }
                        else
                        {
                            return "locationNameExist";
                        }
                    case "editLocation":
                        Location loc = await _locservice.GetByIdAsync(location.Id);
                        if (loc != null)
                        {
                            if (!(await _locservice.NameExist(location)))
                            {
                                await _locservice.UpdateAsync(location);
                                return "editLocationSuccess";
                            }else
                            {
                                return "locationNameExist";
                            }
                        }
                        else
                        {
                            return "editLocationUnknownID";
                        }
                    default:
                        return "failure";
                }
            }
            catch(Exception e)
            {
                return "failure";
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
                        Medicine mymedicine = await _medservice.GetByIdAsync(id);

                        if (mymedicine != null)
                        {
                            var fullPath = _webHostEnvironment.WebRootPath + "\\images\\medicines\\" + mymedicine.Image;
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(fullPath);
                            }
                            await _medservice.DeleteAsync(id);
                            return "success";
                        }
                        else
                        {
                            return "unknownID";
                        }
                    case "removeLocation":
                        if (await _locservice.GetByIdAsync(id) != null)
                        {
                            await _locservice.DeleteAsync(id);
                            return "success";
                        }
                        else
                        {
                            return "unknownID";
                        }
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
