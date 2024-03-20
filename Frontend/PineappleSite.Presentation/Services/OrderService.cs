using AutoMapper;
using Newtonsoft.Json;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Orders;

namespace PineappleSite.Presentation.Services
{
    public class OrderService(ILocalStorageService localStorageService, IOrderClient orderClient, IMapper mapper, IHttpContextAccessor contextAccessor) : BaseOrderService(localStorageService, orderClient, contextAccessor), IOrderService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IOrderClient _orderClient = orderClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<OrderCollectionResult<OrderHeaderViewModel>> GetAllOrdersAsync(string userId)
        {
            AddBearerToken();
            if (_contextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                try
                {
                    var result = await _orderClient.GetAllOrdersAsync(userId);

                    if (result.IsSuccess)
                    {
                        return new OrderCollectionResult<OrderHeaderViewModel>
                        {
                            SuccessCode = result.SuccessCode,
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
                                ValidationErrors = [error],
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
                        ValidationErrors = [ex.Message],
                    };
                }

                catch (OrdersExceptions exceptions)
                {
                    if (exceptions.StatusCode == 403)
                    {
                        return new OrderCollectionResult<OrderHeaderViewModel>
                        {
                            ErrorCode = 403,
                            ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                            ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                        };
                    }

                    else if (exceptions.StatusCode == 401)
                    {
                        return new OrderCollectionResult<OrderHeaderViewModel>
                        {
                            ErrorCode = 401,
                            ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                            ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                        };
                    }

                    else
                    {
                        return new OrderCollectionResult<OrderHeaderViewModel>
                        {
                            ErrorMessage = exceptions.Response,
                            ErrorCode = exceptions.StatusCode,
                            ValidationErrors = [exceptions.Response],
                        };
                    }
                }
            }

            else
            {
                return new OrderCollectionResult<OrderHeaderViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> GetOrderAsync(int orderHeaderId)
        {
            AddBearerToken();
            try
            {
                var response = await _orderClient.GetOrderAsync(orderHeaderId);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessCode = response.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 403,
                        ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                        ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 401,
                        ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                        ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                    };
                }

                else
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response],
                    };
                }
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> CreateOrderAsync(CartViewModel cartViewModel)
        {
            AddBearerToken();
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                var response = await _orderClient.CreateOrderAsync(cartDto);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessCode = response.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 403,
                        ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                        ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 401,
                        ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                        ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                    };
                }

                else
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response],
                    };
                }
            }
        }

        public async Task<OrderResult<StripeRequestViewModel>> CreateStripeSessionAsync(StripeRequestViewModel stripeRequest)
        {
            AddBearerToken();
            try
            {
                StripeRequestDto stripeRequestDto = _mapper.Map<StripeRequestDto>(stripeRequest);
                StripeRequestDtoResult response = await _orderClient.CreateStripeSessionAsync(stripeRequestDto);

                if (response.IsSuccess)
                {
                    return new OrderResult<StripeRequestViewModel>
                    {
                        SuccessCode = response.SuccessCode,
                        SuccessMessage = response.SuccessMessage,
                        Data = _mapper.Map<StripeRequestViewModel>(response.Data),
                    };
                }

                else
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        return new OrderResult<StripeRequestViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<StripeRequestViewModel>();
            }

            catch (OrdersExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new OrderResult<StripeRequestViewModel>()
                    {
                        ErrorCode = 403,
                        ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                        ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new OrderResult<StripeRequestViewModel>()
                    {
                        ErrorCode = 401,
                        ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                        ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                    };
                }

                else
                {
                    return new OrderResult<StripeRequestViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response],
                    };
                }
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> ValidateStripeSessionAsync(int orderHeaderId)
        {
            AddBearerToken();
            try
            {
                var response = await _orderClient.ValidateStripeSessionAsync(orderHeaderId);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessCode = response.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 403,
                        ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                        ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 401,
                        ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                        ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                    };
                }

                else
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response],
                    };
                }
            }
        }

        public async Task<OrderResult<OrderHeaderViewModel>> UpdateOrderStatusAsync(int orderHeaderId, string newStatus)
        {
            AddBearerToken();
            try
            {
                var response = await _orderClient.UpdateOrderStatusAsync(orderHeaderId, newStatus);

                if (response.IsSuccess)
                {
                    return new OrderResult<OrderHeaderViewModel>
                    {
                        SuccessCode = response.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = response.ErrorCode,
                            ErrorMessage = response.ErrorMessage,
                        };
                    }
                }

                return new OrderResult<OrderHeaderViewModel>();
            }

            catch (OrdersExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 403,
                        ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                        ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorCode = 401,
                        ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                        ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                    };
                }

                else
                {
                    return new OrderResult<OrderHeaderViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response],
                    };
                }
            }
        }
    }
}