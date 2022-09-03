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
        public IActionResult Login(string returnUrl = null)
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

        private async Task<bool> ValidateLogin(string userName, string password, string who)
        {
            if (who == "admin")
            {
                if (await _adminservice.Login(userName, password)) {
                    return true;
                }else
                {
                    return false;
                }
            }
            else if (who == "pharmacy")
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string userName = "", string password = "", string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            var whoisLog = string.IsNullOrEmpty(returnUrl) ? null : returnUrl.Substring(1);
            // Normally Identity handles sign in, but you can do it directly
            whoisLog = string.IsNullOrEmpty(whoisLog) ? "admin" : whoisLog;
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
                    return Redirect("/");
                }
            }else
            {
                ViewBag.errorMessage = "Invalid username or password.";
                ViewBag.username = userName;
                return View();

            }
        }
        
        [Route("denied")]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }
        
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/admin");
        }

    }
}
