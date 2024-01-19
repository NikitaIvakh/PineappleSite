using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Services
{
    public class UserService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper, IHttpContextAccessor httpContextAccessor) : BaseIdentityService(localStorageService, identityClient), IUserService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IList<UserWithRolesViewModel>> GetAllUsersAsync(string userId)
        {
            AddBearerToken();
            var user = await _identityClient.GetUserByIdAsync(userId);
            var users = await _identityClient.GetAllUsersAsync(user.User.Id);
            var usersWithRoles = _mapper.Map<IList<UserWithRolesViewModel>>(users);

            return usersWithRoles;
        }

        public async Task<UserWithRolesViewModel> GetUserAsync(string id)
        {
            AddBearerToken();
            var user = await _identityClient.GetUserByIdAsync(id);
            var userWithRole = _mapper.Map<UserWithRolesViewModel>(user);

            return userWithRole;
        }

        public async Task<IdentityResponseViewModel> CreateUserAsync(RegisterRequestViewModel register)
        {
            AddBearerToken();
            try
            {
                IdentityResponseViewModel response = new();
                RegisterRequestDto registerRequestDto = _mapper.Map<RegisterRequestDto>(register);
                RegisterResponseDtoBaseIdentityResponse apiResoponse = await _identityClient.RegisterAsync(registerRequestDto);

                if (apiResoponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Data = apiResoponse.Data;
                    response.Message = apiResoponse.Message;
                }

                else
                {
                    foreach (string error in apiResoponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (IdentityExceptions exception)
            {
                return ConvertIdentityExceptions(exception);
            }
        }

        public async Task<IdentityResponseViewModel> UpdateUserAsync(UpdateUserViewModel updateUserView)
        {
            AddBearerToken();
            try
            {
                IdentityResponseViewModel response = new();
                UpdateUserDto updateUserDto = _mapper.Map<UpdateUserDto>(updateUserView);
                RegisterResponseDtoBaseIdentityResponse apiResponse = await _identityClient.UserPUTAsync(updateUserDto.Id, updateUserDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Data = apiResponse.Data;
                    response.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (IdentityExceptions exception)
            {
                return ConvertIdentityExceptions(exception);
            }
        }

        public async Task<IdentityResponseViewModel> DeleteUserAsync(DeleteUserViewModel delete)
        {
            AddBearerToken();
            try
            {
                IdentityResponseViewModel response = new();
                DeleteUserDto deleteUserDto = _mapper.Map<DeleteUserDto>(delete);
                DeleteUserDtoBaseIdentityResponse apiResponse = await _identityClient.UserDELETEAsync(deleteUserDto.Id, deleteUserDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Data = apiResponse.Data;
                    response.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (IdentityExceptions exception)
            {
                return ConvertIdentityExceptions(exception);
            }
        }

        public async Task<IdentityResponseViewModel> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfile)
        {
            AddBearerToken();
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(key => key.Type == "uid")?.Value;
                IdentityResponseViewModel response = new();

                FileParameter avatarFileParameter = null;
                if (updateUserProfile.Avatar is not null)
                {
                    avatarFileParameter = new FileParameter(updateUserProfile.Avatar.OpenReadStream(), updateUserProfile.Avatar.FileName);
                }

                UserWithRolesBaseIdentityResponse apiResponse = await _identityClient.UpdateUserProfileAsync(userId, updateUserProfile.Id, updateUserProfile?.FirstName, updateUserProfile?.LastName, updateUserProfile?.EmailAddress,
                    updateUserProfile?.UserName, updateUserProfile?.Password, updateUserProfile?.Description, updateUserProfile?.Age, avatarFileParameter, updateUserProfile?.ImageUrl, updateUserProfile?.ImageLocalPath
                );

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Data = apiResponse.Data;
                    response.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (IdentityExceptions exception)
            {
                return ConvertIdentityExceptions(exception);
            }
        }

        public async Task<IdentityResponseViewModel> DeleteUsersAsync(DeleteUserListViewModel deleteUsers)
        {
            AddBearerToken();
            try
            {
                IdentityResponseViewModel response = new();
                DeleteUserListDto deleteUserListDto = _mapper.Map<DeleteUserListDto>(deleteUsers);
                DeleteUserListDtoBaseIdentityResponse apiResponse = await _identityClient.UserDELETE2Async(deleteUserListDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Data = apiResponse.Data;
                    response.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (IdentityExceptions exceptions)
            {
                return ConvertIdentityExceptions(exceptions);
            }
        }
    }
}