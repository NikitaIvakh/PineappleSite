﻿using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Handlers;

public sealed class UpdateUserProfileRequest : IRequest<Result<Unit>>
{
    public UpdateUserProfileDto UpdateUserProfile { get; set; } = null!;
}