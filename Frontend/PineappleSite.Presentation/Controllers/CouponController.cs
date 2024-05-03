using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Controllers;

public sealed class CouponController(ICouponService couponService, IShoppingCartService shoppingCartService)
    : Controller
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

                    coupons = new CollectionResultViewModel<CouponViewModel>
                    {
                        Data = filteredCouponsList
                    };
                }

                ViewData["SearchCode"] = searchCode;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageSize = 10;
                var filteredCoupons = coupons.Data!.AsQueryable();
                var paginatedCoupons =
                    PaginatedList<CouponViewModel>.Create(filteredCoupons, pageNumber ?? 1, pageSize);

                if (paginatedCoupons.Count != 0)
                {
                    return View(paginatedCoupons);
                }

                TempData["error"] = "Нет результатов";
                return RedirectToAction(nameof(Index));
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
        try
        {
            var coupon = await couponService.GetCouponByIdAsync(couponId);

            if (coupon.IsSuccess)
            {
                return View(coupon);
            }

            TempData["error"] = coupon.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: CouponController/Create
    public Task<ActionResult> Create()
    {
        return Task.FromResult<ActionResult>(View());
    }

    // POST: CouponController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateCouponViewModel couponViewModel, int? pageNumber)
    {
        try
        {
            ResultViewModel response = await couponService.CreateCouponAsync(couponViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Create), new { pageNumber = pageNumber });
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }
    }

    // GET: CouponController/Edit/5
    public async Task<ActionResult> Edit(string couponId)
    {
        try
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

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: CouponController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(string couponId, UpdateCouponViewModel updateCoupon, int? pageNumber)
    {
        try
        {
            var response = await couponService.UpdateCouponAsync(couponId, updateCoupon);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Edit), new { pageNumber = pageNumber });
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }
    }

    // POST: CouponController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(string couponId, DeleteCouponViewModel deleteCoupon, int? pageNumber)
    {
        try
        {
            var coupon = await couponService.GetCouponByIdAsync(couponId);
            var deleteCouponByCode = new DeleteCouponByCodeViewModel(coupon.Data!.CouponCode);
            await shoppingCartService.RemoveCouponByCode(deleteCouponByCode);

            var response = await couponService.DeleteCouponAsync(couponId, deleteCoupon);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }
    }

    public async Task<ActionResult> DeleteMultiple(List<string> selectedCoupons, int? pageNumber)
    {
        try
        {
            var coupons = await couponService.GetAllCouponsAsync();
            var matchingCoupons = new List<string>();

            foreach (var selectedCouponCode in selectedCoupons)
            {
                foreach (var coupon in coupons.Data!)
                {
                    if (coupon.CouponId != selectedCouponCode)
                    {
                        continue;
                    }

                    matchingCoupons.Add(coupon.CouponCode);
                    break;
                }
            }

            await shoppingCartService.RemoveCouponsByCode(new DeleteCouponsByCodeViewModel(matchingCoupons));

            DeleteCouponsViewModel deleteCoupons = new(selectedCoupons);
            var response = await couponService.DeleteCouponsAsync(deleteCoupons);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(Index), new { pageNumber = pageNumber });
        }
    }
}