using ClinicBookingSystem.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllActiveUsersAsync();
            if (users == null || users.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No users found.");
            }
            return View(users);
        }
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await userService.DeleteUserAsync(userId);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete user.");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var result = await userService.AddUserToAdmin(userId);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to add user to admin role.");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> MakeDoctor(string userId)
        {
            var result = await userService.AddUserToAdmin(userId);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to add user to admin role.");
            }
            return RedirectToAction("Index");
        }
    }
}
