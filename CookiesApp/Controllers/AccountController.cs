using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookiesApp.Tools;
using CookiesApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookiesApp.Controllers
{
    public class AccountController : Controller
    {
        string _externalCookieScheme = "CookiesInstance";
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            LdapManager ldapManager = new LdapManager();
            var ldabUser = ldapManager.GetUserIdentity(model.Email.Split('@').First());
            if (ldabUser == null)
            {
                ModelState.AddModelError(string.Empty, "عفوا. حدث خطأ أثناء التسجيل");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                int employeeID = int.Parse(ldabUser.EmployeeId);
                if (employeeID == 0)
                {
                    ModelState.AddModelError(string.Empty, "عفوا. ليس لديك صلاحية الوصول");
                    return View(model);
                }
                else
                {
                    List<Claim> Claims = new List<Claim>();
                    //Claims.Add(new Claim(ClaimTypes.Name, ldabUser.Name));
                    Claims.Add(new Claim(ClaimTypes.Email, ldabUser.EmailAddress));
                    Claims.Add(new Claim(ClaimTypes.GivenName, ldabUser.Description));
                    ClaimsIdentity identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };
                    await HttpContext.SignInAsync(_externalCookieScheme, new ClaimsPrincipal(identity), authProperties);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}