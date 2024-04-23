﻿using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers;

public sealed class UserController(
    IUserService userService,
    IIdentityService identityService,
    IHttpContextAccessor contextAccessor) : Controller
{
    // GET: UserController
    public async Task<ActionResult> Index(string searchUser, string currentFilter, int? pageNumber)
    {
        try
        {
            var users = await userService.GetAllUsersAsync();

            if (users.IsSuccess)
            {
                if (!string.IsNullOrEmpty(searchUser))
                {
                    var searchUsers = users.Data!.Where(
                            key => key.FirstName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.LastName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                                   key.EmailAddress.Contains(searchUser,
                                       StringComparison.CurrentCultureIgnoreCase) ||
                                   key.Role.Any(key =>
                                       key.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase)) ||
                                   key.UserName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    users = new IdentityCollectionResult<GetUsersViewModel>
                    {
                        Data = searchUsers,
                    };
                }

                ViewData["SearchUser"] = searchUser;
                ViewData["CurrentFilter"] = currentFilter;

                const int pageSize = 10;
                var filteredUsers = users.Data!.AsQueryable();
                var paginatedUsers =
                    PaginatedList<GetUsersViewModel>.Create(filteredUsers, pageNumber ?? 1, pageSize);

                return View(paginatedUsers);
            }

            else
            {
                foreach (var error in users.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction("Index", "Home");
            }
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View();
        }
    }

    // GET: UserController/Details/5
    public async Task<ActionResult> Details(string userId)
    {
        try
        {
            var user = await userService.GetUserAsync(userId);

            if (user.IsSuccess)
            {
                var getUserViewModel = user.Data!;
                return View(getUserViewModel);
            }

            TempData["error"] = user.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: UserController/Create
    public Task<ActionResult> Create()
    {
        return Task.FromResult<ActionResult>(View());
    }

    // POST: UserController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateUserViewModel createUserViewModel)
    {
        try
        {
            var response = await userService.CreateUserAsync(createUserViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Create));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: UserController/Edit/5
    public async Task<ActionResult> Edit(string userId)
    {
        try
        {
            var userWithRoles = await userService.GetUserAsync(userId);

            if (userWithRoles.IsSuccess)
            {
                var updateUser = new UpdateUserViewModel
                {
                    Id = userWithRoles.Data?.UserId,
                    FirstName = userWithRoles.Data!.FirstName,
                    LastName = userWithRoles.Data.LastName,
                    EmailAddress = userWithRoles.Data.EmailAddress,
                    UserName = userWithRoles.Data.UserName,
                };

                return View(updateUser);
            }

            TempData["error"] = userWithRoles.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // POST: UserController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(UpdateUserViewModel updateUser)
    {
        try
        {
            var response = await userService.UpdateUserAsync(updateUser);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Edit));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    // POST: UserController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(DeleteUserViewModel deleteUser)
    {
        try
        {
            var response = await userService.DeleteUserAsync(deleteUser);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction("Index", "Home");
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> Profile(string? password)
    {
        var userId = User.Claims.FirstOrDefault(key => key.Type == ClaimTypes.NameIdentifier)!.Value;
        var user = await userService.GetUserAsync(userId, password);

        var updateUserProfile = new UpdateUserProfileViewModel
        {
            Id = user.Data?.UserId,
            FirstName = user.Data!.FirstName,
            LastName = user.Data.LastName,
            EmailAddress = user.Data.EmailAddress,
            UserName = user.Data.UserName,
            Description = user.Data.Description,
            Age = user.Data.Age,
            Roles = user.Data.Role,
            ImageUrl = user.Data.ImageUrl,
            ImageLocalPath = user.Data.ImageLocalPath,
        };

        return View(updateUserProfile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Profile(UpdateUserProfileViewModel updateUserProfile)
    {
        try
        {
            var response = await userService.UpdateUserProfileAsync(updateUserProfile);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Profile));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Profile));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<ActionResult> DeleteUserList(List<string> selectedUserIds)
    {
        try
        {
            if (selectedUserIds.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы одного пользователя для удаления.";
                return RedirectToAction(nameof(Index));
            }

            var deleteUsersList = new DeleteUsersViewModel(selectedUserIds);
            var response = await userService.DeleteUsersAsync(deleteUsersList);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction(nameof(Index));
        }

        catch (Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<ActionResult> RefreshToken()
    {
        try
        {
            var jwtToken = contextAccessor.HttpContext!.Request.Cookies["JWTToken"]!;
            var refreshToken = contextAccessor.HttpContext!.Request.Cookies["RefreshToken"]!;

            TokenModelViewModel tokenModelViewModel = new()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
            };

            var response = await identityService.RefreshTokenAsync(tokenModelViewModel);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction("Index", "User");
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction("Index", "User");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> RevokeToken(string userName)
    {
        try
        {
            var response = await identityService.RevokeTokenAsync(userName);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction("Index", "User");
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction("Index", "User");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(userName);
        }
    }

    [HttpPost]
    public async Task<ActionResult> RevokeAllTokens()
    {
        try
        {
            var response = await identityService.RevokeAllTokensAsync();

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction("Index", "User");
            }

            TempData["error"] = response.ValidationErrors;
            return RedirectToAction("Index", "User");
        }

        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}