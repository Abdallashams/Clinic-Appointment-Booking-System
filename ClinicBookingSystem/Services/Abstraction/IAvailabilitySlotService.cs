using ClinicBookingSystem.ModelVM.AvailabilitySlot;

namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IAvailabilitySlotService
    {

        Task<IEnumerable<GetAvailabilitySlotVM>> GetAllAsync();
        Task<IEnumerable<GetAvailabilitySlotVM>> GetByDoctorIdAsync(int doctorId);
        Task<GetAvailabilitySlotVM?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateAvailabilitySlotVM model);
        Task<bool> DeleteAsync(int id);

        Task<bool> UpdateAsync(UpdateAvailabilitySlotVM model);

        Task<bool> IsSlotConflictExistsAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeSlotId = null);


    }
}
