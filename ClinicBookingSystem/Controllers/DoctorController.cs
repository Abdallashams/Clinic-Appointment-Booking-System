using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ClinicBookingSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService doctorService;
        private readonly ISpecialtyService specialtyService;

        public DoctorController(IDoctorService doctorService,ISpecialtyService specialtyService)
        {
            this.doctorService = doctorService;
            this.specialtyService = specialtyService;
        }
        public async Task<IActionResult> Index()
        {
            var doctors = await doctorService.GetAll();
            if (doctors == null || doctors.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No doctors found.");
            }

            return View(doctors);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Specialties = new SelectList(await specialtyService.GetAllAsync(), "Id", "Name");
            return View(new CreateDoctorVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Specialties = new SelectList(await specialtyService.GetAllAsync(), "Id", "Name");
                return View(model);
            }
            try
            {
                await doctorService.MakeDoctor(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the doctor: {ex.Message}");
                ViewBag.Specialties = new SelectList(await specialtyService.GetAllAsync(), "Id", "Name");
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result =await doctorService.DeleteDoctor(id);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete doctor.");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await doctorService.ToggleActive(id);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to toggle active status.");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var doctor = await doctorService.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            var model = new UpdateDoctorVM
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                Bio = doctor.Bio,
                ConsultationDurationMinutes = doctor.ConsultationDurationMinutes,
                SpecialtyId = doctor.SpecialtyId
            };
            ViewBag.Specialties = new SelectList(await specialtyService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDoctorVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await doctorService.UpdateDoctor(model);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update doctor.");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
