using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Application.Utilities;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.ResultIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Identity.Application.Resources;
using Identity.Domain.Enum;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserListRequestHandler(UserManager<ApplicationUser> userManager, ILogger logger, IMapper mapper) : IRequestHandler<GetUserListRequest, CollectionResult<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger _logger = logger.ForContext<GetUserListRequest>();
        private readonly IMapper _mapper = mapper;

        public async Task<CollectionResult<UserWithRolesDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var usersWithRoles = new List<UserWithRolesDto>();
                var currentUser = await _userManager.FindByIdAsync(request.UserId);

                if (currentUser is not null)
                {
                    var roles = await _userManager.GetRolesAsync(currentUser);

                    if (roles.Contains(StaticDetails.AdministratorRole))
                    {
                        var users = await _userManager.Users.ToListAsync(cancellationToken);

                        foreach (var user in users)
                        {
                            var role = await _userManager.GetRolesAsync(user);
                            usersWithRoles.Add(new UserWithRolesDto { User = user, Roles = role });
                        }
                    }

                    else
                    {
                        var userWithRoles = new UserWithRolesDto
                        {
                            User = currentUser,
                            Roles = roles.ToList()
                        };

                        usersWithRoles.Add(userWithRoles);
                    }
                }

                return new CollectionResult<UserWithRolesDto>
                {
                    Data = usersWithRoles,
                    Count = usersWithRoles.Count
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new CollectionResult<UserWithRolesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }
    }
}