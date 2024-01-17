using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class UpdateUserProfileRequest : IRequest<BaseIdentityResponse<ApplicationUser>>
    {
        public UpdateUserProfileDto UpdateUserProfile { get; set; }
    }
}