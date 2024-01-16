using AutoMapper;
using Identity.Application.DTOs.Identities;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Features.Identities.Commands.Queries
{
    public class GetUserDetailsRequestHandler(UserManager<ApplicationUser> userManager, IMapper mapper) : IRequestHandler<GetUserDetailsRequest, UserWithRolesDto>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<UserWithRolesDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(key => key.Id == request.Id) ??
                throw new Exception("Такого пользователя нет");

            var roles = await _userManager.GetRolesAsync(user);
            var userWithRolesDto = new UserWithRoles
            {
                User = user,
                Roles = roles.ToList()
            };

            return _mapper.Map<UserWithRolesDto>(userWithRolesDto);
        }
    }
}