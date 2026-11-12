using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicBookingSystem.Models
{
    public class Doctor
    {

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

        [Display(Name = "Profile Image")]
        public string? ProfileImageUrl { get; set; }

        [Required(ErrorMessage = "Consultation duration is required")]
        [Range(5, 120, ErrorMessage = "Duration must be between 5 and 120 minutes")]
        [Display(Name = "Consultation Duration (minutes)")]
        public int ConsultationDurationMinutes { get; set; } = 30;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Foreign key to Specialty
        [Required(ErrorMessage = "Specialty is required")]
        [Display(Name = "Specialty")]
        public int SpecialtyId { get; set; }

        // Foreign key to ApplicationUser (optional – doctor may have a login)
        public string? ApplicationUserId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(SpecialtyId))]
        public Specialty? Specialty { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        public List<AvailabilitySlot> Availabilities { get; set; } = new List<AvailabilitySlot>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
