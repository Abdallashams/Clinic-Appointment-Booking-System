using ClinicBookingSystem.ModelVM.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await appointmentservic.GetAllAppointmentsAsync();

            return View(appointments);
        }
        [HttpGet]
        public async Task<IActionResult> PatientAppointments()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await appointmentservic.GetPatientAppointmentsAsync(patientId);

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
        [Authorize(Roles = "Patient")]
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
                return RedirectToAction("PatientAppointments");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var doctors = await doctorService.GetAll();
                ViewBag.Doctors = new SelectList(doctors.Where(d => d.IsActive), "Id", "FullName");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int doctorId, DateTime date)
        {
            var times = await appointmentservic.GetAvailableTimesAsync(doctorId, date.Date);
            return Json(times.Select(t => t.ToString(@"hh\:mm")));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id, DateTime? date)
        {
            var result = await appointmentservic.ConfirmAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be confirmed.";
            }
            return RedirectAfterStatusChange(date);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, DateTime? date)
        {
            var result = await appointmentservic.CancelAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be cancelled.";
            }
            return RedirectAfterStatusChange(date);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, DateTime? date)
        {
            var result = await appointmentservic.CompleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "This appointment cannot be completed.";
            }
            return RedirectAfterStatusChange(date);
        }


        [HttpGet]
        public async Task<IActionResult> GetDoctorInfo(int doctorId)
        {
            var info = await appointmentservic.GetDoctorBookingInfoAsync(doctorId);
            if (info == null)
                return NotFound();
            return Json(info);
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorSchedule(DateTime? date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var doctor = await doctorService.GetByUserId(userId);
            if (doctor == null)
                return NotFound("Doctor profile not found.");

            var day = (date ?? DateTime.Today).Date;
            var appointments = await appointmentservic.GetDoctorAppointmentsAsync(doctor.Id, day);

            ViewBag.DoctorId = doctor.Id;
            ViewBag.DoctorName = doctor.FullName;
            ViewBag.Date = day;


            return View(appointments);
        }


        private IActionResult RedirectAfterStatusChange(DateTime? date)
        {
            if (User.IsInRole("Doctor"))
                return RedirectToAction(nameof(DoctorSchedule), new { date = date?.ToString("yyyy-MM-dd") });

            return RedirectToAction(nameof(Index));
        }

    }
}