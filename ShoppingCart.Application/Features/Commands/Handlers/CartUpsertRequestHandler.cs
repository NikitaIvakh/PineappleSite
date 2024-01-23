using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Response;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class CartUpsertRequestHandler(ICartHeaderDbContext cartHeaderContext, ICartDetailsDbContext cartDetailsContext, IMapper mapper) : IRequestHandler<CartUpsertRequest, ShoppingCartAPIResponse>
    {
        private readonly ICartHeaderDbContext _cartHeaderContext = cartHeaderContext;
        private readonly ICartDetailsDbContext _cartDetailsContext = cartDetailsContext;
        private readonly IMapper _mapper = mapper;
        private readonly ShoppingCartAPIResponse _shoppingCartAPIResponse = new();

        public async Task<ShoppingCartAPIResponse> Handle(CartUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeaderFromDb = await _cartHeaderContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(key => key.UserId == request.CartDto.CartHeader.UserId, cancellationToken);

                if (cartHeaderFromDb is null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(request.CartDto.CartHeader);
                    await _cartHeaderContext.CartHeaders.AddAsync(cartHeader, cancellationToken);
                    await _cartHeaderContext.SaveChangesAsync(cancellationToken);

                    request.CartDto.CartDetails.First().Id = cartHeader.Id;
                    await _cartDetailsContext.CartDetails.AddAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()), cancellationToken);
                    await _cartDetailsContext.SaveChangesAsync(cancellationToken);
                }

                else
                {
                    var cartDetailsFromDb = await _cartDetailsContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(key => key.ProductId == request.CartDto.CartDetails.First().ProductId &&
                        key.CartHeaderId == cartHeaderFromDb.Id, cancellationToken);

                    if (cartDetailsFromDb is null || cartDetailsFromDb.Count == 0)
                    {
                        request.CartDto.CartDetails.First().Id = cartHeaderFromDb.Id;
                        await _cartDetailsContext.CartDetails.AddAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()), cancellationToken);
                        await _cartDetailsContext.SaveChangesAsync(cancellationToken);
                    }

                    else
                    {
                        request.CartDto.CartDetails.First().Id = cartDetailsFromDb.Id;
                        request.CartDto.CartDetails.First().Count = cartDetailsFromDb.Count;
                        request.CartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;

                        _cartDetailsContext.CartDetails.Update(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
                        await _cartDetailsContext.SaveChangesAsync(cancellationToken);
                    }
                }

                _shoppingCartAPIResponse.IsSuccess = true;
                _shoppingCartAPIResponse.Message = "Корзина успешно обновлена";
                _shoppingCartAPIResponse.Data = request.CartDto;

                return _shoppingCartAPIResponse;
            }

            catch (Exception exception)
            {
                _shoppingCartAPIResponse.IsSuccess = false;
                _shoppingCartAPIResponse.Message = exception.Message;
            }

            return _shoppingCartAPIResponse;
        }
    }
}