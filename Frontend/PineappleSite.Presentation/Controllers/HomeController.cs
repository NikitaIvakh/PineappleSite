using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Extecsions;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Favorites;
using PineappleSite.Presentation.Services.Products;
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
        public async Task<IActionResult> GetProducts(string searchProduct, string currentFilter, int? pageNumber)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                if (products.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(searchProduct))
                    {
                        var filteredProductList = products.Data.Where(
                            key => key.Name.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.ProductCategory.GetDisplayName().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Price.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Count.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase)).ToList();

                        products = new ProductsCollectionResultViewModel<ProductViewModel>
                        {
                            Data = filteredProductList,
                        };
                    }

                    ViewData["SearchProduct"] = searchProduct;
                    ViewData["CurrentFilter"] = currentFilter;

                    var pageIndex = 9;
                    var filteredProducts = products.Data.AsQueryable();
                    var paginatedProducts = PaginatedList<ProductViewModel>.Create(filteredProducts, pageNumber ?? 1, pageIndex);

                    return View(paginatedProducts);
                }

                else
                {
                    TempData["error"] = products.ErrorMessage;
                    return RedirectToAction(nameof(GetProducts));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);

                if (product.IsSuccess)
                {
                    ProductViewModel productViewModel = new()
                    {
                        Id = product.Data.Id,
                        Name = product.Data.Name,
                        Description = product.Data.Description,
                        ProductCategory = product.Data.ProductCategory,
                        Price = product.Data.Price,
                        Count = product.Data.Count,
                        ImageUrl = product.Data.ImageUrl,
                        ImageLocalPath = product.Data.ImageLocalPath,
                    };

                    string? userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
                    FavouriteResult<FavouriteViewModel> result = await _favoriteService.GetFavouruteProductsAsync(userId);

                    if (result is null || result.Data.FavouriteDetails.Count == 0 || result.Data.FavouriteDetails.FirstOrDefault().ProductId != product.Data.Id)
                    {
                        var favouriteProduct1 = new ProductFavouriteViewModel
                        {
                            Product = productViewModel,
                            Favourite = new FavouriteViewModel(),
                        };

                        return View(favouriteProduct1);
                    }

                    else
                    {
                        FavouriteViewModel favouriteViewModel = new()
                        {
                            FavouriteHeader = new FavouriteHeaderViewModel
                            {
                                FavouriteHeaderId = result.Data.FavouriteHeader.FavouriteHeaderId,
                                UserId = userId,
                            },

                            FavouriteDetails = result.Data.FavouriteDetails,
                        };

                        var favouriteProduct = new ProductFavouriteViewModel
                        {
                            Product = productViewModel,
                            Favourite = favouriteViewModel,
                        };

                        return View(favouriteProduct);
                    }
                }

                else
                {
                    TempData["error"] = product.ErrorMessage;
                    return RedirectToAction(nameof(GetProducts));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        [HttpPost]
        [ActionName("AddToCart")]
        public async Task<IActionResult> AddToCart(ProductFavouriteViewModel productViewModel)
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
                Count = productViewModel.Product.Count,
                ProductId = productViewModel.Product.Id,
            };

            List<CartDetailsViewModel> cartViewModels = [cartDetailsViewModel];
            cartViewModel.CartDetails = cartViewModels;

            CartResult<CartViewModel> response = await _shoppingCartService.CartUpsertAsync(cartViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = response.ErrorMessage;
            }

            return View(cartViewModel);
        }

        [HttpPost]
        [ActionName("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites(ProductFavouriteViewModel productViewModel)
        {
            FavouriteViewModel favouritesViewModel = new()
            {
                FavouriteHeader = new FavouriteHeaderViewModel
                {
                    UserId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value,
                },
            };

            FavouriteDetailsViewModel favoriteDetailsViewModel = new()
            {
                ProductId = productViewModel.Product.Id,
            };

            List<FavouriteDetailsViewModel> favoriteDetailsViewModels = [favoriteDetailsViewModel];
            favouritesViewModel.FavouriteDetails = favoriteDetailsViewModels;

            FavouriteResult<FavouriteViewModel> response = await _favoriteService.FavouruteUpsertProductsAsync(favouritesViewModel);

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

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}