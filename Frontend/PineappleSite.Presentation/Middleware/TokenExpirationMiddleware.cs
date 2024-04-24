using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PineappleSite.Presentation.Middleware;

public sealed class TokenExpirationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.User.Identity!.IsAuthenticated && !context.Request.Cookies.ContainsKey("JWTToken") &&
            context.Request.Path.Value is not "/Authenticate/Login")
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/Authenticate/Login");
            return;
        }

        if (!context.Request.Cookies.ContainsKey("JWTToken") && context.Request.Path.Value is not "/Authenticate/Login")
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/Authenticate/Login");
            return;
        }

        await next(context);
    }
}