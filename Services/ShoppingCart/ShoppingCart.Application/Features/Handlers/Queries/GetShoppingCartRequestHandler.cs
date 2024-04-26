using MediatR;
using AutoMapper;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Results;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Application.Features.Requests.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace ShoppingCart.Application.Features.Handlers.Queries;

public sealed class GetShoppingCartRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    IProductService productService,
    ICouponService couponService,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<GetShoppingCartRequest, Result<CartDto>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<CartDto>> Handle(GetShoppingCartRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out CartDto? cartDto))
            {
                return new Result<CartDto>
                {
                    Data = cartDto,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("YourShoppingCart", SuccessMessage.Culture),
                };
            }

            var cartHeader = cartHeaderRepository.GetAll()
                .Select(key => new CartHeaderDto
                {
                    CartHeaderId = key.CartHeaderId,
                    UserId = key.UserId,
                    CouponCode = key.CouponCode,
                    Discount = key.Discount,
                    CartTotal = key.CartTotal,
                }).FirstOrDefault(key => key.UserId == request.UserId);

            if (cartHeader is null)
            {
                return new Result<CartDto>
                {
                    Data = new CartDto
                    (
                        cartHeader: new CartHeaderDto(),
                        cartDetails: []
                    ),

                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ShoppingCartIsEmpty", SuccessMessage.Culture),
                };
            }

            var cartDetails = cartDetailsRepository.GetAll()
                .Where(key =>
                    key.CartHeader!.UserId == cartHeader.UserId &&
                    key.CartHeaderId == cartHeader.CartHeaderId)
                .Select(key =>
                    new CartDetailsDto
                    {
                        CartDetailsId = key.CartDetailsId,
                        CartHeader = mapper.Map<CartHeaderDto>(key.CartHeader),
                        CartHeaderId = key.CartHeaderId,
                        Product = key.Product,
                        ProductId = key.ProductId,
                        Count = key.Count,
                    }).OrderByDescending(key => key.CartDetailsId).ToList();

            if (cartDetails.Count == 0)
            {
                return new Result<CartDto>()
                {
                    Data = new CartDto
                    (
                        cartHeader: new CartHeaderDto(),
                        cartDetails: []
                    ),

                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ShoppingCartIsEmpty", SuccessMessage.Culture),
                };
            }

            var getCartDto = new CartDto(cartHeader, cartDetails);
            var products = await productService.GetProductsAsync();

            foreach (var product in getCartDto.CartDetails)
            {
                product.Product = products.Data?.FirstOrDefault(key => key.Id == product.ProductId);
                getCartDto.CartHeader.CartTotal += (product.Count * product.Product?.Price ?? 0);
            }

            if (!string.IsNullOrEmpty(getCartDto?.CartHeader.CouponCode))
            {
                var coupon = await couponService.GetCouponAsync(getCartDto.CartHeader.CouponCode);

                if (coupon.Data is null)
                {
                    return new Result<CartDto>
                    {
                        ErrorMessage = ErrorMessages.ResourceManager.GetString("CouponNotFound", ErrorMessages.Culture),
                        ValidationErrors =
                        [
                            ErrorMessages.ResourceManager.GetString("CouponNotFound", ErrorMessages.Culture) ??
                            string.Empty
                        ]
                    };
                }

                if (getCartDto.CartHeader.CartTotal > coupon.Data.MinAmount)
                {
                    getCartDto.CartHeader.CartTotal -= coupon.Data.DiscountAmount;
                    getCartDto.CartHeader.Discount = coupon.Data.DiscountAmount;
                }
            }

            memoryCache.Remove(CacheKey);

            return new Result<CartDto>
            {
                Data = getCartDto,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.ResourceManager.GetString("YourShoppingCart", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<CartDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}