using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Utility;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers;

public sealed class OrderController(IOrderService orderService) : Controller
{
    // GET: OrderController
    public Task<ActionResult> OrderIndex()
    {
        return Task.FromResult<ActionResult>(View());
    }

    public async Task<ActionResult> GetAllOrders(string status)
    {
        try
        {
            var orderHeaderDtos = Enumerable.Empty<OrderHeaderViewModel>();
            var userId = User.Claims.Where(key => key.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value;

            var response = await orderService.GetAllOrdersAsync(userId!);

            if (response.IsSuccess)
            {
                orderHeaderDtos = status switch
                {
                    "approved" => orderHeaderDtos?.Where(key => key.Status == StaticDetails.StatusApproved),
                    "readyforpickup" => orderHeaderDtos?.Where(key => key.Status == StaticDetails.StatusReadyForPickup),
                    "cancelled" => orderHeaderDtos?.Where(key =>
                        key.Status is StaticDetails.StatusCancelled or StaticDetails.StatusRefunded),
                    _ => response.Data
                };
            }

            else
            {
                TempData["error"] = response.ValidationErrors;
                return RedirectToAction("Index", "Home");
            }

            return Json(new { data = orderHeaderDtos });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> GetOrderDetails(int orderId)
    {
        try
        {
            var response = await orderService.GetOrderAsync(orderId);

            if (!response.IsSuccess)
            {
                return RedirectToAction(nameof(OrderIndex), new { orderId = orderId });
            }

            var orderHeaderDto = response.Data;

            if (orderHeaderDto is not null)
            {
                return View(orderHeaderDto);
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(OrderIndex));
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
        try
        {
            var updateOrderStatusViewModel =
                new UpdateOrderStatusViewModel(orderId, StaticDetails.StatusReadyForPickup);
            var response = await orderService.UpdateOrderStatusAsync(updateOrderStatusViewModel);

            if (!response.IsSuccess)
                return RedirectToAction(nameof(OrderIndex));

            TempData["success"] = response.SuccessMessage;
            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(OrderIndex));
        }
    }

    [HttpPost("CompleteOrder")]
    public async Task<ActionResult> CompleteOrder(int orderId)
    {
        try
        {
            var updateOrderStatusViewModel =
                new UpdateOrderStatusViewModel(orderId, StaticDetails.StatusCompleted);
            var response = await orderService.UpdateOrderStatusAsync(updateOrderStatusViewModel);

            if (!response.IsSuccess)
                return RedirectToAction(nameof(OrderIndex));
            TempData["success"] = response.SuccessMessage;

            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(OrderIndex));
        }
    }

    [HttpPost("CancelOrder")]
    public async Task<ActionResult> CancelOrder(int orderId)
    {
        try
        {
            var updateOrderStatusViewModel =
                new UpdateOrderStatusViewModel(orderId, StaticDetails.StatusCancelled);
            var response = await orderService.UpdateOrderStatusAsync(updateOrderStatusViewModel);

            if (!response.IsSuccess)
                return RedirectToAction(nameof(OrderIndex));

            TempData["success"] = response.SuccessMessage;
            return RedirectToAction(nameof(GetOrderDetails), new { orderId = orderId });
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction(nameof(OrderIndex));
        }
    }
}