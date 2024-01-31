using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class DeleteUserListRequest : IRequest<CollectionResult<DeleteUserListDto>>
    {
        public DeleteUserListDto DeleteUserList { get; set; }
    }
}