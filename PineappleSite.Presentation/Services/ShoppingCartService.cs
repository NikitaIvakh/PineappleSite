using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Services
{
    public class ShoppingCartService(ILocalStorageService localStorageService, IShoppingCartClient shoppingCartClient, IMapper mapper) : BaseShoppingCartService(localStorageService, shoppingCartClient), IShoppingCartService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;
        private readonly IMapper _mapper = mapper;

        public async Task<CartResult<CartViewModel>> GetCartAsync(string userId)
        {
            AddBearerToken();
            try
            {
                CartDtoResult cartDto = await _shoppingCartClient.GetShoppingCartAsync(userId);
                CartResult<CartViewModel> result = new()
                {
                    Data = _mapper.Map<CartViewModel>(cartDto.Data),
                    ErrorMessage = cartDto.ErrorMessage,
                    ErrorCode = cartDto.ErrorCode,
                    SuccessMessage = cartDto.SuccessMessage,
                };

                if (result.IsSuccess)
                {
                    return result;
                }

                else
                {
                    foreach (var error in cartDto.ValidationErrors)
                    {
                        return new CartResult<CartViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = result.ErrorCode,
                            ErrorMessage = result.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResult<CartViewModel>> CartUpsertAsync(CartViewModel cartViewModel)
        {
            AddBearerToken();
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResult = await _shoppingCartClient.ShoppingCartPOSTAsync(cartDto);

                if (apiResult.IsSuccess)
                {
                    return new CartResult<CartViewModel>
                    {
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new CartResult<CartViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResult<CartViewModel>> ApplyCouponAsync(CartViewModel cartViewModel)
        {
            AddBearerToken();
            try
            {
                CartDto cartHeaderDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResult = await _shoppingCartClient.ApplyCouponAsync(cartHeaderDto);

                if (apiResult.IsSuccess)
                {
                    return new CartResult<CartViewModel>
                    {
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new CartResult<CartViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResult<CartViewModel>> RemoveCouponAsync(CartViewModel cartViewModel)
        {
            AddBearerToken();
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResult = await _shoppingCartClient.RemoveCouponAsync(cartDto);

                if (apiResult.IsSuccess)
                {
                    return new CartResult<CartViewModel>
                    {
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new CartResult<CartViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResult<CartViewModel>> RemoveCartDetailsAsync(int productId)
        {
            AddBearerToken();
            try
            {
                CartDtoResult apiResult = await _shoppingCartClient.ShoppingCartDELETEAsync(productId.ToString(), productId);

                if (apiResult.IsSuccess)
                {
                    return new CartResult<CartViewModel>
                    {
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new CartResult<CartViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResult<bool>> RabbitMQShoppingCartAsync(CartViewModel cartViewModel)
        {
            AddBearerToken();
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                BooleanResult apiResponse = await _shoppingCartClient.RabbitMQShoppingCartRequestAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResult<bool>
                    {
                        Data = apiResponse.Data,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new CartResult<bool>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new CartResult<bool>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<bool>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = exceptions.Message,
                };
            }
        }
    }
}