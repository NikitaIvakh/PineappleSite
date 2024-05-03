using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;

namespace PineappleSite.Presentation.Controllers;

public sealed class AuthenticateController(IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    [HttpGet]
    public ActionResult Login(string? returnUrl = null)
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(AuthRequestViewModel authRequestViewModel)
    {
        try
        {
            authRequestViewModel.ReturnUrl ??= Url.Content("/");
            var response = await identityService.LoginAsync(authRequestViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return LocalRedirect(authRequestViewModel.ReturnUrl);
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Login));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public Task<ActionResult> Register()
    {
        return Task.FromResult<ActionResult>(View());
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequestViewModel registerRequestViewModel)
    {
        try
        {
            var returnUrl = Url.Content("/");
            var response = await identityService.RegisterAsync(registerRequestViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return LocalRedirect(returnUrl);
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Register));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Logout(string? returnUrl)
    {
        returnUrl ??= Url.Content("/");
        var result =
            httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.IsCompletedSuccessfully)
        {
            return await Task.FromResult<ActionResult>(LocalRedirect(returnUrl));
        }

        TempData["success"] = "Вы успешно вышли из аккаунта";

        httpContextAccessor.HttpContext!.Response.Cookies.Delete("JWTToken");
        httpContextAccessor.HttpContext!.Response.Cookies.Delete("AuthenticateCookie");

        return await Task.FromResult<ActionResult>(LocalRedirect(returnUrl));
    }
}