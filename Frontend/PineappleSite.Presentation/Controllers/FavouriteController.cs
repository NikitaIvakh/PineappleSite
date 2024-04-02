using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Services.Favorites;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers
{
    public class FavouriteController(IFavoriteService favoriteService) : Controller
    {
        private readonly IFavoriteService _favoriteService = favoriteService;

        // GET: FavouriteController
        public async Task<ActionResult> Index()
        {
            try
            {
                string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
                FavouriteResult<FavouriteViewModel> result = await _favoriteService.GetFavouruteProductsAsync(userId);

                if (result.IsSuccess)
                {
                    FavouriteResult<FavouriteViewModel> favouriteViewModel = new()
                    {
                        Data = result.Data,
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        SuccessMessage = result.SuccessMessage,
                        ValidationErrors = result.ValidationErrors,
                    };

                    return View(favouriteViewModel);
                }

                else
                {
                    foreach (var error in result.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // GET: FavouriteController/Delete/5
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            try
            {
                string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
                FavouriteResult<FavouriteViewModel> response = await _favoriteService.FavouruteRemoveProductsAsync(productId);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}