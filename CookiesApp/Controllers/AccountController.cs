using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookiesApp.Models;
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
            //Get the inserted Credentials

            LoginModel LogModel = new LoginModel();
            if (model.Email == LogModel.UserName)
            {
                //Good entry, take the next step
                //Lets carry out some info
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, model.Email));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };
                await HttpContext.SignInAsync(_externalCookieScheme, new ClaimsPrincipal(identity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Wrong Credentials");
                return View();
            }

        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_externalCookieScheme);
            string info = string.Format("Completed by {0} on {1}/ {2}", User.Identity.Name, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            return RedirectToAction("Login");
        }
    }
}