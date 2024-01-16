using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
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

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
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