using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.Application.DTOs.Identities;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Application.Utilities;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserListRequestHandler(UserManager<ApplicationUser> userManager, IMapper mapper) : IRequestHandler<GetUserListRequest, IEnumerable<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserWithRolesDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var usersWithRoles = new List<UserWithRolesDto>();
            var currentUser = await _userManager.FindByIdAsync(request.UserId);

            if (currentUser != null)
            {
                var roles = await _userManager.GetRolesAsync(currentUser);

                if (roles.Contains(StaticDetails.AdministratorRole))
                {
                    var users = await _userManager.Users.ProjectTo<ApplicationUser>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

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

            return usersWithRoles;
        }
    }
}