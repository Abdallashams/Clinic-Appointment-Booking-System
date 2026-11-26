using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ClinicBookingSystem.Controllers
{
    [Authorize(Roles = "Doctor,Admin")]
    public class AvailabilitySlotController : Controller
    {
        private readonly IAvailabilitySlotService slotService;
        private readonly IDoctorService doctorService;

        public AvailabilitySlotController(IAvailabilitySlotService slotService, IDoctorService doctorService)
        {
            this.slotService = slotService;
            this.doctorService = doctorService;
        }

  

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var slots = await slotService.GetAllAsync();
            return View(slots);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var slot = await slotService.GetByIdAsync(id);
            if (slot == null) return NotFound();
            return View(slot);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Doctor") && !User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var doctor = string.IsNullOrEmpty(userId) ? null : await doctorService.GetByUserId(userId);
                if (doctor == null) return NotFound("Doctor profile not found.");

                ViewBag.CurrentDoctorName = doctor.FullName;
                return View(new CreateAvailabilitySlotVM { DoctorId = doctor.Id });
            }

            ViewBag.Doctors = new SelectList(await doctorService.GetAll(), "Id", "FullName");
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAvailabilitySlotVM model)
        {
            if (User.IsInRole("Doctor") && !User.IsInRole("Admin"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var doctor = string.IsNullOrEmpty(userId) ? null : await doctorService.GetByUserId(userId);
                if (doctor == null) return NotFound("Doctor profile not found.");

                model.DoctorId = doctor.Id;
                ModelState.Remove(nameof(model.DoctorId));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await slotService.CreateAsync(model);
                    if (result) return RedirectToAction("Index");

                    ModelState.AddModelError(string.Empty, "Failed to create slot.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.Doctors = new SelectList(await doctorService.GetAll(), "Id", "FullName", model.DoctorId);
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slot = await slotService.GetByIdAsync(id);
            if (slot == null) return NotFound();

            var updateModel = new UpdateAvailabilitySlotVM
            {
                Id = slot.Id,
                DoctorId = slot.DoctorId,
                DayOfWeek = slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsActive = slot.IsActive
            };

            ViewBag.Doctors = new SelectList(await doctorService.GetAll(), "Id", "FullName", slot.DoctorId);
            return View(updateModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAvailabilitySlotVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await slotService.UpdateAsync(model);
                    if (result) return RedirectToAction("Index");

                    ModelState.AddModelError(string.Empty, "Failed to update slot.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.Doctors = new SelectList(await doctorService.GetAll(), "Id", "FullName", model.DoctorId);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await slotService.DeleteAsync(id);
                if (!result) TempData["Error"] = "Failed to delete slot.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }




    }
}
