using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers;

public sealed class UpdateUserRequest(UpdateUserDto updateUser) : IRequest<Result<Unit>>
{
    public UpdateUserDto UpdateUser { get; init; } = updateUser;
}