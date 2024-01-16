using Identity.Application.DTOs.Authentications;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class RegisterUserRequest : IRequest<BaseIdentityResponse<RegisterResponseDto>>
    {
        public RegisterRequestDto RegisterRequest { get; set; }
    }
}