using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;
using PineappleSite.Presentation.Utility;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers
{
    public class ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService) : Controller
    {
        private readonly IShoppingCartService _shoppingCartService = shoppingCartService;
        private readonly IOrderService _orderService = orderService;

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
                string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
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
                        foreach (var error in response.ValidationErrors!)
                        {
                            TempData["error"] = error;
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }

                else
                {
                    foreach (var error in result.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
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
                string? userid = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
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
                        foreach (var error in response.ValidationErrors!)
                        {
                            TempData["error"] = error;
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }

                else
                {
                    foreach (var error in result.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
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
                string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
                var response = await _shoppingCartService.RemoveCartDetailsAsync(productId);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
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
                string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
                CartResult<CartViewModel> result = await _shoppingCartService.GetCartAsync(userId);

                if (result.IsSuccess)
                {
                    CartViewModel cartViewModel = new()
                    {
                        CartHeader = new CartHeaderViewModel
                        {
                            CartHeaderId = result.Data.CartHeader.CartHeaderId,
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

                else
                {
                    foreach (var error in result.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<ActionResult> Checkout(CartViewModel cartViewModel)
        {
            try
            {
                var deliveryDate = cartViewModel.CartHeader.DeliveryDate;
                var deliveryDateUtc = DateTime.SpecifyKind((DateTime)deliveryDate, DateTimeKind.Utc);
                cartViewModel.CartHeader.DeliveryDate = deliveryDateUtc;

                CartResult<CartViewModel> cart = await GetShoppingCartAfterAuthenticate();
                cart.Data.CartHeader.PhoneNumber = cartViewModel.CartHeader.PhoneNumber;
                cart.Data.CartHeader.Email = cartViewModel.CartHeader.Email;
                cart.Data.CartHeader.Name = cartViewModel.CartHeader.Name;
                cart.Data.CartHeader.Address = cartViewModel.CartHeader.Address;
                cart.Data.CartHeader.DeliveryDate = cartViewModel.CartHeader.DeliveryDate;
                cartViewModel.CartDetails = cart.Data.CartDetails;
                cartViewModel.CartHeader.CouponCode = cart.Data.CartHeader.CouponCode;
                cartViewModel.CartHeader.Discount = cart.Data.CartHeader.Discount;

                var response = await _orderService.CreateOrderAsync(cartViewModel);
                OrderHeaderViewModel orderHeaderDto = response.Data;

                if (response is not null && response.IsSuccess)
                {
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                    StripeRequestViewModel stripeRequestDto = new()
                    {
                        ApprovedUrl = domain + "ShoppingCart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                        CancelUrl = domain + "ShoppingCart/Checkout",
                        OrderHeader = orderHeaderDto,
                    };

                    var stripeResponse = await _orderService.CreateStripeSessionAsync(stripeRequestDto);
                    StripeRequestViewModel stripeResponseResult = stripeResponse.Data;
                    Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                    return new StatusCodeResult(303);
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Checkout));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        public async Task<ActionResult> Confirmation(int orderId)
        {
            try
            {
                var response = await _orderService.ValidateStripeSessionAsync(orderId);

                if (response is not null & response.IsSuccess)
                {
                    OrderHeaderViewModel orderHeaderDto = response.Data;

                    if (orderHeaderDto.Status == StaticDetails.StatusApproved)
                    {
                        TempData["success"] = response.SuccessMessage;
                        return View(orderId);
                    }

                    else
                    {
                        foreach (var error in response.ValidationErrors!)
                        {
                            TempData["error"] = error;
                        }

                        return View(orderId);
                    }
                }

                return View(orderId);
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        public async Task<ActionResult> RabbitMQShoppingCart(CartViewModel cartViewModel)
        {
            try
            {
                string userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
                CartResult<CartViewModel> response = await _shoppingCartService.GetCartAsync(userId);

                if (response.IsSuccess)
                {
                    cartViewModel = new CartViewModel
                    {
                        CartHeader = new CartHeaderViewModel
                        {
                            CartHeaderId = response.Data.CartHeader.CartHeaderId,
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

                    var result = await _shoppingCartService.RabbitMQShoppingCartAsync(cartViewModel);

                    if (result.IsSuccess)
                    {
                        TempData["success"] = result.SuccessMessage;
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        foreach (var error in result.ValidationErrors!)
                        {
                            TempData["error"] = error;
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<CartResult<CartViewModel>> GetShoppingCartAfterAuthenticate()
        {
            string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;
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