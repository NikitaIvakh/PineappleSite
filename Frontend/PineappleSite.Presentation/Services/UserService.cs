using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;

namespace PineappleSite.Presentation.Services
{
    public class UserService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : BaseIdentityService(localStorageService, identityClient, httpContextAccessor), IUserService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IdentityCollectionResult<UserWithRolesViewModel>> GetAllUsersAsync(string userId)
        {
            AddBearerToken();
            if (_httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                try
                {
                    UserWithRolesDtoResult user = await _identityClient.GetUserByIdAsync(userId);
                    UserWithRolesDtoCollectionResult users = await _identityClient.GetAllUsersAsync(user.Data.User.Id);

                    if (users.IsSuccess)
                    {
                        return _mapper.Map<IdentityCollectionResult<UserWithRolesViewModel>>(users);
                    }

                    else
                    {
                        foreach (var error in users.ValidationErrors)
                        {
                            return new IdentityCollectionResult<UserWithRolesViewModel>
                            {
                                ValidationErrors = [error],
                                ErrorCode = users.ErrorCode,
                                ErrorMessage = users.ErrorMessage,
                            };
                        }
                    }

                    return new IdentityCollectionResult<UserWithRolesViewModel>();
                }

                catch (IdentityExceptions exceptions)
                {
                    if (exceptions.StatusCode == 403)
                    {
                        return new IdentityCollectionResult<UserWithRolesViewModel>
                        {
                            ErrorCode = exceptions.StatusCode,
                            ErrorMessage = exceptions.Response,
                            ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                        };
                    }

                    else if (exceptions.StatusCode == 401)
                    {
                        return new IdentityCollectionResult<UserWithRolesViewModel>
                        {
                            ErrorCode = exceptions.StatusCode,
                            ErrorMessage = exceptions.Response,
                            ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                        };
                    }

                    else
                    {
                        return new IdentityCollectionResult<UserWithRolesViewModel>
                        {
                            ErrorMessage = exceptions.Response,
                            ErrorCode = exceptions.StatusCode,
                            ValidationErrors = [exceptions.Response]
                        };
                    }
                }
            }

            else
            {
                return new IdentityCollectionResult<UserWithRolesViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
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
                            ValidationErrors = [error],
                            ErrorCode = user.ErrorCode,
                            ErrorMessage = user.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
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
                            ValidationErrors = [error],
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
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
                            ValidationErrors = [error],
                        };
                    }
                }

                return new IdentityResult<RegisterResponseViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<RegisterResponseViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<RegisterResponseViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<RegisterResponseViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
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
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<UserWithRolesViewModel>(apiResponse),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<UserWithRolesViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<UserWithRolesViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<UserWithRolesViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
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
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<DeleteUserViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<DeleteUserViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<DeleteUserViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<DeleteUserViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
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
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<bool>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<bool>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<bool>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<bool>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }
    }
}