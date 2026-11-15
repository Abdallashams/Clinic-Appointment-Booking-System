namespace ClinicBookingSystem.ModelVM.Appointment
{
    public class GetAppointmentVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public string? DoctorName { get; set; }

        [Required]
        public string? SpecialtyName { get; set; }

        [Required]
        public string? PatientId { get; set; }

        [Required]
        public string? PatientName { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
