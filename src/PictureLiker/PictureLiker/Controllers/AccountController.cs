using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Saml;
using PictureLiker.Authentication;
using PictureLiker.DAL;
using PictureLiker.Extensions;
using PictureLiker.Factories;
using PictureLiker.Models;
using PictureLiker.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PictureLiker.Controllers
{   //TODO: Unit Tests
    public class AccountController : Controller
    {
        private readonly IDomainQuery _domainQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityFactory _entityFactory;

        public AccountController(IUnitOfWork unitOfWork, IEntityFactory entityFactory, IDomainQuery domainQuery)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(nameof(Login), model);

            var user = await _unitOfWork.UseRepository.FirstOrDefaultAsync(u => u.Email.EqualsIgnoreCase(model.Email));

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "This Email has not been registered yet.");

                return View(nameof(Login), model);
            }

            await HttpContext.SignInAsync(Startup.DefaultAuthenticationScheme,
                new ClaimsPrincipal(CreateClaimsIdentity(user)));

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegistrationModel model, string returnUrl = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(nameof(Register), model);
            // TODO: duplicated with domain error, implement better error handling 
            if (await IsEmailInUse(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Specified email is already in use.");

                return View(nameof(Register), model);
            }

            var user = _entityFactory.GetUser()
                .SetName(model.PreferredName)
                .SetRole(RoleTypes.GeneralUser);

            await user.SetEmail(model.Email);

            await _unitOfWork.UseRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(Startup.DefaultAuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> IsEmailInUse(string email)
        {
            return (await _unitOfWork.UseRepository.ListAsync(u => u.Email.ToLower().Equals(email.ToLower()))).Any();
        }

        private static ClaimsIdentity CreateClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.AuthenticationMethod, SamlConstants.AuthenticationMethods.PasswordString)
            };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
