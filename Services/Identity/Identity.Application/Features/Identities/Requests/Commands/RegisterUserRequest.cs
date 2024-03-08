using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class RegisterUserRequest : IRequest<Result<RegisterResponseDto>>
    {
        public RegisterRequestDto RegisterRequest { get; set; }
    }
}