using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands;

public sealed class RegisterUserRequest(RegisterRequestDto registerRequest) : IRequest<Result<string>>
{
    public RegisterRequestDto RegisterRequest { get; init; } = registerRequest;
}