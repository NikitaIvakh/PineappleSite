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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShoppingCartController/Create
        public ActionResult Create()
        {
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