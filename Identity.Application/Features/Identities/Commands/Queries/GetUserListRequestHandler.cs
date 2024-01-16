using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.Application.DTOs.Identities;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Core.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserListRequestHandler(UserManager<ApplicationUser> userManager, IMapper mapper) : IRequestHandler<GetUserListRequest, IEnumerable<UserWithRolesDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserWithRolesDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ProjectTo<ApplicationUser>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            var usersWithRoles = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesDto { User = user, Roles = roles });
            }

            return _mapper.Map<IEnumerable<UserWithRolesDto>>(usersWithRoles);
        }
    }
}