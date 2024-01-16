using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using MediatR;
using static Identity.Application.Utilities.StaticDetails;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class UpdateUserRequest : IRequest<BaseIdentityResponse<RegisterResponseDto>>
    {
        public UpdateUserDto UpdateUser { get; set; }

        public UserRoles UserRoles { get; set; }
    }
}