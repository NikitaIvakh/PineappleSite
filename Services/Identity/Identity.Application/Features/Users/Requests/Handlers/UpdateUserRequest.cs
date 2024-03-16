using Identity.Domain.DTOs.Authentications;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class UpdateUserRequest : IRequest<Result<RegisterResponseDto>>
    {
        public UpdateUserDto UpdateUser { get; set; }
    }
}