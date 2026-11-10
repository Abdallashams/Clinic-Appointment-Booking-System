using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public string PatientId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }

        [ForeignKey(nameof(PatientId))]
        public ApplicationUser? Patient { get; set; }
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
