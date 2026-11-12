
namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IDoctorService
    {
        Task<bool> MakeDoctor(CreateDoctorVM createDoctorVM);
        Task<List<GetAllDoctorVM>> GetAll();
        Task<bool> DeleteDoctor(int id);
        Task<bool> ToggleActive(int id);
        Task<bool> UpdateDoctor(UpdateDoctorVM doctor);
        Task<GetAllDoctorVM?> GetByUserId(string userId);
        Task<GetAllDoctorVM?> GetById(int id);
    }
}
