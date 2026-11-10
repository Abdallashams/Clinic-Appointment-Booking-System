using Microsoft.AspNetCore.Identity;

namespace ClinicBookingSystem.Models
{
    public class Role : IdentityRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // HasRole: Role (N) <-> (M) ApplicationUser
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
