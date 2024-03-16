using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class RevokeAllTokensRequest : IRequest<Result<Unit>>
    {

    }
}