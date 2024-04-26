using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Queries;

public sealed class GetShoppingCartRequest(string userId) : IRequest<Result<CartDto>>
{
    public string UserId { get; init; } = userId;
}