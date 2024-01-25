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

        public async Task<ShoppingCartResponseViewModel> GetShoppingCartAsync(string userId)
        {
            var shoppingCart = await _shoppingCartClient.GetCartAsync(userId);
            return _mapper.Map<ShoppingCartResponseViewModel>(shoppingCart);
        }

        public async Task<ShoppingCartResponseViewModel> CartUpsertAsync(CartViewModel cartViewModel)
        {
            try
            {
                ShoppingCartResponseViewModel response = new();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                ShoppingCartAPIResponse apiResponse = await _shoppingCartClient.CartUpsertAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ShoppingCartExceptions exception)
            {
                return ConvertShoppingCartException(exception);
            }
        }

        public async Task<ShoppingCartResponseViewModel> ApplyCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                ShoppingCartResponseViewModel response = new();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                ShoppingCartAPIResponse apiResponse = await _shoppingCartClient.ApplyCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ShoppingCartExceptions exception)
            {
                return ConvertShoppingCartException(exception);
            }
        }

        public async Task<ShoppingCartResponseViewModel> RemoveCouponAsync(CartViewModel cartViewModel)
        {
            try
            {
                ShoppingCartResponseViewModel response = new();
                CartDto cartDto = _mapper.Map<CartDto>(cartViewModel);
                ShoppingCartAPIResponse apiResponse = await _shoppingCartClient.RemoveCouponAsync(cartDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ShoppingCartExceptions exception)
            {
                return ConvertShoppingCartException(exception);
            }
        }

        public async Task<ShoppingCartResponseViewModel> RemoveCartDetailsAsync(int cartDEtailsId)
        {
            try
            {
                ShoppingCartResponseViewModel response = new();
                ShoppingCartAPIResponse apiResponse = await _shoppingCartClient.RemoveCartAsync(cartDEtailsId.ToString(), cartDEtailsId);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ShoppingCartExceptions exception)
            {
                return ConvertShoppingCartException(exception);
            }
        }
    }
}