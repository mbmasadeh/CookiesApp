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
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            //Get the inserted Credentials

            LoginModel LogModel = new LoginModel();
            if (model.Email == LogModel.UserName)
            {
                //Good entry, take the next step
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
        public IActionResult Logout()
        {
            return View();
        }
    }
}