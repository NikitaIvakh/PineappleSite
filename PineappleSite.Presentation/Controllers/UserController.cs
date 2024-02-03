using Microsoft.AspNetCore.Mvc;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Paginated;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        // GET: UserController
        public async Task<ActionResult> Index(string searchUser, string currentFilter, int? pageNumber)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(key => key.Type == "uid")?.Value;
                var users = await _userService.GetAllUsersAsync(userId);

                if (users.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(searchUser))
                    {
                        var searchUsers = users.Data.Where(
                            key => key.User.FirstName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.User.LastName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.User.Email.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase) ||
                            key.User.UserName.Contains(searchUser, StringComparison.CurrentCultureIgnoreCase)).ToList();

                        users = new IdentityCollectionResult<UserWithRolesViewModel>
                        {
                            Data = searchUsers,
                        };
                    }

                    ViewData["SearchUser"] = searchUser;
                    ViewData["CurrentFilter"] = currentFilter;

                    int pageSize = 10;
                    var filteredUsers = users.Data.AsQueryable();
                    var paginatedUsers = PaginatedList<UserWithRolesViewModel>.Create(filteredUsers, pageNumber ?? 1, pageSize);

                    return View(paginatedUsers);
                }

                else
                {
                    TempData["error"] = users.ErrorMessage;
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
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                IdentityResult<UserWithRolesViewModel> user = await _userService.GetUserAsync(id);

                if (user.IsSuccess)
                {
                    UserWithRolesViewModel userWithRolesViewModel = new()
                    {
                        User = user.Data.User,
                        Roles = user.Data.Roles,
                    };

                    return View(userWithRolesViewModel);
                }

                else
                {
                    TempData["error"] = user.ErrorMessage;
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
        public async Task<ActionResult> Create(RegisterRequestViewModel registerRequest)
        {
            try
            {
                IdentityResult<RegisterResponseViewModel> response = await _userService.CreateUserAsync(registerRequest);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Create));
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var userWithRoles = await _userService.GetUserAsync(id);

                if (userWithRoles.IsSuccess)
                {
                    var updateUser = new UpdateUserViewModel
                    {
                        Id = userWithRoles.Data.User.Id,
                        FirstName = userWithRoles.Data.User.FirstName,
                        LastName = userWithRoles.Data.User.LastName,
                        EmailAddress = userWithRoles.Data.User.Email,
                        UserName = userWithRoles.Data.User.UserName,
                        UserRoles = userWithRoles.Data.User.UserRoles,
                    };

                    return View(updateUser);
                }

                else
                {
                    TempData["error"] = userWithRoles.ErrorMessage;
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
                IdentityResult<RegisterResponseViewModel> response = await _userService.UpdateUserAsync(updateUser);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
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
                IdentityResult<DeleteUserViewModel> response = await _userService.DeleteUserAsync(deleteUser);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.SuccessMessage;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Profile()
        {
            string userId = User.Claims.FirstOrDefault(key => key.Type == "uid")?.Value;
            var user = await _userService.GetUserAsync(userId);

            var updateUserPrifile = new UpdateUserProfileViewModel
            {
                Id = user.Data.User.Id,
                FirstName = user.Data.User.FirstName,
                LastName = user.Data.User.LastName,
                EmailAddress = user.Data.User.Email,
                UserName = user.Data.User.UserName,
                Description = user.Data.User.Description,
                Age = user.Data.User.Age,
                Roles = user.Data.Roles,
                ImageUrl = user.Data.User.ImageUrl,
                ImageLocalPath = user.Data.User.ImageLocalPath,
            };

            return View(updateUserPrifile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(UpdateUserProfileViewModel updateUserProfile)
        {
            IdentityResult<UserWithRolesViewModel> response = await _userService.UpdateUserProfileAsync(updateUserProfile);

            if (response.IsSuccess)
            {
                TempData["success"] = response.SuccessMessage;
                return RedirectToAction(nameof(Profile));
            }

            else
            {
                TempData["error"] = response.ErrorMessage;
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
                TempData["error"] = response.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}