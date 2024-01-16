using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class UpdateUserRequest : IRequest<BaseIdentityResponse<RegisterResponseDto>>
    {
        public UpdateUserDto UpdateUser { get; set; }
    }
}