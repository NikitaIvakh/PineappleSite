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

                    products = new ProductsCollectionResultViewModel<GetProductsViewModel>
                    {
                        Data = filteredProductsList,
                    };
                }

                ViewData["SearchProduct"] = searchProduct;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageSize = 10;
                var filteredProducts = products.Data!.AsQueryable();
                var paginatedProducts =
                    PaginatedList<GetProductsViewModel>.Create(filteredProducts, pageNumber ?? 1, pageSize);

                return paginatedProducts.Count == 0 ? View() : View(paginatedProducts);
            }

            TempData["error"] = products.ValidationErrors;
            return RedirectToAction("Index", "Home");
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: ProductController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        try
        {
            var product = await productService.GetProductAsync(id);

            if (product.IsSuccess)
            {
                var productViewModel = product.Data!;
                return View(productViewModel);
            }

            TempData["error"] = product.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
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
    public async Task<ActionResult> Create(CreateProductViewModel createProductViewModel)
    {
        try
        {
            ProductResultViewModel response = await productService.CreateProductAsync(createProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Create));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: ProductController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var product = await productService.GetProductAsync(id);

            if (product.IsSuccess)
            {
                UpdateProductViewModel updateUserViewModel = new()
                {
                    Id = product.Data!.Id,
                    Name = product.Data.Name,
                    Description = product.Data.Description,
                    ProductCategory = product.Data.ProductCategory,
                    Price = product.Data.Price,
                };

                return View(updateUserViewModel);
            }

            TempData["error"] = product.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // POST: ProductController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UpdateProductViewModel updateProductViewModel)
    {
        try
        {
            var response = await productService.UpdateProductAsync(updateProductViewModel.Id, updateProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Edit));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // POST: ProductController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(DeleteProductViewModel deleteProductViewModel)
    {
        try
        {
            var response = await productService.DeleteProductAsync(deleteProductViewModel.Id, deleteProductViewModel);
            await shoppingCartService.RemoveCartDetailsAsync(deleteProductViewModel.Id);
            await favouriteService.DeleteFavouriteProductAsync(deleteProductViewModel.Id);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> DeleteProductList(List<int> selectedProducts)
    {
        try
        {
            if (selectedProducts.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один продукт для удаления.";
                return RedirectToAction(nameof(GetProducts));
            }

            var deleteProducts = new DeleteProductsViewModel(selectedProducts);
            var response = await productService.DeleteProductsAsync(deleteProducts);

            await shoppingCartService.RemoveCartDetailsListAsync(deleteProducts);
            await favouriteService.DeleteFavouriteProductsAsync(deleteProducts);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetProducts));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(GetProducts));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}