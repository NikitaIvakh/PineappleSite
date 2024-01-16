using Identity.Application.DTOs.Authentications;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LoginUserRequest : IRequest<BaseIdentityResponse<AuthResponseDto>>
    {
        public AuthRequestDto AuthRequest { get; set; }
    }
}