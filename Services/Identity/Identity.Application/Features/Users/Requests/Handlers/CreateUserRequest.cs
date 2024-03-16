using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class CreateUserRequest : IRequest<Result<ApplicationUser>>
    {
        public CreateUserDto CreateUser { get; set; }
    }
}