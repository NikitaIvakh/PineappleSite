using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.Entities.Cart;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.ResultCart;
using ShoppingCart.Domain.DTOs;
using Serilog;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Application.Features.Commands.Queries
{
    public class GetShoppingCartRequestHandler(IBaseRepository<CartHeader> cartHeader, IBaseRepository<CartDetails> cartDetails, IMapper mapper, IProductService productService, ICouponService couponService, ILogger logger)
        : IRequestHandler<GetShoppingCartRequest, Result<CartDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeader;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetails;
        private readonly IMapper _mapper = mapper;
        private readonly IProductService _productService = productService;
        private readonly ICouponService _couponService = couponService;
        private readonly ILogger _logger = logger.ForContext<GetShoppingCartRequestHandler>();

        public async Task<Result<CartDto>> Handle(GetShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeader = await _cartHeaderRepository.GetAll().Select(key => new CartHeaderDto
                {
                    Id = key.Id,
                    UserId = key.UserId,
                    CouponCode = key.CouponCode,
                    Discount = key.Discount,
                    CartTotal = key.CartTotal,
                }).FirstOrDefaultAsync(key => key.UserId == request.UserId, cancellationToken);

                if (cartHeader is null)
                {
                    return new Result<CartDto>
                    {
                        Data = new CartDto
                        {
                            CartHeader = new CartHeaderDto(),
                            CartDetails = new CollectionResult<CartDetailsDto>(),
                        },

                        SuccessMessage = "Ваша корзина пустая, добавте товары",
                    };
                }

                else
                {
                    var cartDetails = _cartDetailsRepository.GetAll().Select(key => new CartDetailsDto
                    {
                        Id = key.Id,
                        Count = key.Count,
                        Product = key.Product,
                        ProductId = key.ProductId,
                        CartHeaderId = key.CartHeaderId,
                        CartHeader = _mapper.Map<CartHeaderDto>(key.CartHeader),
                    }).Where(key => key.CartHeaderId == cartHeader.Id).ToList();

                    CartDto cartDto = new()
                    {
                        CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                        CartDetails = new CollectionResult<CartDetailsDto>
                        {
                            Data = cartDetails,
                            Count = cartDetails.Count,
                            SuccessMessage = "Ваши товары в корзине",
                        },
                    };

                    CollectionResult<ProductDto> productDtos = await _productService.GetProductsAsync();

                    if (productDtos is not null)
                    {
                        foreach (var item in cartDto.CartDetails.Data)
                        {
                            item.Product = productDtos.Data.FirstOrDefault(key => key.Id == item.ProductId);
                            cartDto.CartHeader.CartTotal += (item.Count * item.Product?.Price ?? 0);
                        }
                    }

                    if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                    {
                        var coupon = await _couponService.GetCouponAsync(cartDto.CartHeader.CouponCode);

                        if (coupon is not null && cartDto.CartHeader.CartTotal > coupon.Data.MinAmount)
                        {
                            cartDto.CartHeader.CartTotal -= coupon.Data.DiscountAmount;
                            cartDto.CartHeader.Discount = coupon.Data.DiscountAmount;
                        }
                    }

                    return new Result<CartDto>
                    {
                        Data = cartDto,
                        SuccessMessage = "Купону успешно применен",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<CartDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}