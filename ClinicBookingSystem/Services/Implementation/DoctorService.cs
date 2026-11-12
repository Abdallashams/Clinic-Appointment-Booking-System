
using FinalProject.BLL.Helper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicBookingSystem.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly ClinicDbContext clinicDbContext;

        public DoctorService(ClinicDbContext clinicDbContext)
        {
            this.clinicDbContext = clinicDbContext;
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            var doctor = await clinicDbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return false;
            clinicDbContext.Doctors.Remove(doctor);
            clinicDbContext.SaveChanges();
            return true;
        }

        public async Task<List<GetAllDoctorVM>> GetAll()
        {
            var result = await clinicDbContext.Doctors.Select(d => new GetAllDoctorVM
            {
                Id = d.Id,
                FullName = d.FullName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                Bio = d.Bio,
                ProfileImageUrl = d.ProfileImageUrl,
                ConsultationDurationMinutes = d.ConsultationDurationMinutes,
                IsActive = d.IsActive,
                SpecialtyName = d.Specialty.Name
            }).ToListAsync();

          return result;


        }

        public async Task<GetAllDoctorVM?> GetById(int id)
        {
            var doctor = await clinicDbContext.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return null;
            return new GetAllDoctorVM
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                Bio = doctor.Bio,
                ProfileImageUrl = doctor.ProfileImageUrl,
                ConsultationDurationMinutes = doctor.ConsultationDurationMinutes,
                IsActive = doctor.IsActive,
                SpecialtyName = doctor.Specialty?.Name
            };
        }

        public async Task<GetAllDoctorVM?> GetByUserId(string userId)
        {
            var doctor = await clinicDbContext.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
            if (doctor == null)
                return null;
            return new GetAllDoctorVM
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                Bio = doctor.Bio,
                ProfileImageUrl = doctor.ProfileImageUrl,
                ConsultationDurationMinutes = doctor.ConsultationDurationMinutes,
                IsActive = doctor.IsActive,
                SpecialtyName = doctor.Specialty?.Name
            };
        }

        public async Task<bool> MakeDoctor(CreateDoctorVM createDoctorVM)
        {
            var Doctor = new Doctor
            {
                FullName = createDoctorVM.FullName,
                Email = createDoctorVM.Email,
                PhoneNumber = createDoctorVM.PhoneNumber,
                Bio = createDoctorVM.Bio,
                ConsultationDurationMinutes = createDoctorVM.ConsultationDurationMinutes,
                SpecialtyId = createDoctorVM.SpecialtyId,
                ProfileImageUrl = Upload.UploadFile("Files", createDoctorVM.ProfileImage)

            };
            var result = await clinicDbContext.Doctors.AddAsync(Doctor);
            if (result != null)
            {
                await clinicDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ToggleActive(int id)
        {
            var doctor = await clinicDbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
                return false;
            doctor.IsActive = !doctor.IsActive;
            clinicDbContext.SaveChanges();
            return true;
        }

        public async  Task<bool> UpdateDoctor(UpdateDoctorVM doctor)
        {
            var existingDoctor = await clinicDbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctor.Id);
            if (existingDoctor == null)
                return false;
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Email = doctor.Email;
            existingDoctor.PhoneNumber = doctor.PhoneNumber;
            existingDoctor.Bio = doctor.Bio;
            existingDoctor.ConsultationDurationMinutes = doctor.ConsultationDurationMinutes;
            existingDoctor.SpecialtyId = doctor.SpecialtyId;
            if (doctor.ProfileImage != null)
            {
                existingDoctor.ProfileImageUrl = Upload.UploadFile("Files", doctor.ProfileImage);
            }
            clinicDbContext.SaveChanges();
            return true;
        }
    }
}
