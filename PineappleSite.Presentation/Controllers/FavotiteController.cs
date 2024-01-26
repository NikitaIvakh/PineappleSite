using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Controllers
{
    public class FavotiteController(IFavoriteService favoriteService) : Controller
    {
        private readonly IFavoriteService _favoriteService = favoriteService;

        // GET: FavotiteController
        public async Task<ActionResult> Index()
        {
            return View(await GetFavotiteItemsAfterAuthenticate());
        }

        // GET: FavotiteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FavotiteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<FavouritesViewModel> GetFavotiteItemsAfterAuthenticate()
        {
            var userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
            FavoritesResponseViewModel response = await _favoriteService.GetFavoritesAsync(userId);

            if (response.IsSuccess)
            {
                FavouritesViewModel favouritesViewModel = JsonConvert.DeserializeObject<FavouritesViewModel>(Convert.ToString(response.Data));
                return favouritesViewModel;
            }

            return new FavouritesViewModel();
        }
    }
}