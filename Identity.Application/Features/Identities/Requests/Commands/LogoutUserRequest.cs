using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LogoutUserRequest : IRequest<BaseIdentityResponse<Unit>>
    {
        public string ReturnUrl { get; set; }
    }
}