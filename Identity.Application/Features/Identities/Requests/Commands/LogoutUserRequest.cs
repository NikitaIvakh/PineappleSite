using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class LogoutUserRequest : IRequest<Result<bool>>
    {
        public bool LogoutUser { get; set; }
    }
}