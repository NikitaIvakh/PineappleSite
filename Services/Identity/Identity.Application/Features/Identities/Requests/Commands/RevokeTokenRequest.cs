using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands;

public sealed class RevokeTokenRequest(string userName) : IRequest<Result<Unit>>
{
    public string UserName { get; init; } = userName;
}