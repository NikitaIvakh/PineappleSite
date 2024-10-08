﻿using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;

namespace Identity.Application.Features.Users.Requests.Queries;

public class GetUsersRequest : IRequest<CollectionResult<GetUsersDto>>
{
}