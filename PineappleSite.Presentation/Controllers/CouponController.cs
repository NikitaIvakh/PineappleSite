using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Controllers
{
    public class CouponController(ICouponService couponService) : Controller
    {
        private readonly ICouponService _couponService = couponService;

        // GET: CouponController
        public async Task<ActionResult> Index()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return View(coupons);
        }

        // GET: CouponController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var coupon = await _couponService.GetCouponAsync(id);
            return View(coupon);
        }

        // GET: CouponController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: CouponController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCouponViewModel couponViewModel)
        {
            try
            {
                var response = await _couponService.CreateCouponAsync(couponViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction(nameof(Create));
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: CouponController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CouponController/Edit/5
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

        // GET: CouponController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CouponController/Delete/5
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
    }
}