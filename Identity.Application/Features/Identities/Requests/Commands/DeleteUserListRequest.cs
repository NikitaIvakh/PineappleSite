﻿using Identity.Application.DTOs.Identities;
using Identity.Application.Response;
using Identity.Core.Entities.Users;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Commands
{
    public class DeleteUserListRequest : IRequest<BaseIdentityResponse<UserWithRoles>>
    {
        public DeleteUserListDto DeleteUserList { get; set; }
    }
}