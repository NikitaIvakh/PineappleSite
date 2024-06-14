using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Controllers;

public sealed class ProductController(
    IProductService productService,
    IShoppingCartService shoppingCartService,
    IFavouriteService favouriteService) : Controller
{
    // GET: ProductController
    public Task<ActionResult> Index()
    {
        return Task.FromResult<ActionResult>(RedirectToAction("Index", "Home"));
    }

    public async Task<ActionResult> GetProducts(string searchProduct, string currentFilter, int? pageNumber)
    {
        try
        {
            var products = await productService.GetAllProductsAsync();

            if (products.IsSuccess)
            {
                if (!string.IsNullOrEmpty(searchProduct))
                {
                    var filteredProductsList = products.Data!.Where(
                            key => key.Name.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Description.Contains(searchProduct, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.ProductCategory.ToString().Contains(searchProduct,
                                       StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Price.ToString(CultureInfo.InvariantCulture).Contains(searchProduct,
                                       StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    products = new ProductsCollectionResultViewModel<ProductViewModel>
                    {
                        Data = filteredProductsList,
                    };
                }

                ViewData["SearchProduct"] = searchProduct;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageSize = 10;
                var filteredProducts = products.Data!.AsQueryable();
                var paginatedProducts =
                    PaginatedList<ProductViewModel>.Create(filteredProducts, pageNumber ?? 1, pageSize);

                if (paginatedProducts.Count != 0)
                {
                    return View(paginatedProducts);
                }

                TempData["error"] = "Нет результатов";
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = products.ValidationErrors;
            return RedirectToAction(nameof(Create));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: ProductController/Details/5
    public async Task<ActionResult> Details(int productId, int? pageNumber, string? searchProduct)
    {
        try
        {
            var product = await productService.GetProductAsync(productId);

            if (product.IsSuccess)
            {
                var productViewModel = new ProductViewModel()
                {
                    Id = product.Data!.Id,
                    Name = product.Data.Name,
                    Description = product.Data.Description,
                    ProductCategory = product.Data.ProductCategory,
                    Price = product.Data.Price,
                    ImageUrl = product.Data.ImageUrl,
                };

                return View(productViewModel);
            }

            TempData["error"] = product.ValidationErrors;
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }
    }

    // GET: ProductController/Create
    public Task<ActionResult> Create()
    {
        return Task.FromResult<ActionResult>(View());
    }

    // POST: ProductController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateProductViewModel createProductViewModel, int? pageNumber,
        string? searchProduct)
    {
        try
        {
            ProductResultViewModel response = await productService.CreateProductAsync(createProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts),
                    new { pageNumber = pageNumber, searchProduct = searchProduct });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Create), new { pageNumber = pageNumber, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            TempData["error"] = "Такой продукт уже существует";
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Create), new { pageNumber = pageNumber, searchProduct = searchProduct });
        }
    }

    // GET: ProductController/Edit/5
    public async Task<ActionResult> Edit(int productId, int? pageNumber, string? searchProduct)
    {
        try
        {
            var product = await productService.GetProductAsync(productId);

            if (product.IsSuccess)
            {
                UpdateProductViewModel updateUserViewModel = new()
                {
                    Id = product.Data!.Id,
                    Name = product.Data.Name,
                    Description = product.Data.Description,
                    ProductCategory = product.Data.ProductCategory,
                    Price = (decimal)product.Data.Price,
                    ImageUrl = product.Data.ImageUrl,
                };

                return View(updateUserViewModel);
            }

            TempData["error"] = product.ValidationErrors;
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            TempData["error"] = "Такой продукт уже существует";
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }
    }

    // POST: ProductController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UpdateProductViewModel updateProductViewModel, int? pageNumber,
        string? searchProduct)
    {
        try
        {
            var response = await productService.UpdateProductAsync(updateProductViewModel.Id, updateProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts),
                    new { pageNumber = pageNumber, searchProduct = searchProduct });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Edit),
                new { pageNumber = pageNumber, productId = updateProductViewModel.Id, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            TempData["error"] = "Такой продукт уже существует";
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Edit),
                new { pageNumber = pageNumber, productId = updateProductViewModel.Id, searchProduct = searchProduct });
        }
    }

    // POST: ProductController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int productId, int? pageNumber, string? searchProduct)
    {
        try
        {
            var deleteProductViewModel = new DeleteProductViewModel(productId);
            await favouriteService.DeleteFavouriteProductAsync(deleteProductViewModel);
            await shoppingCartService.RemoveCartDetailsAsync(deleteProductViewModel);

            var response = await productService.DeleteProductAsync(deleteProductViewModel.Id, deleteProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts),
                    new { pageNumber = pageNumber, searchProduct = searchProduct });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }
    }

    public async Task<ActionResult> DeleteProductList(List<int> selectedProducts, int? pageNumber,
        string? searchProduct)
    {
        try
        {
            if (selectedProducts.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один продукт для удаления.";
                return RedirectToAction(nameof(GetProducts));
            }

            var deleteProducts = new DeleteProductsViewModel(selectedProducts);
            await favouriteService.DeleteFavouriteProductsAsync(deleteProducts);
            await shoppingCartService.RemoveCartDetailsAsync(deleteProducts);

            var response = await productService.DeleteProductsAsync(deleteProducts);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts),
                    new { pageNumber = pageNumber, searchProduct = searchProduct });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(GetProducts),
                new { pageNumber = pageNumber, searchProduct = searchProduct });
        }
    }
}