﻿using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class ShoppingCartUpsertRequest(CartDto cartDto) : IRequest<Result<Unit>>
{
    public CartDto CartDto { get; init; } = cartDto;
}