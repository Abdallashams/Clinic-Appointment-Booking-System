using ClinicBookingSystem.ModelVM.Appointment;

namespace ClinicBookingSystem.Services.Abstraction
{
    public interface IAppointmentService
    {
        Task<List<GetAppointmentVM>> GetAllAppointmentsAsync();                          
        Task<List<GetAppointmentVM>> GetPatientAppointmentsAsync(string patientId);     
        Task<List<GetAppointmentVM>> GetDoctorAppointmentsAsync(int doctorId, DateTime date); 
        Task<GetAppointmentVM?> GetByIdAsync(int id);                                   

      
        Task<GetAppointmentVM> BookAppointmentAsync(CreateAppointmentVM appointmentVM, string patientId);
        Task<bool> RescheduleAsync(UpdateAppointmentVM appointmentVM);                 
        Task<bool> ConfirmAsync(int id);    
        Task<bool> CancelAsync(int id);     
        Task<bool> CompleteAsync(int id);   
        public Task<List<TimeSpan>> GetAvailableTimesAsync(int doctorId, DateTime date);
        Task<DoctorBookingInfoVM?> GetDoctorBookingInfoAsync(int doctorId);



    }
}
