using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Queries;

public class GetUserRequest(string userId) : IRequest<Result<GetUserDto>>
{
    public string UserId { get; init; } = userId;
}