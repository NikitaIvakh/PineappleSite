using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers;

public sealed class DeleteUserRequest(DeleteUserDto deleteUser) : IRequest<Result<Unit>>
{
    public DeleteUserDto DeleteUser { get; init; } = deleteUser;
}