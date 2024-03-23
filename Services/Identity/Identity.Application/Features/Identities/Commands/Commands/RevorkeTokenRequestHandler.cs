using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class RevorkeTokenRequestHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : IRequestHandler<RevorkeTokenRequest, Result<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<Unit>> Handle(RevorkeTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user is null)
                {
                    return new Result<Unit>
                    {
                        Data = Unit.Value,
                        ErrorCode = (int)ErrorCodes.UserNotFound,
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ValidationErrors = [ErrorMessage.UserNotFound]
                    };
                }

                else
                {
                    user.RefreshToken = null;
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new Result<Unit>
                    {
                        Data = Unit.Value,
                        SuccessCode = (int)SuccessCode.Deleted,
                        SuccessMessage = SuccessMessage.TokenSuccessfullyDeleted,
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