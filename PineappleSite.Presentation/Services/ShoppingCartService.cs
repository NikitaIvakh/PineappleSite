using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
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
            CartDtoResult cartDto = await _shoppingCartClient.GetShoppingCartAsync(userId);
            CartResult<CartViewModel> result = new()
            {
                Data = _mapper.Map<CartViewModel>(cartDto.Data),
                ErrorMessage = cartDto.ErrorMessage,
                ErrorCode = cartDto.ErrorCode,
                SuccessMessage = cartDto.SuccessMessage,
            };

            return result;
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

        public async Task<CartResult<CartHeaderViewModel>> ApplyCouponAsync(CartHeaderViewModel cartHeaderViewModel)
        {
            AddBearerToken();
            try
            {
                CartHeaderDto cartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeaderViewModel);
                CartHeaderDtoResult apiResult = await _shoppingCartClient.ApplyCouponAsync(cartHeaderDto);

                if (apiResult.IsSuccess)
                {
                    return new CartResult<CartHeaderViewModel>
                    {
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<CartHeaderViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new CartResult<CartHeaderViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
                        };
                    }
                }

                return new CartResult<CartHeaderViewModel>();
            }

            catch (ShoppingCartExceptions exceptions)
            {
                return new CartResult<CartHeaderViewModel>
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
                        ErrorCode = apiResult.ErrorCode,
                        ErrorMessage = apiResult.ErrorMessage,
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

        public async Task<CartResult<CartViewModel>> RemoveCartDetailsAsync(int cartDetailsId)
        {
            AddBearerToken();
            try
            {
                CartDtoResult apiResult = await _shoppingCartClient.ShoppingCartDELETEAsync(cartDetailsId);

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
    }
}