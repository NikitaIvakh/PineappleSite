using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return View(await GetShoppingCartAfterAuthenticate());
        }

        // GET: ShoppingCartController/Details/5
        public async Task<ActionResult> ApplyCoupon(CartViewModel cartViewModel)
        {
            try
            {
                string? userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
                CartResult<CartViewModel> result = await _shoppingCartService.GetCartAsync(userId);

                if (result.IsSuccess)
                {
                    cartViewModel = new()
                    {
                        CartHeader = new CartHeaderViewModel
                        {
                            CartHeaderId = result.Data.CartHeader.CartHeaderId,
                            UserId = userId,
                            CouponCode = cartViewModel.CartHeader.CouponCode,
                            Discount = result.Data.CartHeader.Discount,
                            CartTotal = result.Data.CartHeader.CartTotal,
                        },
                        CartDetails = result.Data.CartDetails,
                    };

                    var response = await _shoppingCartService.ApplyCouponAsync(cartViewModel);

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

                else
                {
                    TempData["error"] = result.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(cartViewModel);
            }
        }

        public async Task<ActionResult> RemoveCoupon(CartViewModel cartViewModel)
        {
            try
            {
                string? userid = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
                CartResult<CartViewModel> result = await _shoppingCartService.GetCartAsync(userid);

                if (result.IsSuccess)
                {
                    cartViewModel = new CartViewModel
                    {
                        CartHeader = new CartHeaderViewModel
                        {
                            CartHeaderId = result.Data.CartHeader.CartHeaderId,
                            UserId = result.Data.CartHeader.UserId,
                            CouponCode = null,
                            Discount = result.Data.CartHeader.CartHeaderId,
                            CartTotal = result.Data.CartHeader.CartTotal,
                        },
                        CartDetails = result.Data.CartDetails,
                    };

                    var response = await _shoppingCartService.RemoveCouponAsync(cartViewModel);

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

                else
                {
                    TempData["error"] = result.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View(cartViewModel);
            }
        }


        private async Task<CartResult<CartViewModel>> GetShoppingCartAfterAuthenticate()
        {
            string? userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
            CartResult<CartViewModel> result = await _shoppingCartService.GetCartAsync(userId);

            if (result.IsSuccess)
            {
                CartResult<CartViewModel> apiResult = new()
                {
                    Data = result.Data,
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage,
                    SuccessMessage = result.SuccessMessage,
                    ValidationErrors = result.ValidationErrors,
                };

                return apiResult;
            }

            return result;
        }
    }
}