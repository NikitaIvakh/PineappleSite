using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LogoutUserRequest : IRequest<BaseIdentityResponse<LogoutUserDto>>
    {
        public LogoutUserDto LogoutUser { get; set; }
    }
}