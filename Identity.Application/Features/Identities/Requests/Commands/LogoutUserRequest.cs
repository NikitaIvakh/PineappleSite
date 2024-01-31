using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LogoutUserRequest : IRequest<Result<LogoutUserDto>>
    {
        public LogoutUserDto LogoutUser { get; set; }
    }
}