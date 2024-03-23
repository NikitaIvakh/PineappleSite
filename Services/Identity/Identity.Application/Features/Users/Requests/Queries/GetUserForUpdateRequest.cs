﻿using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Queries
{
    public class GetUserForUpdateRequest : IRequest<Result<GetUserForUpdateDto>>
    {
        public string UserId { get; set; } = null!;

        public string? Password { get; set; }
    }
}