using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class UpdateUserProfileRequest : IRequest<Result<GetUserForUpdateDto>>
    {
        public UpdateUserProfileDto UpdateUserProfile { get; set; } = null!;
    }
}