using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class UpdateUserRequest : IRequest<Result<Unit>>
    {
        public UpdateUserDto UpdateUser { get; set; } = null!;
    }
}