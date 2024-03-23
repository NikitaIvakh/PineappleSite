﻿using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RevokeAllTokensRequestHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : IRequestHandler<RevokeAllTokensRequest, Result<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<Unit>> Handle(RevokeAllTokensRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userManager.Users.ToListAsync(cancellationToken);

                if (users is null || users.Count == 0)
                {
                    return new Result<Unit>
                    {
                        Data = Unit.Value,
                        ErrorCode = (int)ErrorCodes.UsersNotFound,
                        ErrorMessage = ErrorMessage.UsersNotFound,
                        ValidationErrors = [ErrorMessage.UsersNotFound]
                    };
                }

                else
                {
                    foreach (var user in users)
                    {
                        user.RefreshToken = null;
                        await _userManager.UpdateAsync(user);
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return new Result<Unit>
                    {
                        Data = Unit.Value,
                        SuccessCode = (int)SuccessCode.Deleted,
                        SuccessMessage = SuccessMessage.TokensSuccessfullyDeleted,
                    };
                }
            }

            catch
            {
                return new Result<Unit>
                {
                    Data = Unit.Value,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}