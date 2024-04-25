using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PineappleSite.Presentation.Middleware;

public sealed class TokenExpirationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.User.Identity!.IsAuthenticated && !context.Request.Cookies.ContainsKey("JWTToken") &&
            context.Request.Path.Value is not "/Authenticate/Login" &&
            context.Request.Path.Value is not "/Authenticate/Register" && context.Request.Path.Value is not "/")
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/");
            return;
        }

        if (!context.Request.Cookies.ContainsKey("JWTToken") &&
            context.Request.Path.Value is not "/Authenticate/Login" &&
            context.Request.Path.Value is not "/Authenticate/Register" && context.Request.Path.Value is not "/")
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/");
            return;
        }

        await next(context);
    }
}