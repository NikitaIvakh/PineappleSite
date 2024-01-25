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
                ShoppingCartResponseViewModel response = await _shoppingCartService.ApplyCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
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
                ShoppingCartResponseViewModel response = await _shoppingCartService.RemoveCouponAsync(cartViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View();
        }

        // POST: ShoppingCartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingCartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShoppingCartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingCartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShoppingCartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<CartViewModel> LoadCartViewModelBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(key => key.Type == "uid").FirstOrDefault()?.Value;
            ShoppingCartResponseViewModel response = await _shoppingCartService.GetShoppingCartAsync(userId);

            if (response is not null && response.IsSuccess)
            {
                CartViewModel cartViewModel = JsonConvert.DeserializeObject<CartViewModel>(Convert.ToString(response.Data));
                return cartViewModel;
            }

            return new CartViewModel();
        }
    }
}