namespace ClinicBookingSystem.ModelVM.Appointment
{
    public class DoctorBookingInfoVM
    {
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? SpecialtyName { get; set; }
        public string? Bio { get; set; }
        public int ConsultationDurationMinutes { get; set; }
        public List<DoctorScheduleVM> Schedule { get; set; } = new List<DoctorScheduleVM>();
    }


}
