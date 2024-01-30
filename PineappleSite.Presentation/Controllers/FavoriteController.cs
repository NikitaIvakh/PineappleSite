using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Controllers
{
    public class FavoriteController(IFavoriteService favoriteService) : Controller
    {
        private readonly IFavoriteService _favoriteService = favoriteService;

        // GET: FavotiteController
        public async Task<ActionResult> Index()
        {
            return View(await GetFavotiteItemsAfterAuthenticate());
        }

        public async Task<ActionResult> RemoveDetails(int detailsId)
        {
            try
            {
                FavouriteResultViewModel response = await _favoriteService.DeleteFavoriteDetails(detailsId);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }

            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View();
        }

        private async Task<FavouriteResultViewModel<FavouritesViewModel>> GetFavotiteItemsAfterAuthenticate()
        {
            string userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
            FavouriteResultViewModel<FavouritesViewModel> response = await _favoriteService.GetFavoritesAsync(userId);

            if (response.IsSuccess)
            {
                FavouriteResultViewModel<FavouritesViewModel> favouritesViewModel = new()
                { 
                    Data = response.Data,
                    ErrorMessage = response.ErrorMessage,
                    ErrorCode = response.ErrorCode,
                    SuccessMessage = response.SuccessMessage,
                    ValidationErrors = response.ValidationErrors,
                };

                return favouritesViewModel;
            }

            return response;
        }
    }
}