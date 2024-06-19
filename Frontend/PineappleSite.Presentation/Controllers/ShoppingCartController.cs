using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;
using PineappleSite.Presentation.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Controllers;

public sealed class ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
    : Controller
{
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
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()
                ?.Value;
            var result = await shoppingCartService.GetCartAsync(userId);
            var couponCode = cartViewModel.CartHeader.CouponCode.ToLower();

            if (result.IsSuccess)
            {
                cartViewModel = new CartViewModel()
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        CartHeaderId = result.Data!.CartHeader.CartHeaderId,
                        UserId = userId,
                        CouponCode = couponCode,
                        Discount = result.Data.CartHeader.Discount,
                        CartTotal = result.Data.CartHeader.CartTotal,
                    },

                    CartDetails = result.Data.CartDetails,
                };

                var response = await shoppingCartService.ApplyCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["error"] = response.ValidationErrors;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = result.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> RemoveCoupon(CartViewModel cartViewModel)
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()
                ?.Value;
            var result = await shoppingCartService.GetCartAsync(userId!);

            if (result.IsSuccess)
            {
                cartViewModel = new CartViewModel()
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        CartHeaderId = result.Data!.CartHeader.CartHeaderId,
                        UserId = result.Data.CartHeader.UserId,
                        CouponCode = null,
                        Discount = result.Data.CartHeader.CartHeaderId,
                        CartTotal = result.Data.CartHeader.CartTotal,
                    },

                    CartDetails = result.Data.CartDetails,
                };

                var response = await shoppingCartService.RemoveCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["error"] = response.ValidationErrors;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = result.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> RemoveCartDetails(int productId)
    {
        try
        {
            var deleteProductViewModel = new DeleteProductViewModel(productId);
            var response = await shoppingCartService.RemoveCartDetailsAsync(deleteProductViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
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

    public async Task<ActionResult> RemoveCartDetailsByUser(int productId)
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
            var deleteProductByUserViewModel = new DeleteProductByUserViewModel(productId, userId!);
            var response = await shoppingCartService.RemoveCartDetailsByUserAsync(deleteProductByUserViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
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

    public async Task<IActionResult> Checkout()
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()
                ?.Value;
            var result = await shoppingCartService.GetCartAsync(userId!);

            if (result.IsSuccess)
            {
                var cartViewModel = new CartViewModel()
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        CartHeaderId = result.Data!.CartHeader.CartHeaderId,
                        UserId = userId,
                        CouponCode = result.Data.CartHeader.CouponCode,
                        Discount = result.Data.CartHeader.Discount,
                        CartTotal = result.Data.CartHeader.CartTotal,
                        Address = result.Data.CartHeader.Address,
                        DeliveryDate = result.Data.CartHeader.DeliveryDate,
                    },

                    CartDetails = result.Data.CartDetails,
                };

                return View(cartViewModel);
            }

            TempData["error"] = result.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ActionName("Checkout")]
    public async Task<ActionResult> Checkout(CartViewModel cartViewModel)
    {
        try
        {
            var deliveryDate = cartViewModel.CartHeader.DeliveryDate;
            var deliveryDateUtc = DateTime.SpecifyKind((DateTime)deliveryDate!, DateTimeKind.Utc);
            cartViewModel.CartHeader.DeliveryDate = deliveryDateUtc;
            cartViewModel.CartHeader.Email = cartViewModel.CartHeader.Email!.ToLower();

            var cart = await GetShoppingCartAfterAuthenticate();
            cart.Data!.CartHeader.PhoneNumber = cartViewModel.CartHeader.PhoneNumber;
            cart.Data.CartHeader.Email = cartViewModel.CartHeader.Email!.ToLower();
            cart.Data.CartHeader.Name = cartViewModel.CartHeader.Name;
            cart.Data.CartHeader.Address = cartViewModel.CartHeader.Address;
            cart.Data.CartHeader.DeliveryDate = cartViewModel.CartHeader.DeliveryDate;
            cartViewModel.CartDetails = cart.Data.CartDetails;
            cartViewModel.CartHeader.CouponCode = cart.Data.CartHeader.CouponCode;
            cartViewModel.CartHeader.Discount = cart.Data.CartHeader.Discount;

            var response = await orderService.CreateOrderAsync(cartViewModel);
            var orderHeaderDto = response.Data;

            if (response.IsSuccess)
            {
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestViewModel stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "ShoppingCart/Confirmation?orderId=" + orderHeaderDto!.OrderHeaderId,
                    CancelUrl = domain + "ShoppingCart/Checkout",
                    OrderHeader = orderHeaderDto,
                };

                var stripeResponse = await orderService.CreateStripeSessionAsync(stripeRequestDto);
                var stripeResponseResult = stripeResponse.Data;
                Response.Headers.Append("Location", stripeResponseResult!.StripeSessionUrl);
                return new StatusCodeResult(303);
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Checkout));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> Confirmation(int orderId)
    {
        try
        {
            var validateStripSessionViewModel = new ValidateStripSessionViewModel(orderId);
            var response = await orderService.ValidateStripeSessionAsync(validateStripSessionViewModel);

            if (!response.IsSuccess)
            {
                return View(orderId);
            }

            var orderHeaderDto = response.Data;

            if (orderHeaderDto!.Status == StaticDetails.StatusApproved)
            {
                TempData["success"] = response.SuccessMessage;
                return View(orderId);
            }

            TempData["error"] = response.ValidationErrors;
            return View(orderId);
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<ActionResult> RabbitMqShoppingCart(CartViewModel cartViewModel)
    {
        try
        {
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()
                ?.Value;
            var response = await shoppingCartService.GetCartAsync(userId);

            if (response.IsSuccess)
            {
                cartViewModel = new CartViewModel
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        CartHeaderId = response.Data!.CartHeader.CartHeaderId,
                        UserId = userId,
                        CouponCode = response.Data.CartHeader.CouponCode,
                        Discount = response.Data.CartHeader.Discount,
                        CartTotal = response.Data.CartHeader.CartTotal,
                        Name = response.Data.CartHeader.Name,
                        PhoneNumber = response.Data.CartHeader.PhoneNumber,
                        Email = response.Data.CartHeader.Email,
                    },

                    CartDetails = response.Data.CartDetails,
                };

                var result = await shoppingCartService.RabbitMqShoppingCartAsync(cartViewModel);

                if (result.IsSuccess)
                {
                    TempData["success"] = result.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                TempData["error"] = result.ValidationErrors;
                return RedirectToAction(nameof(Index));
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

    private async Task<CartResult<CartViewModel>> GetShoppingCartAfterAuthenticate()
    {
        var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
        var result = await shoppingCartService.GetCartAsync(userId!);

        return result;
    }
}