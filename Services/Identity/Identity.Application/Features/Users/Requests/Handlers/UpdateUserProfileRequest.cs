using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class UpdateUserProfileRequest : IRequest<Result<UserWithRolesDto>>
    {
        public UpdateUserProfileDto UpdateUserProfile { get; set; }
    }
}