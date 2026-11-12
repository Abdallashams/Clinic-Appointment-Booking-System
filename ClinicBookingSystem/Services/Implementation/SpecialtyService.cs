using Microsoft.EntityFrameworkCore;
namespace ClinicBookingSystem.Services.Implementation
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ClinicDbContext  _context;

        public SpecialtyService(ClinicDbContext context)
        {
            _context = context;

        }


        public async Task<IEnumerable<GetSpecialtyVM>> GetAllAsync()
        {
            return await _context.Specialties
                .Select(s => new GetSpecialtyVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToListAsync();
        }



        public async Task<GetSpecialtyVM?> GetByIdAsync(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return null;

            return new GetSpecialtyVM
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description
            };
        }



        public async Task<bool> CreateAsync(CreateSpecialtyVM model)
        {
            var specialty = new Specialty
            {
                Name = model.Name,
                Description = model.Description
            };

            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> UpdateAsync(UpdateSpecialtyVM model)
        {
            var specialty = await _context.Specialties.FindAsync(model.Id);
            if (specialty == null) return false;

            specialty.Name = model.Name;
            specialty.Description = model.Description;

            _context.Specialties.Update(specialty);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null) return false;

            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
