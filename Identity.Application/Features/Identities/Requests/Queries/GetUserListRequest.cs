using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Queries
{
    public class GetUserListRequest : IRequest<CollectionResult<UserWithRolesDto>>
    {
        public string UserId { get; set; }
    }
}