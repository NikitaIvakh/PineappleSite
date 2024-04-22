using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers;

public sealed class DeleteUsersRequest(DeleteUsersDto deleteUsers) : IRequest<CollectionResult<Unit>>
{
    public DeleteUsersDto DeleteUsers { get; init; } = deleteUsers;
}