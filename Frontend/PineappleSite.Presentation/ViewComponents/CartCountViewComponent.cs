using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.ViewComponents;

public sealed class CartCountViewComponent(
    IShoppingCartService shoppingCartService,
    IShoppingCartClient shoppingCartClient,
    IHttpContextAccessor contextAccessor)
    : ViewComponent
{
    public async Task<IViewComponentResult?> InvokeAsync()
    {
        AddBearerToken();
        try
        {
            var userId = contextAccessor.HttpContext!.User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)
                ?.FirstOrDefault()?.Value;

            var result = await shoppingCartService.GetCartAsync(userId!);
            var cartItemsCount = result.Data!.CartDetails.Count;
            return Content(cartItemsCount.ToString());
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Content("0");
        }
    }

    private void AddBearerToken()
    {
        if (contextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            shoppingCartClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", contextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}