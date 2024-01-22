using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using System.Diagnostics;

namespace PineappleSite.Presentation.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProductService productService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
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
