using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Queries
{
    public class GetUserListRequest : IRequest<IEnumerable<UserWithRolesDto>>
    {

    }
}