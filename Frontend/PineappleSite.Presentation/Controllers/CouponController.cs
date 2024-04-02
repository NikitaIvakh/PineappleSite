using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Controllers
{
    public class CouponController(ICouponService couponService) : Controller
    {
        private readonly ICouponService _couponService = couponService;

        // GET: CouponController
        public async Task<ActionResult> Index(string searchCode, string currentFilter, int? pageNumber)
        {
            try
            {
                var coupons = await _couponService.GetAllCouponsAsync();

                if (coupons.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(searchCode))
                    {
                        var filteredCouponsList = coupons.Data.Where(
                            key => key.CouponCode.Contains(searchCode, StringComparison.CurrentCultureIgnoreCase) ||
                            key.DiscountAmount.ToString().Contains(searchCode, StringComparison.CurrentCultureIgnoreCase) ||
                            key.MinAmount.ToString().Contains(searchCode, StringComparison.CurrentCultureIgnoreCase)).ToList();

                        coupons = new CollectionResultViewModel<GetCouponsViewModel>
                        {
                            Data = filteredCouponsList
                        };
                    }

                    ViewData["SearchCode"] = searchCode;
                    ViewData["CurrentFilter"] = currentFilter;

                    int pageSize = 10;
                    var filteredCoupons = coupons.Data.AsQueryable();
                    var paginatedCoupons = PaginatedList<GetCouponsViewModel>.Create(filteredCoupons, pageNumber ?? 1, pageSize);

                    if (paginatedCoupons.Count == 0)
                    {
                        return View();
                    }

                    return View(paginatedCoupons);
                }

                else
                {
                    foreach (var error in coupons.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return View();
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: CouponController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var coupon = await _couponService.GetCouponAsync(id);

            if (coupon.IsSuccess)
            {
                CouponViewModel couponViewModel = new()
                {
                    CouponId = coupon.Data.CouponId,
                    CouponCode = coupon.Data.CouponCode,
                    DiscountAmount = coupon.Data.DiscountAmount,
                    MinAmount = coupon.Data.MinAmount,
                };

                return View(couponViewModel);
            }

            else
            {
                foreach (var error in coupon.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction(nameof(Index));
            }
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
                ResultViewModel response = await _couponService.CreateCouponAsync(couponViewModel);

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

            if (coupon.IsSuccess)
            {
                UpdateCouponViewModel couponViewModel = new()
                {
                    CouponId = coupon.Data.CouponId,
                    CouponCode = coupon.Data.CouponCode,
                    DiscountAmount = coupon.Data.DiscountAmount,
                    MinAmount = coupon.Data.MinAmount,
                };

                return View(couponViewModel);
            }

            else
            {
                foreach (var error in coupon.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CouponController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateCouponViewModel updateCoupon)
        {
            try
            {
                ResultViewModel response = await _couponService.UpdateCouponAsync(id, updateCoupon);

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
        public async Task<ActionResult> Delete(int couponId, DeleteCouponViewModel deleteCoupon)
        {
            try
            {
                ResultViewModel response = await _couponService.DeleteCouponAsync(couponId, deleteCoupon);

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

            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> DeleteMultiple(List<int> selectedCoupons)
        {
            if (selectedCoupons.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы один купон для удаления.";
                return RedirectToAction(nameof(Index));
            }

            DeleteCouponListViewModel deleteCouponList = new() { CouponIds = selectedCoupons };
            var response = await _couponService.DeleteCouponsAsync(deleteCouponList);

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
    }
}