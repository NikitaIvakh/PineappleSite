using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Utility;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers
{
    public class OrderController(IOrderService orderService) : Controller
    {
        private readonly IOrderService _orderService = orderService;

        // GET: OrderController
        public async Task<ActionResult> OrderIndex()
        {
            return View();
        }

        public async Task<ActionResult> GetAllOrders(string status)
        {
            IEnumerable<OrderHeaderViewModel> orderHeaderDtos;
            string? userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;

            var response = await _orderService.GetAllOrdersAsync(userId);

            if (response is not null && response.IsSuccess)
            {
                orderHeaderDtos = response.Data;
                switch (status)
                {
                    case "approved":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.StatusApproved);
                        break;

                    case "readyforpickup":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.StatusReadyForPickup);
                        break;

                    case "cancelled":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.StatusCancelled || key.Status == StaticDetails.StatusRefunded);
                        break;

                    default:
                        break;
                }
            }

            else
            {
                foreach (var error in response!.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction("Index", "Home");
            }

            return Json(new { data = orderHeaderDtos });
        }

        public async Task<ActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                OrderHeaderViewModel orderHeaderDto = new();
                string userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()!.Value!;

                var response = await _orderService.GetOrderAsync(orderId);

                if (response is not null && response.IsSuccess)
                {
                    orderHeaderDto = response.Data;

                    if (orderHeaderDto is not null)
                    {
                        return View(orderHeaderDto);
                    }

                    else
                    {
                        foreach (var error in response.ValidationErrors!)
                        {
                            TempData["error"] = error;
                        }

                        return RedirectToAction(nameof(OrderIndex));
                    }
                }

                else
                {
                    return RedirectToAction(nameof(OrderIndex), new { orderId = orderId });
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return RedirectToAction(nameof(OrderIndex));
            }
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<ActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.StatusReadyForPickup);

            if (!response.IsSuccess) 
                return RedirectToAction(nameof(OrderIndex));
            
            TempData["success"] = response.SuccessMessage;
            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
        }

        [HttpPost("CompleteOrder")]
        public async Task<ActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.StatusCompleted);

            if (!response.IsSuccess) 
                return RedirectToAction(nameof(OrderIndex));
            TempData["success"] = response.SuccessMessage;
            
            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
        }

        [HttpPost("CancelOrder")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.StatusCancelled);

            if (!response.IsSuccess) 
                return RedirectToAction(nameof(OrderIndex));
            
            TempData["success"] = response.SuccessMessage;
            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });

        }
    }
}
