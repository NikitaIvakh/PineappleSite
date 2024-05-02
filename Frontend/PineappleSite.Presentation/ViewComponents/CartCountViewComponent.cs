using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;

namespace PineappleSite.Presentation.ViewComponents;

public sealed class CartCountViewComponent(
    IShoppingCartService shoppingCartService,
    IHttpContextAccessor contextAccessor)
    : ViewComponent
{
    public async Task<string> InvokeAsync()
    {
        var userId = contextAccessor.HttpContext!.User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)
            ?.FirstOrDefault()?.Value;

        var result = await shoppingCartService.GetCartAsync(userId!);
        var cartItemsCount = result.Data!.CartDetails.Count;
        return cartItemsCount.ToString();
    }
}