using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Favorites;
using PineappleSite.Presentation.Services.ShoppingCarts;
using System.Diagnostics;

namespace PineappleSite.Presentation.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService shoppingCartService, IFavoriteService favoriteService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IProductService _productService = productService;
        private readonly IShoppingCartService _shoppingCartService = shoppingCartService;
        private readonly IFavoriteService _favoriteService = favoriteService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            var product = await _productService.GetProductAsync(id);
            return View(product);
        }

        [HttpPost]
        [ActionName("GetProductDetails")]
        public async Task<IActionResult> GetProductDetails(ProductViewModel productViewModel)
        {
            CartViewModel cartViewModel = new()
            {
                CartHeader = new CartHeaderViewModel { UserId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value, },
            };

            CartDetailsViewModel cartDetailsViewModel = new()
            {
                Count = productViewModel.Count,
                ProductId = productViewModel.Id,
            };

            List<CartDetailsViewModel> cartViewModels = [cartDetailsViewModel];
            cartViewModel.CartDetails = cartViewModels;

            FavouritesViewModel favouritesViewModel = new()
            {
                FavoutiteHeader = new FavouritesHeaderViewModel { UserId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value, },
                FavouritesDetails = new List<FavoriteDetailsViewModel> { new() { ProductId = productViewModel.Id } }
            };

            ShoppingCartResponseViewModel cartResponse = await _shoppingCartService.CartUpsertAsync(cartViewModel);
            FavouriteResultViewModel<FavouritesViewModel> favoriteResponse = await _favoriteService.FavoritesUpsertAsync(favouritesViewModel);

            if (favoriteResponse.IsSuccess)
            {
                TempData["success"] = favoriteResponse.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = favoriteResponse.ErrorMessage;
                return RedirectToAction(nameof(GetProducts));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}