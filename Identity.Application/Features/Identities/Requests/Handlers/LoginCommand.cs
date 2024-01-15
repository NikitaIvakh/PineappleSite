using Identity.Application.DTOs.Identity;
using Identity.Application.Response;
using Identity.Core.Entities.Identities;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Handlers
{
    public class LoginCommand : IRequest<BaseIdentityResponse>
    {
        public AuthRequestDto AuthRequest { get; set; }
    }
}