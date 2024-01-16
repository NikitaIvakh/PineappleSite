using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Queries
{
    public class GetUserDetailsRequest : IRequest<UserWithRolesDto>
    {
        public string Id { get; set; }
    }
}