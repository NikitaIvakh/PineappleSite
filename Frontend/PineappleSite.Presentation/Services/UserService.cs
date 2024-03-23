using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Users;
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

        public async Task<IdentityCollectionResult<GetAllUsersViewModel>> GetAllUsersAsync()
        {
            AddBearerToken();
            if (_httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                try
                {
                    GetAllUsersDtoCollectionResult users = await _identityClient.GetAllUsersAsync();
                    var userWithRole = new List<GetAllUsersViewModel>();

                    foreach (var user in users.Data)
                    {
                        userWithRole.Add(new GetAllUsersViewModel(user.UserId, user.FirstName, user.LastName, user.UserName, user.EmailAddress, user.Role, user.CreatedTime, user.ModifiedTime));
                    }

                    if (users.IsSuccess)
                    {
                        return new IdentityCollectionResult<GetAllUsersViewModel>
                        {
                            Count = users.Count,
                            SuccessCode = users.SuccessCode,
                            SuccessMessage = users.SuccessMessage,
                            Data = userWithRole,
                        };
                    }

                    else
                    {
                        foreach (var error in users.ValidationErrors)
                        {
                            return new IdentityCollectionResult<GetAllUsersViewModel>
                            {
                                ValidationErrors = [error],
                                ErrorCode = users.ErrorCode,
                                ErrorMessage = users.ErrorMessage,
                            };
                        }
                    }

                    return new IdentityCollectionResult<GetAllUsersViewModel>();
                }

                catch (IdentityExceptions exceptions)
                {
                    if (exceptions.StatusCode == 403)
                    {
                        return new IdentityCollectionResult<GetAllUsersViewModel>
                        {
                            ErrorCode = exceptions.StatusCode,
                            ErrorMessage = exceptions.Response,
                            ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                        };
                    }

                    else if (exceptions.StatusCode == 401)
                    {
                        return new IdentityCollectionResult<GetAllUsersViewModel>
                        {
                            ErrorCode = exceptions.StatusCode,
                            ErrorMessage = exceptions.Response,
                            ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                        };
                    }

                    else
                    {
                        return new IdentityCollectionResult<GetAllUsersViewModel>
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
                return new IdentityCollectionResult<GetAllUsersViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }
        }

        public async Task<IdentityResult<GetUserViewModel>> GetUserAsync(string id)
        {
            AddBearerToken();
            try
            {
                GetUserDtoResult user = await _identityClient.GetUserByIdAsync(id);
                var userWithRole = new GetUserViewModel(user.Data.UserId, user.Data.FirstName, user.Data.LastName, user.Data.UserName, user.Data.EmailAddress, user.Data.Role,
                    user.Data.CreatedTime, user.Data.ModifiedTime);

                if (user.IsSuccess)
                {
                    return new IdentityResult<GetUserViewModel>
                    {
                        Data = userWithRole,
                        SuccessCode = user.SuccessCode,
                        SuccessMessage = user.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in user.ValidationErrors)
                    {
                        return new IdentityResult<GetUserViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = user.ErrorCode,
                            ErrorMessage = user.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<GetUserViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<GetUserViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<GetUserViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<GetUserViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<IdentityResult<GetUserForUpdateViewModel>> GetUserAsync(string userId, string? Password)
        {
            AddBearerToken();
            try
            {
                GetUserForUpdateDtoResult user = await _identityClient.GetUserForUpdateAsync(userId, Password);
                GetUserForUpdateViewModel getUserForUpdateViewModel = new(user.Data.UserId, user.Data.FirstName, user.Data.LastName, user.Data.UserName, user.Data.EmailAddress, user.Data.Role, user.Data.Description, user.Data.Age,
                    user.Data.Password, user.Data.ImageUrl, user.Data.ImageLocalPath);

                if (user.IsSuccess)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        SuccessCode = user.SuccessCode,
                        SuccessMessage = user.SuccessMessage,
                        Data = getUserForUpdateViewModel,
                    };
                }

                else
                {
                    foreach (var error in user.ValidationErrors)
                    {
                        return new IdentityResult<GetUserForUpdateViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = user.ErrorCode,
                            ErrorMessage = user.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<GetUserForUpdateViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<IdentityResult<string>> CreateUserAsync(CreateUserViewModel createUserViewModel)
        {
            AddBearerToken();
            try
            {
                CreateUserDto createUserDto = _mapper.Map<CreateUserDto>(createUserViewModel);
                StringResult apiResponse = await _identityClient.CreateUserAsync(createUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<string>
                    {
                        Data = apiResponse.Data,
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<string>
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = [error],
                        };
                    }
                }

                return new IdentityResult<string>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<string>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<string>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<string>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserViewModel updateUserView)
        {
            AddBearerToken();
            try
            {
                UpdateUserDto updateUserDto = _mapper.Map<UpdateUserDto>(updateUserView);
                UnitResult apiResponse = await _identityClient.UserPUTAsync(updateUserView.Id, updateUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult
                        {
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                            ValidationErrors = [error],
                        };
                    }
                }

                return new IdentityResult();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<IdentityResult<GetUserForUpdateViewModel>> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfile)
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

                GetUserForUpdateDtoResult apiResponse = await _identityClient.UpdateUserProfileAsync(userId, updateUserProfile.Id, updateUserProfile?.FirstName, updateUserProfile?.LastName, updateUserProfile?.EmailAddress,
                    updateUserProfile?.UserName, updateUserProfile?.Password, updateUserProfile?.Description, updateUserProfile?.Age, avatarFileParameter, updateUserProfile?.ImageUrl, updateUserProfile?.ImageLocalPath
                );

                GetUserForUpdateViewModel getUserForUpdateViewModel = new(apiResponse.Data.UserId, apiResponse.Data.FirstName, apiResponse.Data.LastName, apiResponse.Data.UserName,
                    apiResponse.Data.EmailAddress, apiResponse.Data.Role, apiResponse.Data.Description, apiResponse.Data.Age, apiResponse.Data.Password, apiResponse.Data.ImageUrl, apiResponse.Data.ImageLocalPath);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        Data = getUserForUpdateViewModel,
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult<GetUserForUpdateViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult<GetUserForUpdateViewModel>();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult<GetUserForUpdateViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(DeleteUserViewModel delete)
        {
            AddBearerToken();
            try
            {
                DeleteUserDto deleteUserDto = _mapper.Map<DeleteUserDto>(delete);
                UnitResult apiResponse = await _identityClient.UserDELETEAsync(deleteUserDto.Id, deleteUserDto);

                if (apiResponse.IsSuccess)
                {
                    return new IdentityResult
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new IdentityResult
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new IdentityResult();
            }

            catch (IdentityExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new IdentityResult
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new IdentityResult
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertIdentityExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new IdentityResult
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