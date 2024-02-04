using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Controllers
{
    public class ShoppingCartController(IShoppingCartService shoppingCartService) : Controller
    {
        private readonly IShoppingCartService _shoppingCartService = shoppingCartService;

        // GET: ShoppingCartController
        public async Task<ActionResult> Index()
        {
            return View(await LoadCartViewModelBasedOnLoggedInUser());
        }

        // GET: ShoppingCartController/Details/5
        public async Task<ActionResult> ApplyCoupon(CartViewModel cartViewModel)
        {
            try
            {
                CartResultViewModel<CartHeaderViewModel> response = await _shoppingCartService.ApplyCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(cartViewModel);
        }

        // GET: ShoppingCartController/Create
        public async Task<ActionResult> RemoveCoupon(CartViewModel cartViewModel)
        {
            try
            {
                CartResultViewModel<CartHeaderViewModel> response = await _shoppingCartService.RemoveCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View();
        }

        public async Task<IActionResult> RemoveDetails(int productDetailsId)
        {
            try
            {
                var userId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value;
                CartResultViewModel<CartDetailsViewModel> response = await _shoppingCartService.RemoveCartDetailsAsync(productDetailsId);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View();
        }

        private async Task<CartResultViewModel<CartViewModel>> LoadCartViewModelBasedOnLoggedInUser()
        {
            string userId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value;
            CartResultViewModel<CartViewModel> response = await _shoppingCartService.GetShoppingCartAsync(userId);

            if (response.IsSuccess)
            {
                CartResultViewModel<CartViewModel> cartResultViewModel = new()
                {
                    Data = response.Data,
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage,
                    SuccessMessage = response.SuccessMessage,
                    ValidationErrors = response.ValidationErrors,
                };

                return cartResultViewModel;
            }

            return response;
        }
    }
}