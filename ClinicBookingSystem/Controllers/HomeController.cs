using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClinicBookingSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IDoctorService doctorService;
        private readonly ISpecialtyService specialtyService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAppointmentService appointmentService;

        public HomeController(IDoctorService doctorService,ISpecialtyService specialtyService,UserManager<ApplicationUser> userManager,IAppointmentService appointmentService, ILogger<HomeController> logger)
        {
            this.doctorService = doctorService;
            this.specialtyService = specialtyService;
            this.userManager = userManager;
            this.appointmentService = appointmentService;
            _logger = logger;
        }
  
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctors = await doctorService.GetAll();
            ViewBag.AppointmentsCount = (await appointmentService.GetAllAppointmentsAsync()).Count;
            ViewBag.PatientsCount = (await userManager.GetUsersInRoleAsync("Patient")).Count;
            return View(doctors);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
