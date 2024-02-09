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
            AddBearerToken();
            try
            {
                var result = await _orderClient.GetAllOrdersAsync(userId);

                if (result.IsSuccess)
                {
                    return new OrderCollectionResult<OrderHeaderViewModel>
                    {
                        SuccessMessage = result.SuccessMessage,
                        Data = _mapper.Map<IReadOnlyCollection<OrderHeaderViewModel>>(result.Data),
                    };
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

        public async Task<OrderResult<OrderHeaderViewModel>> GetOrderAsync(int orderHeaderId)
        {
            try
            {
                var response = await _orderClient.GetOrderAsync(orderHeaderId);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        Data = _mapper.Map<OrderHeaderViewModel>(response.Data),
                        SuccessMessage = response.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<OrderHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorMessage = response.ErrorMessage,
                            ErrorCode = response.ErrorCode,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exception)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode
                };
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> CreateOrderAsync(CartViewModel cartViewModel)
        {
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                var response = await _orderClient.CreateOrderAsync(cartDto);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        Data = _mapper.Map<OrderHeaderViewModel>(response.Data),
                        SuccessMessage = response.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<OrderHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exception)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode
                };
            }
        }

        public async Task<OrderResult<StripeRequestViewModel>> CreateStripeSessionAsync(StripeRequestViewModel stripeRequest)
        {
            try
            {
                StripeRequestDto stripeRequestDto = _mapper.Map<StripeRequestDto>(stripeRequest);
                StripeRequestDtoResult response = await _orderClient.CreateStripeSessionAsync(stripeRequestDto);

                if (response.IsSuccess)
                {
                    return new OrderResult<StripeRequestViewModel>
                    {
                        Data = _mapper.Map<StripeRequestViewModel>(response.Data),
                        SuccessMessage = response.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<StripeRequestViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<StripeRequestViewModel>();
            }

            catch (OrdersExceptions exception)
            {
                return new OrderResult<StripeRequestViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode
                };
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> ValidateStripeSessionAsync(int orderHeaderId)
        {
            try
            {
                var response = await _orderClient.ValidateStripeSessionAsync(orderHeaderId);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessMessage = response.SuccessMessage,
                        Data = _mapper.Map<OrderHeaderViewModel>(response.Data),
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<OrderHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exception)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> UpdateOrderStatusAsync(int orderHeaderId, string newStatus)
        {
            try
            {
                var response = await _orderClient.UpdateOrderStatusAsync(orderHeaderId, newStatus);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessMessage = response.SuccessMessage,
                        Data = _mapper.Map<OrderHeaderViewModel>(response),
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<OrderHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exception)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }
    }
}