using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Queries;

public sealed class GetUserForUpdateRequest(string userId, string password) : IRequest<Result<GetUserForUpdateDto>>
{
    public string UserId { get; init; } = userId;

    public string Password { get; init; } = password;
}