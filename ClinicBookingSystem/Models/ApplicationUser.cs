using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Doctor? Doctor { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}