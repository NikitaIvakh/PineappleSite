using AutoMapper;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Orders;

namespace PineappleSite.Presentation.Services
{
    public class OrderService(ILocalStorageService localStorageService, IOrderClient orderClient, IMapper mapper) : BaseOrderService(localStorageService, orderClient), IOrderService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IOrderClient _orderClient = orderClient;
        private readonly IMapper _mapper = mapper;

        public async Task<OrderCollectionResult<OrderHeaderViewModel>> GetAllOrdersAsync(string userId)
        {
            try
            {
                var result = await _orderClient.GetAllOrdersAsync(userId);

                if (result.IsSuccess)
                {
                    OrderCollectionResult<OrderHeaderViewModel> response = new()
                    {
                        Data = _mapper.Map<IReadOnlyCollection<OrderHeaderViewModel>>(result.Data),
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage,
                        Count = result.Count,
                        SuccessMessage = result.SuccessMessage,
                    };

                    return response;
                }

                else
                {
                    foreach (var error in result.ValidationErrors)
                    {
                        return new OrderCollectionResult<OrderHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = result.ErrorCode,
                            ErrorMessage = result.ErrorMessage,
                        };
                    }
                }

                return new OrderCollectionResult<OrderHeaderViewModel>();
            }

            catch (JsonSerializationException ex)
            {
                return new OrderCollectionResult<OrderHeaderViewModel>
                {
                    ErrorMessage = "An error occurred while deserializing the response.",
                    ErrorCode = 500,
                    ValidationErrors = ex.Message,
                };
            }

            catch (OrdersExceptions exceptions)
            {
                return new OrderCollectionResult<OrderHeaderViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = exceptions.Message,
                };
            }
        }

        public Task<OrderResult<OrderHeaderViewModel>> GetOrderAsync(int orderHeaderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult<OrderHeaderViewModel>> CreateOrderAsync(CartViewModel cartViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult<StripeRequestViewModel>> CreateStripeSessionAsync(StripeRequestViewModel stripeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult<OrderHeaderViewModel>> ValidateStripeSessionAsync(int orderHeaderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult<OrderHeaderViewModel>> UpdateOrderStatusAsync(int orderHeaderId, string newStatus)
        {
            throw new NotImplementedException();
        }
    }
}