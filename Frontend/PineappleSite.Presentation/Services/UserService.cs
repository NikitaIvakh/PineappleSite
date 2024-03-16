using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;

namespace PineappleSite.Presentation.Services
{
    public class UserService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper, IHttpContextAccessor httpContextAccessor) : BaseIdentityService(localStorageService, identityClient), IUserService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IdentityCollectionResult<UserWithRolesViewModel>> GetAllUsersAsync(string userId)
        {
            AddBearerToken();
            try
            {
                UserWithRolesDtoResult user = await _identityClient.GetUserByIdAsync(userId);
                UserWithRolesDtoCollectionResult users = await _identityClient.GetAllUsersAsync(user.Data.User.Id);

                if (users.IsSuccess || user.IsSuccess)
                {
                    return _mapper.Map<IdentityCollectionResult<UserWithRolesViewModel>>(users);
                }

                else
                {
                    foreach (var error in users.ValidationErrors)
                    {
                        return new IdentityCollectionResult<UserWithRolesViewModel>
                        {
                            ErrorCode = users.ErrorCode,
                            ErrorMessage = users.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityCollectionResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityCollectionResult<UserWithRolesViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<UserWithRolesViewModel>> GetUserAsync(string id)
        {
            AddBearerToken();
            try
            {
                UserWithRolesDtoResult user = await _identityClient.GetUserByIdAsync(id);

                if (user.IsSuccess)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        Data = _mapper.Map<UserWithRolesViewModel>(user.Data)
                    };
                }

                else
                {
                    foreach (var error in user.ValidationErrors)
                    {
                        return new IdentityResult<UserWithRolesViewModel>
                        {
                            ErrorMessage = user.ErrorMessage,
                            ErrorCode = user.ErrorCode,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<UserWithRolesViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<UserWithRolesViewModel>> CreateUserAsync(CreateUserViewModel createUserViewModel)
        {
            AddBearerToken();
            try
            {
                CreateUserDto createUserDto = _mapper.Map<CreateUserDto>(createUserViewModel);
                UserWithRolesDtoResult apiResponse = await _identityClient.CreateUserAsync(createUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        Data = _mapper.Map<UserWithRolesViewModel>(apiResponse.Data),
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<UserWithRolesViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exception)
            {
                return new IdentityResult<UserWithRolesViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<RegisterResponseViewModel>> UpdateUserAsync(UpdateUserViewModel updateUserView)
        {
            AddBearerToken();
            try
            {
                UpdateUserDto updateUserDto = _mapper.Map<UpdateUserDto>(updateUserView);
                RegisterResponseDtoResult apiResponse = await _identityClient.UserPUTAsync(updateUserDto.Id, updateUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<RegisterResponseViewModel>
                    {
                        Data = _mapper.Map<RegisterResponseViewModel>(apiResponse),
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<RegisterResponseViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<RegisterResponseViewModel>();
            }

            catch (IdentityExceptions exception)
            {
                return new IdentityResult<RegisterResponseViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<UserWithRolesViewModel>> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfile)
        {
            AddBearerToken();
            try
            {
                string userId = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(key => key.Type == ClaimTypes.NameIdentifier)!.Value!;

                FileParameter avatarFileParameter = null;

                if (updateUserProfile.Avatar is not null)
                {
                    avatarFileParameter = new FileParameter(updateUserProfile.Avatar.OpenReadStream(), updateUserProfile.Avatar.FileName);
                }

                UserWithRolesDtoResult apiResponse = await _identityClient.UpdateUserProfileAsync(userId, updateUserProfile.Id, updateUserProfile?.FirstName, updateUserProfile?.LastName, updateUserProfile?.EmailAddress,
                    updateUserProfile?.UserName, updateUserProfile?.Password, updateUserProfile?.Description, updateUserProfile?.Age, avatarFileParameter, updateUserProfile?.ImageUrl, updateUserProfile?.ImageLocalPath
                );

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        Data = _mapper.Map<UserWithRolesViewModel>(apiResponse),
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<UserWithRolesViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exception)
            {
                return new IdentityResult<UserWithRolesViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<DeleteUserViewModel>> DeleteUserAsync(DeleteUserViewModel delete)
        {
            AddBearerToken();
            try
            {
                DeleteUserDto deleteUserDto = _mapper.Map<DeleteUserDto>(delete);
                DeleteUserDtoResult apiResponse = await _identityClient.UserDELETEAsync(deleteUserDto.Id, deleteUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<DeleteUserViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<DeleteUserViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<DeleteUserViewModel>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<DeleteUserViewModel>();
            }

            catch (IdentityExceptions exception)
            {
                return new IdentityResult<DeleteUserViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<IdentityResult<bool>> DeleteUsersAsync(DeleteUserListViewModel deleteUsers)
        {
            AddBearerToken();
            try
            {
                DeleteUserListDto deleteUserListDto = _mapper.Map<DeleteUserListDto>(deleteUsers);
                BooleanResult apiResponse = await _identityClient.UserDELETE2Async(deleteUserListDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<bool>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = apiResponse.Data,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<bool>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new IdentityResult<bool>();
            }

            catch (IdentityExceptions exceptions)
            {
                return new IdentityResult<bool>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }
    }
}