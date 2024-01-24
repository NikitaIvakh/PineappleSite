using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Response;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class RemoveCartRequestHandlerc(ICartHeaderDbContext cartHeaderContext, ICartDetailsDbContext cartDetailsContext, IMapper mapper) : IRequestHandler<RemoveCartRequest, ShoppingCartAPIResponse>
    {
        private readonly ICartHeaderDbContext _cartHeaderContext = cartHeaderContext;
        private readonly ICartDetailsDbContext _cartDetailsContext = cartDetailsContext;
        private readonly IMapper _mapper = mapper;
        private readonly ShoppingCartAPIResponse _response = new();

        public async Task<ShoppingCartAPIResponse> Handle(RemoveCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartDetails cartDetails = await _cartDetailsContext.CartDetails.FirstOrDefaultAsync(key => key.Id == request.CartDetailsId, cancellationToken);
                int totalCartRemoveItems = _cartDetailsContext.CartDetails.Where(key => key.CartHeaderId == cartDetails.CartHeaderId).Count();
                _cartDetailsContext.CartDetails.Remove(cartDetails);

                if (cartDetails.CartHeaderId == 1)
                {
                    CartHeader cartHeader = await _cartHeaderContext.CartHeaders.FirstOrDefaultAsync(key => key.Id == cartDetails.CartHeaderId);
                    _cartHeaderContext.CartHeaders.Remove(cartHeader);
                }

                await _cartDetailsContext.SaveChangesAsync(cancellationToken);
                await _cartHeaderContext.SaveChangesAsync(cancellationToken);
            }

            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Message = exception.Message;
            }

            return _response;
        }
    }
}