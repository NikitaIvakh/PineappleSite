using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LoginUserRequest : IRequest<Result<AuthResponseDto>>
    {
        public AuthRequestDto AuthRequest { get; set; }
    }
}