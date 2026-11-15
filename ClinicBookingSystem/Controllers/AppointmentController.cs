using ClinicBookingSystem.ModelVM.Appointment;
using ClinicBookingSystem.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicBookingSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentservic;
        private readonly IDoctorService doctorService;

        public AppointmentController(IAppointmentService appointmentservic, IDoctorService doctorService)
        {
            this.appointmentservic = appointmentservic;
            this.doctorService = doctorService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await appointmentservic.GetAllAppointmentsAsync();

            return View(appointments);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int? doctorId)
        {
            var doctors = await doctorService.GetAll();
            ViewBag.Doctors = new SelectList(doctors.Where(d => d.IsActive), "Id", "FullName");
            return View(new CreateAppointmentVM
            {
                AppointmentDate = DateTime.Today,
                DoctorId = doctorId ?? 0
            });
        }
        //[Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                var doctors = await doctorService.GetAll();
                ViewBag.Doctors = new SelectList(doctors.Where(d => d.IsActive), "Id", "FullName");
                return View(model);
            }

            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await appointmentservic.BookAppointmentAsync(model, patientId);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var doctors = await doctorService.GetAll();
                ViewBag.Doctors = new SelectList(doctors.Where(d => d.IsActive), "Id", "FullName");
                return View(model);
            }
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int doctorId, DateTime date)
        {
            var times = await appointmentservic.GetAvailableTimesAsync(doctorId, date.Date);
            return Json(times.Select(t => t.ToString(@"hh\:mm")));
        }
        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await appointmentservic.ConfirmAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be confirmed.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await appointmentservic.CancelAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be canceld.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await appointmentservic.CompleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be completed.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorInfo(int doctorId)
        {
            var info = await appointmentservic.GetDoctorBookingInfoAsync(doctorId);
            if (info == null)
                return NotFound();
            return Json(info);
        }
    }
}