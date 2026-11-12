namespace ClinicBookingSystem.ModelVM.Doctor
{
    public class GetAllDoctorVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Required]
        public int ConsultationDurationMinutes { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string? SpecialtyName { get; set; }
        public int SpecialtyId { get; set; }
        
    }
}
