using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.Models
{
    public class Specialty
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Specialty name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Specialty Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        // Navigation property
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
