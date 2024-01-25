using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;
using System.Diagnostics;

namespace PineappleSite.Presentation.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService shoppingCartService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IProductService _productService = productService;
        private readonly IShoppingCartService _shoppingCartService = shoppingCartService;

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