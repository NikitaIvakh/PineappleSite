using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands;

public sealed class LoginUserRequest(AuthRequestDto authRequest) : IRequest<Result<string>>
{
    public AuthRequestDto AuthRequest { get; init; } = authRequest;
}