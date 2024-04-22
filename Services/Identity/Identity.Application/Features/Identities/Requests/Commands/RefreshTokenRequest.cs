using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Application.Features.Identities.Requests.Commands;

public sealed class RefreshTokenRequest(TokenModelDto tokenModelDto) : IRequest<Result<ObjectResult>>
{
    public TokenModelDto TokenModelDto { get; init; } = tokenModelDto;
}