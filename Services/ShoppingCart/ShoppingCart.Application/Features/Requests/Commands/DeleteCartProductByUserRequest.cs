using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public sealed class DeleteCartProductByUserRequest(DeleteProductByUserDto deleteProductByUserDto)
    : IRequest<Result<Unit>>
{
    public DeleteProductByUserDto DeleteProductByUserDto { get; set; } = deleteProductByUserDto;
}