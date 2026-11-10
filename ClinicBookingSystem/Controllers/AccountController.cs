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
        public async Task<IActionResult> Index()
        {
           var users= await userService.GetAllActiveUsersAsync();
            if (users == null || users.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No users found.");
            }
            return View(users);
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
            if (ModelState.IsValid)
            {
                try
                {
                    await userService.LoginAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
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
