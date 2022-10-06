using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PharmacyLocator.Controllers
{
    [Route("api/app")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private long _UserId;
        private readonly IMedicineService _medservice;
        private readonly IStoreService _storeservice;
        private readonly IPharmacyService _pharmaservice;
        private readonly ILocationService _locservice;
        private readonly IUserService _userservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IRecordService _recordService;

        public AppController(IMedicineService medservice, IStoreService storeService, IPharmacyService pharmacyService, ILocationService locationService, IUserService userservice, IWebHostEnvironment webHostEnvironment,IConfiguration configuration, IRecordService recordService)
        {
            _medservice = medservice;
            _storeservice = storeService;
            _pharmaservice = pharmacyService;
            _locservice = locationService;
            _userservice = userservice;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _recordService = recordService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("API Working...");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromForm] String username, [FromForm] String password)
        {
            if(await _userservice.Login(username, password))
            {
                User user = await _userservice.GetByIdAsync(await _userservice.getIdFromUsername(username));
                return Ok(GenerateJWTToken(user));
            }else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> signup([FromForm] String name, [FromForm] String username, [FromForm] String password)
        {
            try
            {
                if (password.Length >= 8)
                {
                    User user = new User();
                    user.Name = name;
                    user.Username = username;
                    user.Password = password;
                    await _userservice.AddAsync(user);
                    return new OkObjectResult(new { status = 200, message = "Success" });
                }
                else
                {
                    return new BadRequestObjectResult(new { status = 400, message = "Password Length must be at least 8" });
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { status = 400, message = e.InnerException.Message.Contains("IX_users_Username") ? "Username Exists" : "Unknown Exception"});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetMedicines")]
        public async Task<IActionResult> medList()
        {
            try
            {
                IEnumerable<Medicine> medicines = await _medservice.GetAllAsync();
                var medicinesName = medicines.Select(x => x.Name); ;
                return new OkObjectResult(new {status= 200,medicines = medicinesName });
            }
            catch
            {
                return new BadRequestObjectResult(new { status = 400 });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetMedicine")]
        public async Task<IActionResult> medList([FromQuery] String name)
        {
            try
            {
                long medId = await _medservice.getIdByName(name);
                Medicine medicine = await _medservice.GetByIdAsync(medId);
                return new OkObjectResult(new { status = 200, medicine = medicine });
            }
            catch
            {
                return new BadRequestObjectResult(new { status = 400 });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] String current, [FromForm] String password, [FromForm] long id)
        {
            try
            {
                User user = await _userservice.GetByIdAsync(id);
                if (user.Password == current)
                {
                    user.Password = password;
                    await _userservice.UpdateAsync(user);
                    return new OkObjectResult(new { status = 200, message = "success" });
                }
                else
                {
                    return new BadRequestObjectResult(new { status = 400, message="Current password not correct." });
                }
                
            }
            catch
            {
                return new BadRequestObjectResult(new { status = 400 , message="Unexpected Error"});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetPharmacies")]
        public async Task<IActionResult> pharmaList([FromQuery] String name, [FromQuery] long userId)
        {
            Medicine med = new Medicine();
            med.Name = name;
            if (await _medservice.NameExist(med))
            {
                long id = await _medservice.getIdByName(name);
                
                try
                {
                    Record record = new Record();
                    record.MedicineId = id;
                    record.UserId = userId;
                    var rec = await _recordService.checkRecord(_UserId, id);
                    if ((rec) != null)
                    {
                        record.Id = rec.Id;
                        record.Count = rec.Count + 1;
                        await _recordService.UpdateAsync(record);
                    }
                    else
                    {
                        record.Count = 1;
                        await _recordService.AddAsync(record);
                    }
                }
                catch (Exception)
                {

                }

                IEnumerable<Store> medicines = await _storeservice.getByMedID(id);
                if (medicines.Count() != 0)
                {
                    List<dynamic> response = new List<dynamic>();
                    foreach (Store item in medicines)
                    {
                        Medicine medicine = await _medservice.GetByIdAsync(long.Parse(item.MedicineId.ToString()));
                        Pharmacy pharmacy = await _pharmaservice.GetByIdAsync(long.Parse(item.PharmacyId.ToString()));
                        Location location = await _locservice.GetByIdAsync(pharmacy.LocationId);
                        response.Add(new {Name=pharmacy.Name,description=pharmacy.Description,image = pharmacy.Image,lat = pharmacy.Latitude,lng= pharmacy.Longitude,location = location.Name });
                    }
                    return new OkObjectResult(new { status = 200, data = response });
                }
                else
                {
                    return new BadRequestObjectResult(new { status = 400, message = "Medicine can not be located anywhere" });
                }
            }
            else
            {
                return new BadRequestObjectResult(new {status= 400, message= "Unknown Medicine"});
            }
        }
        
        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim("username", user.Username));
            claims.Add(new Claim("name", user.Name));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(_configuration["JWT:Issuer"], //Issure    
                            _configuration["JWT:Issuer"],  //Audience    
                            claims,
                            expires: DateTime.Now.AddDays(7),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}
