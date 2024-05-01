using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Services;

public sealed class UserService(
    ILocalStorageService localStorageService,
    IIdentityClient identityClient,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : BaseIdentityService(localStorageService, identityClient, httpContextAccessor), IUserService
{
    private readonly IIdentityClient _identityClient = identityClient;

    public async Task<IdentityCollectionResult<GetUsersViewModel>> GetAllUsersAsync()
    {
        AddBearerToken();

        try
        {
            var users = await _identityClient.GetUsersAsync();
            var userWithRole = users.Data.Select(user => new GetUsersViewModel
            (
                user.UserId,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.EmailAddress,
                user.Role,
                user.CreatedTime,
                user.ModifiedTime
            )).ToList();

            if (users.IsSuccess)
            {
                return new IdentityCollectionResult<GetUsersViewModel>
                {
                    Count = users.Count,
                    Data = userWithRole,
                    StatusCode = users.StatusCode,
                    SuccessMessage = users.SuccessMessage,
                };
            }

            return new IdentityCollectionResult<GetUsersViewModel>
            {
                StatusCode = users.StatusCode,
                ErrorMessage = users.ErrorMessage,
                ValidationErrors = string.Join(", ", users.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityCollectionResult<GetUsersViewModel>
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityCollectionResult<GetUsersProfileViewModel>> GetUsersProfileAsync()
    {
        AddBearerToken();
        try
        {
            var apiResponse = await _identityClient.GetUsersProfileAsync();
            var getUsersProfileViewModel = apiResponse.Data.Select(key => new GetUsersProfileViewModel
            (
                UserId: key.UserId,
                FirstName: key.FirstName,
                LastName: key.LastName,
                UserName: key.UserName,
                EmailAddress: key.EmailAddress,
                Description: key.Description,
                Age: key.Age,
                ImageUrl: key.ImageUrl,
                ImageLocalPath: key.ImageLocalPath,
                CreatedTime: key.CreatedTime,
                ModifiedTime: key.ModifiedTime,
                Role: key.Role
            )).ToList();

            if (apiResponse.IsSuccess)
            {
                return new IdentityCollectionResult<GetUsersProfileViewModel>()
                {
                    Count = apiResponse.Count,
                    Data = getUsersProfileViewModel,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityCollectionResult<GetUsersProfileViewModel>()
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityCollectionResult<GetUsersProfileViewModel>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }


    public async Task<IdentityResult<GetUserViewModel>> GetUserAsync(string id)
    {
        AddBearerToken();
        try
        {
            var user = await _identityClient.GetUserAsync(id);
            var userWithRole = new GetUserViewModel
            (
                user.Data.UserId,
                user.Data.FirstName,
                user.Data.LastName,
                user.Data.UserName,
                user.Data.EmailAddress,
                user.Data.Role,
                user.Data.CreatedTime,
                user.Data.ModifiedTime
            );

            if (user.IsSuccess)
            {
                return new IdentityResult<GetUserViewModel>
                {
                    Data = userWithRole,
                    StatusCode = user.StatusCode,
                    SuccessMessage = user.SuccessMessage,
                };
            }

            return new IdentityResult<GetUserViewModel>
            {
                StatusCode = user.StatusCode,
                ErrorMessage = user.ErrorMessage,
                ValidationErrors = string.Join(", ", user.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<GetUserViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult<GetUserForUpdateViewModel>> GetUserAsync(string userId, string? password)
    {
        AddBearerToken();
        try
        {
            var user = await _identityClient.GetUserForUpdateProfileAsync(userId, password);
            GetUserForUpdateViewModel getUserForUpdateViewModel = new
            (
                user.Data.UserId,
                user.Data.FirstName,
                user.Data.LastName,
                user.Data.UserName,
                user.Data.EmailAddress,
                user.Data.Role,
                user.Data.Description,
                user.Data.Age,
                user.Data.Password,
                user.Data.ImageUrl,
                user.Data.ImageLocalPath
            );

            if (user.IsSuccess)
            {
                return new IdentityResult<GetUserForUpdateViewModel>
                {
                    StatusCode = user.StatusCode,
                    Data = getUserForUpdateViewModel,
                    SuccessMessage = user.SuccessMessage,
                };
            }

            return new IdentityResult<GetUserForUpdateViewModel>
            {
                StatusCode = user.StatusCode,
                ErrorMessage = user.ErrorMessage,
                ValidationErrors = string.Join(", ", user.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<GetUserForUpdateViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<IdentityResult<string>> CreateUserAsync(CreateUserViewModel createUserViewModel)
    {
        AddBearerToken();
        try
        {
            var createUserDto = mapper.Map<CreateUserDto>(createUserViewModel);
            var apiResponse = await _identityClient.CreateUserAsync(createUserDto);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult<string>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult<string>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<string>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult> UpdateUserAsync(UpdateUserViewModel updateUserView)
    {
        AddBearerToken();
        try
        {
            var updateUserDto = mapper.Map<UpdateUserDto>(updateUserView);
            var apiResponse = await _identityClient.UpdateUserAsync(updateUserView.Id, updateUserDto);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult<GetUserForUpdateViewModel>> UpdateUserProfileAsync(
        UpdateUserProfileViewModel updateUserProfile)
    {
        AddBearerToken();
        try
        {
            FileParameter avatarFileParameter = null;

            if (updateUserProfile.Avatar is not null)
            {
                avatarFileParameter = new FileParameter(updateUserProfile.Avatar.OpenReadStream(),
                    updateUserProfile.Avatar.FileName);
            }

            var apiResponse = await _identityClient.UpdateUserProfileAsync
            (
                updateUserProfile.Id,
                updateUserProfile.Id,
                updateUserProfile.FirstName,
                updateUserProfile.LastName,
                updateUserProfile.EmailAddress,
                updateUserProfile.UserName,
                updateUserProfile.Password,
                updateUserProfile.Description,
                updateUserProfile.Age,
                avatarFileParameter,
                updateUserProfile.ImageUrl,
                updateUserProfile.ImageLocalPath
            );

            GetUserForUpdateViewModel getUserForUpdateViewModel = new
            (
                apiResponse.Data.UserId,
                apiResponse.Data.FirstName,
                apiResponse.Data.LastName,
                apiResponse.Data.UserName,
                apiResponse.Data.EmailAddress,
                apiResponse.Data.Role,
                apiResponse.Data.Description,
                apiResponse.Data.Age,
                apiResponse.Data.Password,
                apiResponse.Data.ImageUrl,
                apiResponse.Data.ImageLocalPath
            );

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult<GetUserForUpdateViewModel>
                {
                    Data = getUserForUpdateViewModel,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult<GetUserForUpdateViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult<GetUserForUpdateViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult> DeleteUserAsync(DeleteUserViewModel delete)
    {
        AddBearerToken();
        try
        {
            var deleteUserDto = mapper.Map<DeleteUserDto>(delete);
            var apiResponse = await _identityClient.DeleteUserAsync(deleteUserDto.UserId, deleteUserDto);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<IdentityResult> DeleteUsersAsync(DeleteUsersViewModel deleteUsers)
    {
        AddBearerToken();
        try
        {
            var deleteUserListDto = mapper.Map<DeleteUsersDto>(deleteUsers);
            var apiResponse = await _identityClient.DeleteUsersAsync(deleteUserListDto);

            if (apiResponse.IsSuccess)
            {
                return new IdentityResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new IdentityResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (IdentityExceptions<string> exceptions)
        {
            return new IdentityResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }
}