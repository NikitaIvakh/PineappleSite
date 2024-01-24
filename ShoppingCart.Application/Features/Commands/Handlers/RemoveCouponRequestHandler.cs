﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Response;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class RemoveCouponRequestHandler(ICartHeaderDbContext cartHeaderContext, IMapper mapper) : IRequestHandler<RemoveCouponRequest, ShoppingCartAPIResponse>
    {
        private readonly ICartHeaderDbContext _cartHeaderContext = cartHeaderContext;
        private readonly IMapper _mapper = mapper;
        private readonly ShoppingCartAPIResponse _response = new();

        public async Task<ShoppingCartAPIResponse> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CartHeader cartHeaderFromDb = await _cartHeaderContext.CartHeaders.FirstAsync(key => key.UserId == request.CartDto.CartHeader.UserId, cancellationToken);
                cartHeaderFromDb.CouponCode = string.Empty;
                _cartHeaderContext.CartHeaders.Update(cartHeaderFromDb);
                await _cartHeaderContext.SaveChangesAsync(cancellationToken);

                _response.IsSuccess = true;
                _response.Message = "Купон успешно удален";
                _response.Id = cartHeaderFromDb.Id;
                _response.Data = cartHeaderFromDb;

                return _response;
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