using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;

namespace PineappleSite.Presentation.Controllers
{
    public class AuthenticateController(IIdentityService identityService) : Controller
    {
        private readonly IIdentityService _identityService = identityService;

        [HttpGet]
        public async Task<ActionResult> Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthRequestViewModel authRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                authRequestViewModel.ReturnUrl ??= Url.Content("/");
                var isLoggedIn = await _identityService.LoginAsync(authRequestViewModel);

                if (isLoggedIn)
                {
                    TempData["success"] = "Успешный вход в аккаукнт";
                    return LocalRedirect(authRequestViewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Ошибка входа, попробуйте еще раз!");
            return View(authRequestViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestViewModel registerRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = Url.Content("/");
                var IsRegister = await _identityService.RegisterAsync(registerRequestViewModel);

                if (IsRegister)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Ошибка регистрации, попробуйте еще раз!");
            return View(registerRequestViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            returnUrl ??= Url.Content("/");
            await _identityService.LogoutAsync();
            return LocalRedirect(returnUrl);
        }
    }
}