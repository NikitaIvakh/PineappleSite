using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Controllers;

public class CouponController(ICouponService couponService) : Controller
{
    // GET: CouponController
    public async Task<ActionResult> Index(string searchCode, string currentFilter, int? pageNumber)
    {
        try
        {
            var coupons = await couponService.GetAllCouponsAsync();

            if (coupons.IsSuccess)
            {
                if (!string.IsNullOrEmpty(searchCode))
                {
                    var filteredCouponsList = coupons.Data!.Where(
                            key => key.CouponCode.Contains(searchCode, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.DiscountAmount.ToString(CultureInfo.InvariantCulture)
                                       .Contains(searchCode, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.MinAmount.ToString(CultureInfo.InvariantCulture)
                                       .Contains(searchCode, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    coupons = new CollectionResultViewModel<GetCouponsViewModel>
                    {
                        Data = filteredCouponsList
                    };
                }

                ViewData["SearchCode"] = searchCode;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageSize = 10;
                var filteredCoupons = coupons.Data!.AsQueryable();
                var paginatedCoupons =
                    PaginatedList<GetCouponsViewModel>.Create(filteredCoupons, pageNumber ?? 1, pageSize);

                return paginatedCoupons.Count == 0 ? View() : View(paginatedCoupons);
            }

            TempData["error"] = coupons.ValidationErrors;
            return RedirectToAction(nameof(Create));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    // GET: CouponController/Details/5
    public async Task<ActionResult> Details(string couponId)
    {
        var coupon = await couponService.GetCouponByIdAsync(couponId);

        if (coupon.IsSuccess)
        {
            return View(coupon);
        }

        TempData["error"] = coupon.ValidationErrors;
        return RedirectToAction(nameof(Index));
    }

    // GET: CouponController/Create
    public Task<ActionResult> Create()
    {
        return Task.FromResult<ActionResult>(View());
    }

    // POST: CouponController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateCouponViewModel couponViewModel)
    {
        try
        {
            ResultViewModel response = await couponService.CreateCouponAsync(couponViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Create));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
    
    // GET: CouponController/Edit/5
    public async Task<ActionResult> Edit(string couponId)
    {
        var coupon = await couponService.GetCouponByIdAsync(couponId);

        if (coupon.IsSuccess)
        {
            UpdateCouponViewModel couponViewModel = new()
            {
                CouponId = coupon.Data!.CouponId,
                CouponCode = coupon.Data.CouponCode,
                DiscountAmount = coupon.Data.DiscountAmount,
                MinAmount = coupon.Data.MinAmount,
            };

            return View(couponViewModel);
        }

        TempData["error"] = coupon.ValidationErrors;
        return RedirectToAction(nameof(Index));
    }
    
    // POST: CouponController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(string couponId, UpdateCouponViewModel updateCoupon)
    {
        try
        {
            var response = await couponService.UpdateCouponAsync(couponId, updateCoupon);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Edit));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: CouponController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(string couponId, DeleteCouponViewModel deleteCoupon)
    {
        try
        {
            var response = await couponService.DeleteCouponAsync(couponId, deleteCoupon);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
    
    public async Task<ActionResult> DeleteMultiple(List<string> selectedCoupons)
    {
        if (selectedCoupons.Count <= 1)
        {
            TempData["error"] = "Выберите хотя бы один купон для удаления.";
            return RedirectToAction(nameof(Index));
        }

        DeleteCouponsViewModel deleteCoupons = new(selectedCoupons);
        var response = await couponService.DeleteCouponsAsync(deleteCoupons);

        if (response.IsSuccess)
        {
            TempData["success"] = response.SuccessMessage;
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = response.ValidationErrors;
        return RedirectToAction(nameof(Index));
    }
}