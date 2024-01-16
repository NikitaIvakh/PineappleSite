using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Services
{
    public class UserService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper) : BaseIdentityService(localStorageService, identityClient), IUserService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;

        public async Task<IList<UserWithRolesViewModel>> GetAllUsersAsync()
        {
            var users = await _identityClient.GetAllUsersAsync();
            var usersWithRoles = _mapper.Map<IList<UserWithRolesViewModel>>(users);

            return usersWithRoles;
        }

        public async Task<UserWithRolesViewModel> GetUserAsync(string id)
        {
            var user = await _identityClient.GetUserByIdAsync(id);
            var userWithRole = _mapper.Map<UserWithRolesViewModel>(user);

            return userWithRole;
        }

        public Task<IdentityResponseViewModel> UpdateUserAsync(RegisterRequestViewModel register)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResponseViewModel> DeleteUserAsync(DeleteUserViewModel delete)
        {
            throw new NotImplementedException();
        }
    }
}