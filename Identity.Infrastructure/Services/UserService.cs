using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using Identity.Core.Interfaces;
using Identity.Core.Response;
using Identity.Infrastructure.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRegisterRequestDtoValidator registerValidator) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IRegisterRequestDtoValidator _registerValidator = registerValidator;

        public async Task<IEnumerable<UserWithRoles>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<UserWithRoles>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRoles { User = user, Roles = roles });
            }

            return usersWithRoles;
        }

        public async Task<UserWithRoles> GetByIdAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(key => key.Id == id) ??
                throw new Exception("Такого пользователя нет");

            var roles = await _userManager.GetRolesAsync(user);
            var userWithRolesDto = new UserWithRoles
            {
                User = user,
                Roles = roles.ToList()
            };

            return userWithRolesDto;
        }

        public Task<BaseIdentityResponse<ApplicationUser>> CreateUserAsync(RegisterRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<BaseIdentityResponse<ApplicationUser>> UpdateUserAsync(RegisterRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<BaseIdentityResponse<ApplicationUser>> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}