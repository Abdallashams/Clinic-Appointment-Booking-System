using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicBookingSystem.Models
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Day of week is required")]
        [Display(Name = "Day of Week")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }
    
     }
}
