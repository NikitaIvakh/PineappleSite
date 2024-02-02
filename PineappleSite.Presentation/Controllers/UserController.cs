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
            string userId = User.Claims.FirstOrDefault(key => key.Type == "uid")?.Value;
            var users = await _userService.GetAllUsersAsync(userId);

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

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userService.GetUserAsync(id);
            return View(user);
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
                    TempData["success"] = "Пользователь успешно добавлен";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
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
            var userWithRoles = await _userService.GetUserAsync(id);
            var updateUser = new UpdateUserViewModel
            {
                Id = userWithRoles.User.Id,
                FirstName = userWithRoles.User.FirstName,
                LastName = userWithRoles.User.LastName,
                EmailAddress = userWithRoles.User.Email,
                UserName = userWithRoles.User.UserName,
                UserRoles = userWithRoles.User.UserRoles,
            };

            return View(updateUser);
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
                Id = user.User.Id,
                FirstName = user.User.FirstName,
                LastName = user.User.LastName,
                EmailAddress = user.User.Email,
                UserName = user.User.UserName,
                Description = user.User.Description,
                Age = user.User.Age,
                Roles = user.Roles,
                ImageUrl = user.User.ImageUrl,
                ImageLocalPath = user.User.ImageLocalPath,
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