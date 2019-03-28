using System;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PictureLiker.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(model);
            
            //todo: save user info to db

            return View(model);
        }
    }
}
