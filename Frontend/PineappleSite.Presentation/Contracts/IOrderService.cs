using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Orders;

namespace PineappleSite.Presentation.Contracts
{
    public interface IOrderService
    {
        Task<OrderCollectionResult<OrderHeaderViewModel>> GetAllOrdersAsync(string userId);

        Task<OrderResult<OrderHeaderViewModel>> GetOrderAsync(int orderHeaderId);

        Task<OrderResult<OrderHeaderViewModel>> CreateOrderAsync(CartViewModel cartViewModel);

        Task<OrderResult<StripeRequestViewModel>> CreateStripeSessionAsync(StripeRequestViewModel stripeRequest);

        Task<OrderResult<OrderHeaderViewModel>> ValidateStripeSessionAsync(int orderHeaderId);

        Task<OrderResult<OrderHeaderViewModel>> UpdateOrderStatusAsync(int orderHeaderId, string newStatus);
    }
}