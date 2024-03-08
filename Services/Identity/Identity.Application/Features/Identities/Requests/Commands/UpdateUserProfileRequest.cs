using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class UpdateUserProfileRequest : IRequest<Result<UserWithRolesDto>>
    {
        public UpdateUserProfileDto UpdateUserProfile { get; set; }
    }
}