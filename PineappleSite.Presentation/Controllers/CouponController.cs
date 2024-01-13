using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;

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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CouponController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CouponController/Create
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