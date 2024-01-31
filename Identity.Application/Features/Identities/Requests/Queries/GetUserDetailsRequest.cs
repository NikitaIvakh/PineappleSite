﻿using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Identities.Requests.Queries
{
    public class GetUserDetailsRequest : IRequest<Result<UserWithRolesDto>>
    {
        public string Id { get; set; }
    }
}