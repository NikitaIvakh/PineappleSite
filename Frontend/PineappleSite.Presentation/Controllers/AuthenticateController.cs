using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Controllers
{
    public class AuthenticateController(IIdentityService identityService) : Controller
    {
        private readonly IIdentityService _identityService = identityService;

        [HttpGet]
        public ActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthRequestViewModel authRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                authRequestViewModel.ReturnUrl ??= Url.Content("/");
                IdentityResult<AuthResponseViewModel> response = await _identityService.LoginAsync(authRequestViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return LocalRedirect(authRequestViewModel.ReturnUrl);
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Login));
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
                IdentityResult<RegisterResponseViewModel> response = await _identityService.RegisterAsync(registerRequestViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return LocalRedirect(returnUrl);
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Register));
                }
            }

            ModelState.AddModelError(string.Empty, "Ошибка регистрации, попробуйте еще раз!");
            return View(registerRequestViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var returnUrl = Url.Content("/");
            var response = await _identityService.LogoutAsync();

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return LocalRedirect(returnUrl);
            }

            else
            {
                TempData["error"] = response.ErrorMessage;
                return LocalRedirect(returnUrl);
            }
        }
    }
}