﻿using MediatR;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using Microsoft.EntityFrameworkCore;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Microsoft.Extensions.Caching.Memory;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Users.Commands.Queries;

public sealed class GetUsersRequestHandler(
    UserManager<ApplicationUser> userManager,
    IMemoryCache memoryCache) : IRequestHandler<GetUsersRequest, CollectionResult<GetUsersDto>>
{
    private const string CacheKey = "СacheUserKey";

    public async Task<CollectionResult<GetUsersDto>> Handle(GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out List<GetUsersDto>? usersWithRoles))
            {
                return new CollectionResult<GetUsersDto>
                {
                    Data = usersWithRoles,
                    Count = usersWithRoles!.Count,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage = SuccessMessage.UsersSuccessfullyGeted,
                };
            }

            var users = await userManager.Users.ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return new CollectionResult<GetUsersDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                        [ErrorMessage.ResourceManager.GetString("UsersNotFound", ErrorMessage.Culture) ?? string.Empty],
                };
            }

            var getUsersDtoList = new List<GetUsersDto>();

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);
                getUsersDtoList.Add(new GetUsersDto
                (
                    UserId: user.Id,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    UserName: user.UserName!,
                    EmailAddress: user.Email!,
                    Role: role,
                    CreatedTime: user.CreatedTime,
                    ModifiedTime: user.ModifiedTime
                ));
            }

            var getOrderedList = getUsersDtoList.OrderByDescending(key => key.CreatedTime).ToList();

            memoryCache.Remove(CacheKey);

            return new CollectionResult<GetUsersDto>
            {
                Data = getOrderedList,
                Count = getUsersDtoList.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("UsersSuccessfullyGeted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<GetUsersDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}