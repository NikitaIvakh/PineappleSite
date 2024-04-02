using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;
using System.Security.Claims;

namespace PineappleSite.Presentation.Controllers
{
    public class UserController(IUserService userService, IIdentityService identityService, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IIdentityService _identityService = identityService;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        // GET: UserController
        public async Task<ActionResult> Index(string searchUser, string currentFilter, int? pageNumber)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(searchUser))
                    {
                        var searchUsers = users.Data!.Where(
                            key => key.FirstName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.LastName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.EmailAddress.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.UserName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase)).ToList();

                        users = new IdentityCollectionResult<GetAllUsersViewModel>
                        {
                            Data = searchUsers,
                        };
                    }

                    ViewData["SearchUser"] = searchUser;
                    ViewData["CurrentFilter"] = currentFilter;

                    int pageSize = 10;
                    var filteredUsers = users.Data!.AsQueryable();
                    var paginatedUsers = PaginatedList<GetAllUsersViewModel>.Create(filteredUsers, pageNumber ?? 1, pageSize);

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
                IdentityResult<GetUserViewModel> user = await _userService.GetUserAsync(userId);

                if (user.IsSuccess)
                {
                    GetUserViewModel getUserViewModel = user.Data!;
                    return View(getUserViewModel);
                }

                else
                {
                    foreach (var error in user.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            try
            {
                IdentityResult<string> response = await _userService.CreateUserAsync(createUserViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Create));
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string userId)
        {
            try
            {
                var userWithRoles = await _userService.GetUserAsync(userId);

                if (userWithRoles.IsSuccess)
                {
                    var updateUser = new UpdateUserViewModel
                    {
                        Id = userWithRoles.Data.UserId,
                        FirstName = userWithRoles.Data.FirstName,
                        LastName = userWithRoles.Data.LastName,
                        EmailAddress = userWithRoles.Data.EmailAddress,
                        UserName = userWithRoles.Data.UserName,
                    };

                    return View(updateUser);
                }

                else
                {
                    foreach (var error in userWithRoles.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateUserViewModel updateUser)
        {
            try
            {
                IdentityResult response = await _userService.UpdateUserAsync(updateUser);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Edit));
                }
            }

            catch
            {
                return View();
            }
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteUserViewModel deleteUser)
        {
            try
            {
                IdentityResult response = await _userService.DeleteUserAsync(deleteUser);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> Profile(string? Password)
        {
            string userId = User.Claims.FirstOrDefault(key => key.Type == ClaimTypes.NameIdentifier)!.Value;
            var user = await _userService.GetUserAsync(userId, Password);

            var updateUserPrifile = new UpdateUserProfileViewModel
            {
                Id = user.Data.UserId,
                FirstName = user.Data.FirstName,
                LastName = user.Data.LastName,
                EmailAddress = user.Data.EmailAddress,
                UserName = user.Data.UserName,
                Description = user.Data.Description,
                Age = user.Data.Age,
                Roles = user.Data.Role,
                ImageUrl = user.Data.ImageUrl,
                ImageLocalPath = user.Data.ImageLocalPath,
            };

            return View(updateUserPrifile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(UpdateUserProfileViewModel updateUserProfile)
        {
            IdentityResult<GetUserForUpdateViewModel> response = await _userService.UpdateUserProfileAsync(updateUserProfile);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Profile));
            }

            else
            {
                foreach (var error in response.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction(nameof(Profile));
            }
        }

        public async Task<ActionResult> DeleteUserList(List<string> selectedUserIds)
        {
            if (selectedUserIds is null || selectedUserIds.Count <= 1)
            {
                TempData["error"] = "Выберите хотя бы одного пользователя для удаления.";
                return RedirectToAction(nameof(Index));
            }

            var deleteUsersList = new DeleteUserListViewModel { UserIds = selectedUserIds };
            var response = await _userService.DeleteUsersAsync(deleteUsersList);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            else
            {
                foreach (var error in response.ValidationErrors!)
                {
                    TempData["error"] = error;
                }

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<ActionResult> RefreshToken()
        {
            try
            {
                string jwtToken = _contextAccessor.HttpContext!.Request.Cookies["JWTToken"]!;
                string refreshToken = _contextAccessor.HttpContext!.Request.Cookies["RefreshToken"]!;

                TokenModelViewModel tokenModelViewModel = new()
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken,
                };

                var response = await _identityService.RefreshTokenAsync(tokenModelViewModel);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction("Index", "User");
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction("Index", "User");
                }
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
                var response = await _identityService.RevokeTokenAsync(userName);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction("Index", "User");
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction("Index", "User");
                }
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
                var response = await _identityService.RevokeAllTokensAsync();

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction("Index", "User");
                }

                else
                {
                    foreach (var error in response.ValidationErrors!)
                    {
                        TempData["error"] = error;
                    }

                    return RedirectToAction("Index", "User");
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}