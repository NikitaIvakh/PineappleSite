using Carter;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.API.Endpoints;

public class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users");

        group.MapGet("/GetUsers", GetUsers).WithName(nameof(GetUsers));
    }

    private static async Task<Results<Ok<CollectionResult<GetUsersDto>>, BadRequest<string>>> GetUsers(ISender sender,
        ILogger<GetUsersDto> logger)
    {
        var request = await sender.Send(new GetUsersRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователи успешно получены");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Получить пользователей не удалось");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }
}