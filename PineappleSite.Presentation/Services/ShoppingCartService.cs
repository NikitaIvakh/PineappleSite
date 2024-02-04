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

        public async Task<CartResultViewModel<CartViewModel>> GetShoppingCartAsync(string userId)
        {
            AddBearerToken();
            try
            {
                CartDtoResult shoppingCart = await _shoppingCartClient.GetCartAsync(userId);

                if (shoppingCart.IsSuccess)
                {
                    CartResultViewModel<CartViewModel> resultViewModel = new()
                    {
                        ErrorCode = shoppingCart.ErrorCode,
                        ErrorMessage = shoppingCart.ErrorMessage,
                        SuccessMessage = shoppingCart.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(shoppingCart.Data),
                    };

                    return resultViewModel;
                }

                else
                {
                    foreach (var error in shoppingCart.ValidationErrors)
                    {
                        return new CartResultViewModel<CartViewModel>
                        {
                            ErrorCode = shoppingCart.ErrorCode,
                            ErrorMessage = shoppingCart.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResultViewModel<CartViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartHeaderViewModel>> CartUpsertAsync(CartViewModel cartViewModel)
        {
            try
            {
                AddBearerToken();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.CartUpsertAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartHeaderViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartHeaderViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartHeaderViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartHeaderViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartHeaderViewModel>> ApplyCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                AddBearerToken();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.ApplyCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartHeaderViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartHeaderViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartHeaderViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartHeaderViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartHeaderViewModel>> RemoveCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                AddBearerToken();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.RemoveCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartHeaderViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartHeaderViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartHeaderViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartHeaderViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartHeaderViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartDetailsViewModel>> RemoveCartDetailsAsync(int cartDEtailsId)
        {
            try
            {
                AddBearerToken();
                CartDtoResult apiResponse = await _shoppingCartClient.RemoveCartAsync(cartDEtailsId.ToString(), cartDEtailsId);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartDetailsViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartDetailsViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartDetailsViewModel>()
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartDetailsViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartDetailsViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }
    }
}