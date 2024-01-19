using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LogoutUserRequest : IRequest<BaseIdentityResponse<bool>>
    {

    }
}