using AutoMapper;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Application.Resources;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserDetailsRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMapper mapper) : IRequestHandler<GetUserDetailsRequest, Result<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserDetailsRequestHandler>();
        private readonly IMapper _mapper = mapper;

        public async Task<Result<UserWithRolesDto>> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(key => key.Id == request.Id, cancellationToken);

                if (user is null)
                {
                    return new Result<UserWithRolesDto>
                    {
                        ErrorMessage = ErrorMessage.UserNotFound,
                        ErrorCode = (int)ErrorCodes.UserNotFound,
                    };
                }

                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userWithRolesDto = new UserWithRoles
                    {
                        User = user,
                        Roles = roles.ToList()
                    };

                    return new Result<UserWithRolesDto>
                    {
                        Data = _mapper.Map<UserWithRolesDto>(userWithRolesDto),

                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<UserWithRolesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}