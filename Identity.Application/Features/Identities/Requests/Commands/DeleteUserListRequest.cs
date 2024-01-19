using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class DeleteUserListRequest : IRequest<BaseIdentityResponse<DeleteUserListDto>>
    {
        public DeleteUserListDto DeleteUserList { get; set; }
    }
}