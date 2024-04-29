using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Extecsions;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Products;
using PineappleSite.Presentation.Services.ShoppingCarts;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using PineappleSite.Presentation.Services.Favourite;

namespace PineappleSite.Presentation.Controllers;

public sealed class HomeController(
    IProductService productService,
    IShoppingCartService shoppingCartService,
    IFavouriteService favouriteService) : Controller
{
    [HttpGet]
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string searchProduct, string currentFilter, int? pageNumber)
    {
        try
        {
            var products = await productService.GetAllProductsAsync();

            if (products.IsSuccess)
            {
                if (!string.IsNullOrEmpty(searchProduct))
                {
                    var filteredProductList = products.Data!.Where(
                            key => key.Name.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.ProductCategory.GetDisplayName().Contains(searchProduct,
                                       StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Price.ToString(CultureInfo.InvariantCulture)
                                       .Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Count.ToString().Contains(searchProduct,
                                       StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    products = new ProductsCollectionResultViewModel<ProductViewModel>
                    {
                        Data = filteredProductList,
                    };
                }

                ViewData["SearchProduct"] = searchProduct;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageIndex = 9;
                var filteredProducts = products.Data!.AsQueryable();
                var paginatedProducts =
                    PaginatedList<ProductViewModel>.Create(filteredProducts, pageNumber ?? 1, pageIndex);

                return View(paginatedProducts);
            }

            TempData["error"] = products.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProductDetails(int id)
    {
        try
        {
            var product = await productService.GetProductAsync(id);

            if (product.IsSuccess)
            {
                var productViewModel = new ProductViewModel
                {
                    Id = product.Data!.Id,
                    Name = product.Data.Name,
                    Description = product.Data.Description,
                    ProductCategory = product.Data.ProductCategory,
                    Price = product.Data.Price,
                    Count = product.Data.Count,
                    ImageUrl = product.Data.ImageUrl,
                    ImageLocalPath = product.Data.ImageLocalPath
                };

                var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()
                    ?.Value;

                var result = await favouriteService.GetFavouriteProductsAsync(userId);
                var allFavouriteProducts =
                    result.Data!.FavouriteDetails.FindAll(key => key.ProductId == productViewModel.Id);

                if (result.Data.FavouriteDetails.Count == 0 || allFavouriteProducts.Count == 0)
                {
                    var favouriteProduct1 = new ProductFavouriteViewModel
                    {
                        Product = productViewModel,
                        Favourite = new FavouriteViewModel(null, null),
                    };

                    return View(favouriteProduct1);
                }

                var favouriteHeader = new FavouriteHeaderViewModel
                {
                    FavouriteHeaderId = result.Data.FavouriteHeader.FavouriteHeaderId,
                    UserId = userId,
                };

                var favouriteDetails = result.Data.FavouriteDetails;

                FavouriteViewModel favouriteViewModel = new(favouriteHeader, favouriteDetails);

                var favouriteProduct = new ProductFavouriteViewModel
                {
                    Product = productViewModel,
                    Favourite = favouriteViewModel,
                };

                return View(favouriteProduct);
            }

            TempData["error"] = product.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ActionName("AddToCart")]
    public async Task<IActionResult> AddToCart(ProductFavouriteViewModel productViewModel)
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
            var cartHeader = new CartHeaderViewModel() { UserId = userId };
            CartDetailsViewModel cartDetailsViewModel = new()
            {
                Count = productViewModel.Product.Count,
                ProductId = productViewModel.Product.Id,
            };

            List<CartDetailsViewModel> cartViewModels = [cartDetailsViewModel];
            
            var cartViewModel = new CartViewModel() { CartHeader = cartHeader, CartDetails = cartViewModels };
            var response = await shoppingCartService.CartUpsertAsync(cartViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ActionName("AddToFavorites")]
    public async Task<IActionResult> AddToFavorites(ProductFavouriteViewModel productViewModel)
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
            var favouriteHeader = new FavouriteHeaderViewModel { UserId = userId, };
            var favoriteDetailsViewModel = new FavouriteDetailsViewModel { ProductId = productViewModel.Product.Id };
            List<FavouriteDetailsViewModel> favoriteDetailsViewModels = [favoriteDetailsViewModel];

            FavouriteViewModel favouritesViewModel = new(favouriteHeader, favoriteDetailsViewModels);

            var response = await favouriteService.FavouriteProductUpsertAsync(favouritesViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
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

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult ServicesProvided()
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