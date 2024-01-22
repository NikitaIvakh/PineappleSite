using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> GetProducts(string searchProduct, string currentFilter, int? pageNumber)
        {
            var products = await _productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(searchProduct))
            {
                products = products.Where(
                    key => key.Name.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                    key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                    key.ProductCategory.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                    key.Price.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            ViewData["SearchProduct"] = searchProduct;
            ViewData["CurrentFilter"] = currentFilter;

            int pageSize = 10;
            var filteredProducts = products.AsQueryable();
            var paginatedProducts = PaginatedList<ProductViewModel>.Create(filteredProducts, pageNumber ?? 1, pageSize);

            return View(paginatedProducts);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.GetProductAsync(id);
            return View(product);
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductViewModel createProductViewModel)
        {
            try
            {
                ProductAPIViewModel response = await _productService.CreateProductAsync(createProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Create));
                }
            }

            catch
            {
                return View(createProductViewModel);
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = await _productService.GetProductAsync(id);
            var updateUserViewModel = new UpdateProductViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Description = user.Description,
                ProductCategory = user.ProductCategory,
                Price = user.Price,
            };

            return View(updateUserViewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateProductViewModel updateProductViewModel)
        {
            try
            {
                ProductAPIViewModel response = await _productService.UpdateProductAsync(updateProductViewModel.Id, updateProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Edit));
                }
            }

            catch
            {
                return View(updateProductViewModel);
            }
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteProductViewModel deleteProductViewModel)
        {
            try
            {
                ProductAPIViewModel response = await _productService.DeleteProductAsync(deleteProductViewModel.Id, deleteProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(GetProducts));
                }
            }

            catch
            {
                return View(deleteProductViewModel);
            }
        }

        public async Task<ActionResult> DeleteProductList(List<int> selectedProducts)
        {
            if (selectedProducts is null || selectedProducts.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один продукт для удаления.";
                return RedirectToAction(nameof(GetProducts));
            }

            var deleteProducts = new DeleteProductsViewModel { ProductIds = selectedProducts };
            ProductAPIViewModel response = await _productService.DeleteProductsAsync(deleteProducts);

            if (response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = response.ValidationErrors;
                return RedirectToAction(nameof(GetProducts));
            }
        }
    }
}