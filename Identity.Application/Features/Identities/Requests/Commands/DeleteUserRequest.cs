using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class DeleteUserRequest : IRequest<BaseIdentityResponse<DeleteUserDto>>
    {
        public DeleteUserDto DeleteUser { get; set; }
    }
}