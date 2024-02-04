using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Coupons;
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
            CartDtoResult shoppingCart = await _shoppingCartClient.GetCartAsync(userId);
            CartResultViewModel<CartViewModel> resultViewModel = new()
            {
                Data = _mapper.Map<CartViewModel>(shoppingCart.Data),
                ErrorMessage = shoppingCart.ErrorMessage,
                ErrorCode = shoppingCart.ErrorCode,
                SuccessMessage = shoppingCart.SuccessMessage,
            };

            return resultViewModel;
        }

        public async Task<CartResultViewModel<CartViewModel>> CartUpsertAsync(CartViewModel cartViewModel)
        {
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.CartUpsertAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartViewModel>> ApplyCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.ApplyCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartViewModel>> RemoveCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                CartDtoResult apiResponse = await _shoppingCartClient.RemoveCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<CartResultViewModel<CartViewModel>> RemoveCartDetailsAsync(int cartDetailsId)
        {
            try
            {
                CartDtoResult apiResponse = await _shoppingCartClient.RemoveCartAsync(cartDetailsId.ToString(), cartDetailsId);

                if (apiResponse.IsSuccess)
                {
                    return new CartResultViewModel<CartViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CartViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CartResultViewModel<CartViewModel>()
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CartResultViewModel<CartViewModel>();
            }

            catch (ShoppingCartExceptions exception)
            {
                return new CartResultViewModel<CartViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }
    }
}