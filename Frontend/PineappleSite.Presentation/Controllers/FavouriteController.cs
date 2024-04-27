using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using System.Security.Claims;
using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Controllers;

public sealed class FavouriteController(IFavouriteService favouriteService) : Controller
{
    // GET: FavouriteController
    public async Task<ActionResult> Index()
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
            var result = await favouriteService.GetFavouriteProductsAsync(userId!);

            if (result.IsSuccess)
            {
                TempData["success"] = result.SuccessMessage;
                return View(result);
            }

            TempData["error"] = result.ValidationErrors;
            return RedirectToAction("Index", "Home");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: FavouriteController/Delete/5
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        try
        {
            var deleteProductViewModel = new DeleteProductViewModel(productId);
            var response = await favouriteService.DeleteFavouriteProductAsync(deleteProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}