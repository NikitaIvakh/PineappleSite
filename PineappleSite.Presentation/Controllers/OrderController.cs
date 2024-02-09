using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Utility;

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
            string userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;

            var response = await _orderService.GetAllOrdersAsync(userId);

            if (response is not null && response.IsSuccess)
            {
                orderHeaderDtos = response.Data;
                switch (status)
                {
                    case "approved":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.Status_Approved);
                        break;

                    case "readyforpickup":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.Status_ReadyForPickup);
                        break;

                    case "cancelled":
                        orderHeaderDtos = orderHeaderDtos.Where(key => key.Status == StaticDetails.Status_Cancelled || key.Status == StaticDetails.Status_Refunded);
                        break;

                    default:
                        break;
                }
            }

            else
                orderHeaderDtos = new List<OrderHeaderViewModel>();

            return Json(new { data = orderHeaderDtos.OrderByDescending(key => key.OrderHeaderId) });
        }

        public async Task<ActionResult> GetOrderDetails(int orderId)
        {
            OrderHeaderViewModel orderHeaderDto = new();
            string userId = User.Claims.Where(key => key.Type == "uid")?.FirstOrDefault()?.Value;

            var response = await _orderService.GetOrderAsync(orderId);

            if (response is not null && response.IsSuccess)
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderViewModel>(Convert.ToString(response.Data));

            if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderHeaderDto.UserId)
                return NotFound();

            return View(orderHeaderDto);
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<ActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.Status_ReadyForPickup);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<ActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.Status_Completed);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, StaticDetails.Status_Cancelled);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
            }

            return View();
        }
    }
}
