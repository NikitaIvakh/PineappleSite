using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Exceptions;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Response;
using ShoppingCart.Application.Services.IServices;

namespace ShoppingCart.Application.Features.Commands.Queries
{
    public class GetShoppingCartRequestHandler(ICartHeaderDbContext cartContext, ICartDetailsDbContext detailsContext, IMapper mapper,
        IProductService productService, ICouponService couponService) : IRequestHandler<GetShoppingCartRequest, ShoppingCartAPIResponse>
    {
        private readonly ICartHeaderDbContext _cartContext = cartContext;
        private readonly ICartDetailsDbContext _detailsContext = detailsContext;
        private readonly IMapper _mapper = mapper;
        private readonly IProductService _productService = productService;
        private readonly ICouponService _couponService = couponService;
        private readonly ShoppingCartAPIResponse _shoppingCartAPIResponse = new();

        public async Task<ShoppingCartAPIResponse> Handle(GetShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeader = await _cartContext.CartHeaders.FirstOrDefaultAsync(key => key.UserId == request.UserId, cancellationToken) ??
                    throw new NotFoundException($"Корзины пользователя:", request.UserId);

                if (cartHeader is null)
                {
                    _shoppingCartAPIResponse.IsSuccess = true;
                    _shoppingCartAPIResponse.Data = new CartDto();

                    return _shoppingCartAPIResponse;
                }

                var cartDetails = await _detailsContext.CartDetails.Where(key => key.CartHeaderId == cartHeader.Id).ToListAsync(cancellationToken);

                CartDto cartDto = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                    CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails),
                };

                IEnumerable<ProductDto> productDtos = await _productService.GetProductsAsync();

                foreach (var item in cartDto.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(key => key.Id == item.ProductId);
                    cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCouponAsync(cartDto.CartHeader.CouponCode);

                    if (coupon is not null && cartDto.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cartDto.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _shoppingCartAPIResponse.IsSuccess = true;
                _shoppingCartAPIResponse.Message = "Купон успешно применен";
                _shoppingCartAPIResponse.Data = cartDto;

                return _shoppingCartAPIResponse;
            }

            catch (Exception ex)
            {
                _shoppingCartAPIResponse.IsSuccess = false;
                _shoppingCartAPIResponse.Message = ex.Message;
            }

            _shoppingCartAPIResponse.IsSuccess = true;
            return _shoppingCartAPIResponse;
        }
    }
}