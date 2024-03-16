using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class DeleteUserRequest : IRequest<Result<DeleteUserDto>>
    {
        public DeleteUserDto DeleteUser { get; set; }
    }
}