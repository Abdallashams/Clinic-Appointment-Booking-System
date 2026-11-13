using ClinicBookingSystem.ModelVM.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await userService.CreateUserAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(user);
        }
        [HttpGet] 
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await userService.LoginAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            if (result.IsLockedOut)
                ModelState.AddModelError("", "Your account is locked.");

            else if (result.IsNotAllowed)
                ModelState.AddModelError("", "Login is not allowed.");

            else
                ModelState.AddModelError("", "Invalid email or password.");

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await userService.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
