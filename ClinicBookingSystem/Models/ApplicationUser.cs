using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;


        // the user MAY have a doctor profile
        public Doctor? DoctorProfile { get; set; }

        // appointments booked as a patient
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}