using ClinicBookingSystem.ModelVM.AvailabilitySlot;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Services.Implementation
{
    public class AvailabilitySlotService : IAvailabilitySlotService
    {
        private readonly ClinicDbContext _context;

        public AvailabilitySlotService(ClinicDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<GetAvailabilitySlotVM>> GetAllAsync()
        {
            return await _context.AvailabilitySlots
                .Include(a => a.Doctor)
                .Select(a => new GetAvailabilitySlotVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<GetAvailabilitySlotVM>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.AvailabilitySlots
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Doctor)
                .Select(a => new GetAvailabilitySlotVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }



        public async Task<GetAvailabilitySlotVM?> GetByIdAsync(int id)
        {
            var slot = await _context.AvailabilitySlots
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (slot == null) return null;

            return new GetAvailabilitySlotVM
            {
                Id = slot.Id,
                DoctorId = slot.DoctorId,
                DoctorName = slot.Doctor != null ? slot.Doctor.FullName : string.Empty,
                DayOfWeek = slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsActive = slot.IsActive
            };
        }


        public async Task<bool> CreateAsync(CreateAvailabilitySlotVM model)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId);
            if (!doctorExists) return false;

            bool isConflict = await IsSlotConflictExistsAsync(model.DoctorId, model.DayOfWeek, model.StartTime, model.EndTime);
            if (isConflict)
            {
                throw new Exception("This time slot conflicts with another existing schedule for this doctor on the same day. Please choose a different time.");
            }

            var slot = new AvailabilitySlot
            {
                DoctorId = model.DoctorId,
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                IsActive = true
            };

            await _context.AvailabilitySlots.AddAsync(slot);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> IsSlotConflictExistsAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeSlotId = null)
        {
            var query = _context.AvailabilitySlots
                .Where(a => a.DoctorId == doctorId && a.DayOfWeek == dayOfWeek);

            if (excludeSlotId.HasValue)
            {
                query = query.Where(a => a.Id != excludeSlotId.Value);
            }

            return await query.AnyAsync(a =>
                (startTime >= a.StartTime && startTime < a.EndTime) ||
                (endTime > a.StartTime && endTime <= a.EndTime) ||
                (startTime <= a.StartTime && endTime >= a.EndTime)
            );
        }


        public async Task<bool> UpdateAsync(UpdateAvailabilitySlotVM model)
        {
            var slot = await _context.AvailabilitySlots.FindAsync(model.Id);
            if (slot == null) return false;

            bool isConflict = await IsSlotConflictExistsAsync(model.DoctorId, model.DayOfWeek, model.StartTime, model.EndTime, model.Id);
            if (isConflict)
            {
                throw new Exception("This updated time slot overlaps with an existing schedule for this doctor. Please adjust the times.");
            }

            slot.DoctorId = model.DoctorId;
            slot.DayOfWeek = model.DayOfWeek;
            slot.StartTime = model.StartTime;
            slot.EndTime = model.EndTime;
            slot.IsActive = model.IsActive;

            _context.AvailabilitySlots.Update(slot);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var slot = await _context.AvailabilitySlots.FindAsync(id);
            if (slot == null) return false;

            _context.AvailabilitySlots.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
