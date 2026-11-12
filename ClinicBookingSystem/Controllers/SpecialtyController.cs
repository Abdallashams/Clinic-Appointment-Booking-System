using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    public class SpecialtyController : Controller
    {

        private readonly ISpecialtyService specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            this.specialtyService = specialtyService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specialties = await specialtyService.GetAllAsync();
            return View(specialties);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var specialty = await specialtyService.GetByIdAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSpecialtyVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await specialtyService.CreateAsync(model);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "Failed to create specialty.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var specialty = await specialtyService.GetByIdAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }

            var updateModel = new UpdateSpecialtyVM
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description
            };

            return View(updateModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSpecialtyVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await specialtyService.UpdateAsync(model);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "Failed to update specialty.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await specialtyService.DeleteAsync(id);
                if (!result)
                {
                    TempData["Error"] = "Failed to delete specialty.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }




    }
}
