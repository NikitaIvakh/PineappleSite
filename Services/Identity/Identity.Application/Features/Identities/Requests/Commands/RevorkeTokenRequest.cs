using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class RevorkeTokenRequest : IRequest<Result<Unit>>
    {
        public string UserName { get; set; } = null!;
    }
}