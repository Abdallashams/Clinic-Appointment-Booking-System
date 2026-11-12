namespace ClinicBookingSystem.ModelVM.Doctor
{
    public class UpdateDoctorVM
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "Consultation duration is required")]
        [Range(5, 120, ErrorMessage = "Duration must be between 5 and 120 minutes")]
        [Display(Name = "Consultation Duration (minutes)")]
        public int ConsultationDurationMinutes { get; set; } = 30;

        [Required(ErrorMessage = "Specialty is required")]
        [Display(Name = "Specialty")]
        public int SpecialtyId { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImage { get; set; }
    }
}
