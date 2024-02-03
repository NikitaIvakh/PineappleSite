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
            try
            {
                var products = await _productService.GetAllProductsAsync();

                if (products.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(searchProduct))
                    {
                        var filtetedProductsList = products.Data.Where(
                            key => key.Name.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.ProductCategory.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                            key.Price.ToString().Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();

                        products = new ProductsCollectionResultViewModel<ProductViewModel>
                        {
                            Data = filtetedProductsList,
                        };
                    }

                    ViewData["SearchProduct"] = searchProduct;
                    ViewData["CurrentFilter"] = currentFilter;

                    int pageSize = 10;
                    var filteredProducts = products.Data.AsQueryable();
                    var paginatedProducts = PaginatedList<ProductViewModel>.Create(filteredProducts, pageNumber ?? 1, pageSize);

                    return View(paginatedProducts);
                }

                else
                {
                    TempData["error"] = products.ErrorMessage;
                    return RedirectToAction("Index", "Home");
                }

            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
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

                    return View(productViewModel);
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
                ProductResultViewModel response = await _productService.CreateProductAsync(createProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
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
            try
            {
                var product = await _productService.GetProductAsync(id);

                if (product.IsSuccess)
                {
                    UpdateProductViewModel updateUserViewModel = new()
                    {
                        Id = product.Data.Id,
                        Name = product.Data.Name,
                        Description = product.Data.Description,
                        ProductCategory = product.Data.ProductCategory,
                        Price = product.Data.Price,
                    };

                    return View(updateUserViewModel);
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

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateProductViewModel updateProductViewModel)
        {
            try
            {
                ProductResultViewModel response = await _productService.UpdateProductAsync(updateProductViewModel.Id, updateProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
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
                ProductResultViewModel response = await _productService.DeleteProductAsync(deleteProductViewModel.Id, deleteProductViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
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
            var response = await _productService.DeleteProductsAsync(deleteProducts);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction(nameof(GetProducts));
            }
        }
    }
}