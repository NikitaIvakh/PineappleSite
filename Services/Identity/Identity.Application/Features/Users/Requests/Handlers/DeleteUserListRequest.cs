using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers
{
    public class DeleteUserListRequest : IRequest<Result<bool>>
    {
        public DeleteUserListDto DeleteUserList { get; set; } = null!;
    }
}