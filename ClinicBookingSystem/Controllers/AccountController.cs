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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVm user)
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
    }
}
