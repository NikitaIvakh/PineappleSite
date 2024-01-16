using Identity.Application.DTOs.Identities;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Queries
{
    public class GetUserDetailsRequest : IRequest<UserWithRolesDto>
    {
        public string Id { get; set; }
    }
}