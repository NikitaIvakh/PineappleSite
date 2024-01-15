using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models;
using PineappleSite.Presentation.Models.Coupons;

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
                ResponseViewModel response = await _couponService.CreateCouponAsync(couponViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Create));
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: CouponController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var coupon = await _couponService.GetCouponAsync(id);
            return View(coupon);
        }

        // POST: CouponController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateCouponViewModel updateCoupon)
        {
            try
            {
                ResponseViewModel response = await _couponService.UpdateCouponAsync(id, updateCoupon);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Edit));
                }
            }

            catch
            {
                return View();
            }
        }

        // POST: CouponController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, DeleteCouponViewModel deleteCoupon)
        {
            try
            {
                ResponseViewModel response = await _couponService.DeleteCouponAsync(id, deleteCoupon);

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

            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> DeleteMultiple(List<int> selectedCoupons)
        {
            if (selectedCoupons is null || selectedCoupons.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один купон для удаления.";
                return RedirectToAction(nameof(Index));
            }

            var deleteCouponList = new DeleteCouponListViewModel { CouponIds = selectedCoupons };
            var response = await _couponService.DeleteCouponsAsync(deleteCouponList);

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
    }
}