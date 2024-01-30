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
        [ActionName("AddToCart")]
        public async Task<IActionResult> AddToCart(ProductViewModel productViewModel)
        {
            CartViewModel cartViewModel = new()
            {
                CartHeader = new CartHeaderViewModel
                {
                    UserId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value,
                },
            };

            CartDetailsViewModel cartDetailsViewModel = new()
            {
                Count = productViewModel.Count,
                ProductId = productViewModel.Id,
            };

            List<CartDetailsViewModel> cartViewModels = [cartDetailsViewModel];
            cartViewModel.CartDetails = cartViewModels;

            ShoppingCartResponseViewModel response = await _shoppingCartService.CartUpsertAsync(cartViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = response.ValidationErrors;
            }

            return View(cartViewModel);
        }

        [HttpPost]
        [ActionName("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites(ProductViewModel productViewModel)
        {
            FavouritesViewModel favouritesViewModel = new()
            {
                FavoutiteHeader = new FavouritesHeaderViewModel
                {
                    UserId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value,
                },
            };

            FavoriteDetailsViewModel favoriteDetailsViewModel = new()
            {
                ProductId = productViewModel.Id,
            };

            List<FavoriteDetailsViewModel> favoriteDetailsViewModels = [favoriteDetailsViewModel];
            favouritesViewModel.FavouritesDetails = favoriteDetailsViewModels;

            FavouriteResultViewModel<FavouritesHeaderViewModel> response = await _favoriteService.FavoritesUpsertAsync(favouritesViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = response.ErrorMessage;

            }

            return View(favouritesViewModel);
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