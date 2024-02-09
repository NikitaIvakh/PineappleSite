using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;
using PineappleSite.Presentation.Utility;

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

        public async Task<ActionResult> RemoveCartDetails(int productId)
        {
            try
            {
                string? userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;
                var response = await _shoppingCartService.RemoveCartDetailsAsync(productId);

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
                return View(productId);
            }
        }

        [ActionName("Checkout")]
        public async Task<ActionResult> Checkout(CartViewModel cartViewModel)
        {
            CartResult<CartViewModel> cart = await GetShoppingCartAfterAuthenticate();
            cartViewModel = new()
            { 
                CartHeader = new CartHeaderViewModel
                {
                    CartHeaderId = cart.Data.CartHeader.CartHeaderId,
                    UserId = cart.Data.CartHeader.UserId,
                    CouponCode = cart.Data.CartHeader.CouponCode,
                    Discount = cart.Data.CartHeader.Discount,
                    CartTotal = cart.Data.CartHeader.CartTotal,
                    Name = cart.Data.CartHeader.Name,
                    PhoneNumber = cart.Data.CartHeader.PhoneNumber,
                    Email = cart.Data.CartHeader.Email,
                },

                CartDetails = cart.Data.CartDetails,
            };

            cart.Data.CartHeader.PhoneNumber = cartViewModel.CartHeader.PhoneNumber;
            cart.Data.CartHeader.Email = cartViewModel.CartHeader.Email;
            cart.Data.CartHeader.Name = cartViewModel.CartHeader.Name;

            var response = await _orderService.CreateOrderAsync(cartViewModel);
            OrderHeaderViewModel orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderViewModel>(Convert.ToString(response.Data));

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
                StripeRequestViewModel stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestViewModel>(Convert.ToString(stripeResponse.Data));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult(303);
            }

            return View();
        }

        public async Task<ActionResult> Confirmation(int orderId)
        {
            var response = await _orderService.ValidateStripeSessionAsync(orderId);

            if (response is not null & response.IsSuccess)
            {
                OrderHeaderViewModel orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderViewModel>(Convert.ToString(response.Data));

                if (orderHeaderDto.Status == StaticDetails.Status_Approved)
                    return View(orderId);
            }

            return View(orderId);
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