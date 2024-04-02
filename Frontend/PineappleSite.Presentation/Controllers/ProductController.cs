using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Controllers
{
    public class ProductController(IProductService productService, IShoppingCartService shoppingCartService, IFavoriteService favoriteService) : Controller
    {
        private readonly IProductService _productService = productService;
        private readonly IShoppingCartService _shoppingCartService = shoppingCartService;
        private readonly IFavoriteService _favoriteService = favoriteService;

        // GET: ProductController
        public Task<ActionResult> Index()
        {
            return Task.FromResult<ActionResult>(RedirectToAction("Index", "Home"));
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

                    if (paginatedProducts.Count == 0)
                    {
                        return View();
                    }

                    return View(paginatedProducts);
                }

                else
                {
                    foreach (var error in products.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

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
                    foreach (var error in product.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

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
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

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
                    foreach (var error in product.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

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
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

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
                await _shoppingCartService.RemoveCartDetailsAsync(deleteProductViewModel.Id);
                await _favoriteService.FavouruteRemoveProductsAsync(deleteProductViewModel.Id);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(GetProducts));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(GetProducts));
                }
            }

            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> DeleteProductList(List<int> selectedProducts)
        {
            if (selectedProducts.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один продукт для удаления.";
                return RedirectToAction(nameof(GetProducts));
            }

            var deleteProducts = new DeleteProductsViewModel { ProductIds = selectedProducts };
            var response = await _productService.DeleteProductsAsync(deleteProducts);

            await _shoppingCartService.RemoveCartDetailsListAsync(deleteProducts);
            await _favoriteService.FavouruteRemoveProductsListAsync(deleteProducts);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            else
            {
                foreach (var error in response.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction(nameof(GetProducts));
            }
        }
    }
}