using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Controllers
{
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
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
                IdentityResponseViewModel response = await _userService.CreateUserAsync(registerRequest);

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
                IdentityResponseViewModel response = await _userService.UpdateUserAsync(updateUser);

                if (response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["error"] = response.ValidationErrors;
                    return RedirectToAction(nameof(Edit));
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}