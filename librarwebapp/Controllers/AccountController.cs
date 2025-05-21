using librarwebapp.Interfaces.Services;
using librarwebapp.Models;
using librarwebapp.Models.ExternalServices.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace librarwebapp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAppAuthenticationService _appAuthenticationService;

        public AccountController(IAppAuthenticationService appAuthenticationService)
        {
            _appAuthenticationService = appAuthenticationService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _appAuthenticationService.Authenticate(request);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new CreateUserRequest
            {
                Email = model.Email,
                Name = model.Name,
                LastName = model.LastName,
                Password = model.Password
            };

            var result = await _appAuthenticationService.Create(request);
            if(result.UserId != Guid.Empty)
            {
                var loginModel = new LoginDto
                {
                    Username = request.Email,
                    Password = request.Password
                };
                await _appAuthenticationService.Authenticate(loginModel);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LoginTwoFactor(TwoFactorLoginDto request)
        {
            bool isVerified = _appAuthenticationService.VerifyTwoFactorAuthentication(request);
            if (isVerified)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };
                ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Código de validación incorrecto");
            return View(request);
        }

        [HttpGet, Authorize]
        public IActionResult EnableAuthenticator()
        {
            var model = new EnableAuthenticatorDto();

            model.Username = User.Identity.Name;

            var info = _appAuthenticationService.GenerateTwoFactorInfo(model.Username);

            model.SecretKey = info.SecretKey;

            model.AuthenticatorUri = info.AuthenticatorUri;

            return View(model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorDto request)
        {
            var res = await _appAuthenticationService.EnableAuthenticator(request);

            if (res)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Código de validación incorrecto");
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
