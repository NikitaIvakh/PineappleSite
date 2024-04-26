using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class DeleteCartProductByUserRequest(DeleteProductDto deleteProductDto, string userId) : IRequest<Result<Unit>>
{
    public DeleteProductDto DeleteProductDto { get; set; } = deleteProductDto;
    
    public string UserId { get; set; } = userId;
}