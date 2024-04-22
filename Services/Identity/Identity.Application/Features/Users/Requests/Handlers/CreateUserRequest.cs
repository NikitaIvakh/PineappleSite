using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers;

public sealed class CreateUserRequest(CreateUserDto createUser) : IRequest<Result<string>>
{
    public CreateUserDto CreateUser { get; init; } = createUser;
}