﻿using MediatR;
using AutoMapper;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Results;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Application.Features.Requests.Queries;

namespace ShoppingCart.Application.Features.Handlers.Queries
{
    public class GetShoppingCartRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository,
        IProductService productService, ICouponService couponService, IMapper mapper) : IRequestHandler<GetShoppingCartRequest, Result<CartDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IProductService _productService = productService;
        private readonly ICouponService _couponService = couponService;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CartDto>> Handle(GetShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeaderDto? cartHeader = _cartHeaderRepository.GetAll().Select(key => new CartHeaderDto
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
                        {
                            CartHeader = new CartHeaderDto(),
                            CartDetails = new List<CartDetailsDto>(),
                        },

                        SuccessMessage = "Ваша корзина пуста! Добавьте товары"
                    };
                }

                else
                {
                    List<CartDetailsDto> cartDetails = _cartDetailsRepository.GetAll().Select(key => new CartDetailsDto
                    {
                        CartDetailsId = key.CartDetailsId,
                        CartHeader = _mapper.Map<CartHeaderDto>(key.CartHeader),
                        CartHeaderId = key.CartHeaderId,
                        Product = key.Product,
                        ProductId = key.ProductId,
                        Count = key.Count,
                    }).Where(key => key.CartHeaderId == cartHeader.CartHeaderId).ToList();

                    CartDto cartDto = new()
                    {
                        CartHeader = cartHeader,
                        CartDetails = cartDetails,
                    };

                    CollectionResult<ProductDto> products = await _productService.GetProductListAsync();

                    foreach (var product in cartDto.CartDetails)
                    {
                        product.Product = products.Data.FirstOrDefault(key => key.Id == product.ProductId);
                        cartDto.CartHeader.CartTotal += (product.Count *= product.Product?.Price ?? 0);
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
                        SuccessMessage = "Ваша корзина с товарами",
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<CartDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [exception.Message]
                };
            }
        }
    }
}