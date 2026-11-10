using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // HasRole (M:N)
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // the user MAY have a doctor profile
        public Doctor? DoctorProfile { get; set; }

        // appointments booked as a patient
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}