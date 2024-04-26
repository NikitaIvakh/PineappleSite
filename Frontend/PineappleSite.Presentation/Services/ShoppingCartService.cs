using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Services;

public sealed class ShoppingCartService(
    ILocalStorageService localStorageService,
    IShoppingCartClient shoppingCartClient,
    IMapper mapper,
    IHttpContextAccessor contextAccessor)
    : BaseShoppingCartService(localStorageService, shoppingCartClient, contextAccessor), IShoppingCartService
{
    private readonly IShoppingCartClient _shoppingCartClient = shoppingCartClient;

    public async Task<CartResult<CartViewModel>> GetCartAsync(string userId)
    {
        AddBearerToken();
        try
        {
            var cartDto = await _shoppingCartClient.GetShoppingCartAsync(userId);

            CartResult<CartViewModel> result = new()
            {
                StatusCode = cartDto.StatusCode,
                SuccessMessage = cartDto.SuccessMessage,
                Data = mapper.Map<CartViewModel>(cartDto.Data),
            };

            if (result.IsSuccess)
            {
                return result;
            }

            return new CartResult<CartViewModel>
            {
                StatusCode = cartDto.StatusCode,
                ErrorMessage = cartDto.ErrorMessage,
                ValidationErrors = string.Join(", ", cartDto.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult<CartViewModel>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult> CartUpsertAsync(CartViewModel cartViewModel)
    {
        AddBearerToken();
        try
        {
            var cartDto = mapper.Map<CartDto>(cartViewModel);
            var apiResponse = await _shoppingCartClient.ShoppingCartUpsertAsync(cartDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult<CartHeaderViewModel>> ApplyCouponAsync(CartViewModel cartViewModel)
    {
        AddBearerToken();
        try
        {
            var cartDto = mapper.Map<CartDto>(cartViewModel);
            var apiResponse = await _shoppingCartClient.ApplyCouponAsync(cartDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult<CartHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<CartHeaderViewModel>(apiResponse.Data),
                };
            }

            return new CartResult<CartHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult<CartHeaderViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult<CartHeaderViewModel>> RemoveCouponAsync(CartViewModel cartViewModel)
    {
        AddBearerToken();
        try
        {
            var cartDto = mapper.Map<CartDto>(cartViewModel);
            var apiResponse = await _shoppingCartClient.RemoveCouponAsync(cartDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult<CartHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<CartHeaderViewModel>(apiResponse.Data),
                };
            }

            return new CartResult<CartHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult<CartHeaderViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult> RemoveCartDetailsAsync(DeleteProductViewModel deleteProductViewModel)
    {
        AddBearerToken();
        try
        {
            var deleteProductDto = mapper.Map<DeleteProductDto>(deleteProductViewModel);
            var apiResponse = await _shoppingCartClient.RemoveProductAsync(deleteProductDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<CartResult> RemoveCartDetailsByUserAsync(
        DeleteProductByUserViewModel deleteProductByUserViewModel)
    {
        AddBearerToken();
        try
        {
            var deleteProductByUserDto = mapper.Map<DeleteProductByUserDto>(deleteProductByUserViewModel);
            var apiResponse = await _shoppingCartClient.RemoveProductByUserAsync(deleteProductByUserDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult()
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartResult()
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartCollectionResult<bool>> RemoveCartDetailsAsync(
        DeleteProductsViewModel deleteProductListViewModel)
    {
        AddBearerToken();
        try
        {
            var deleteProductsDto = mapper.Map<DeleteProductsDto>(deleteProductListViewModel);
            var apiResponse = await _shoppingCartClient.RemoveProductsAsync(deleteProductsDto);

            if (apiResponse.IsSuccess)
            {
                return new CartCollectionResult<bool>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartCollectionResult<bool>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartCollectionResult<bool>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult> RemoveCouponByCode(DeleteCouponByCodeViewModel deleteCouponByCodeViewModel)
    {
        AddBearerToken();
        try
        {
            var deleteCouponDto = mapper.Map<DeleteCouponDto>(deleteCouponByCodeViewModel);
            var apiResponse = await shoppingCartClient.RemoveCouponCodeAsync(deleteCouponDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult()
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartResult()
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors)
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<CartResult<bool>> RabbitMqShoppingCartAsync(CartViewModel cartViewModel)
    {
        AddBearerToken();
        try
        {
            var cartDto = mapper.Map<CartDto>(cartViewModel);
            var apiResponse = await _shoppingCartClient.SendMessageAsync(cartDto);

            if (apiResponse.IsSuccess)
            {
                return new CartResult<bool>
                {
                    Data = apiResponse.Data,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CartResult<bool>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ShoppingCartExceptions<string> exceptions)
        {
            return new CartResult<bool>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }
}