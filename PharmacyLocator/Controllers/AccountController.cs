using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Security.Claims;

namespace PharmacyLocator.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAdminService _adminservice;
        private readonly IPharmacyService _pharmaservice;
        private readonly IUserService _userservice;


        public AccountController(IAdminService adminservice, IPharmacyService pharmaservice, IUserService userservice)
        {
            _adminservice = adminservice;
            _pharmaservice = pharmaservice;
            _userservice = userservice;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [Route("Login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }
        
        [Route("SignUp")]
        public IActionResult SignUpForUsers() {
            return View();
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUpForUsers(User user,string cpassword)
        {
            try
            {
                if (!(await _userservice.checkUsername(user.Username)))
                {
                    if (user.Password.Length >= 8 && cpassword.Length >= 8)
                    {
                        if (user.Password == cpassword)
                        {
                            if (user.Name.Length > 3)
                            {
                                await _userservice.AddAsync(user);
                                return RedirectToAction("Login");
                            }
                            else
                            {
                                ViewBag.errorMessage = "Please enter a valid name";
                            }
                        }
                        else
                        {
                            ViewBag.errorMessage = "Password doesn't match";
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = "Password length must be 8 or more characters";
                    }
                }
                else
                {
                    ViewBag.errorMessage = "username already taken";
                }

                ViewBag.name = user.Name;
                ViewBag.username = user.Username;
            }
            catch (Exception e)
            {
                if (e.Message == "Object reference not set to an instance of an object.")
                {
                    ViewBag.errorMessage = "Error! There is an empty field.";
                }else
                {
                    ViewBag.errorMessage = "Unexpected error please try again later " + e;
                }
            }

            return View();
        }

        private async Task<bool> ValidateLogin(string userName, string password, string who)
        {
            if (who == "admin")
            {
                if (await _adminservice.Login(userName, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }   
            }
            else if (who == "pharmacy")
            {
                if (await _pharmaservice.Login(userName, password))
                {
                    return true;
                }else
                {
                    return false;
                }
            }
            else
            {
                if (await _userservice.Login(userName,password))
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string userName = "", string password = "", string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            var whoisLog = string.IsNullOrEmpty(returnUrl) ? null : returnUrl.Substring(1);
            // Normally Identity handles sign in, but you can do it directly
            whoisLog = string.IsNullOrEmpty(whoisLog) ? "user" : whoisLog;
            if (await ValidateLogin(userName, password, whoisLog))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", whoisLog)
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Redirect("/user");
                }
            }else
            {
                if (returnUrl == "/pharmacy")
                {
                    ViewBag.errorMessage = "Invalid Email or password.";
                }else
                {
                    ViewBag.errorMessage = "Invalid username or password.";
                }
                
                ViewBag.username = userName;
                return View();
            }
        }
        
        [Route("denied")]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            return View();
        }
        
        [Route("logout")]
        public async Task<IActionResult> Logout(string type)
        {
            await HttpContext.SignOutAsync();
            if (type == "admin")
            {
                return Redirect("/admin");
            }
            else if (type == "pharmacy")
            {
                return Redirect("/pharmacy");
            }
            else
            {
                return Redirect("/user");
            }
        }

    }
}
