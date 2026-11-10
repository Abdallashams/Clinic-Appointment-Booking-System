using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.ModelVM
{
    public class ApplicationUserVm
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@gmail\\.com$", ErrorMessage = "Email must be a valid Gmail address.")]
        public string? Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; } = null!;
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required.")]

        [Compare("Password", ErrorMessage = "Passwords do not match.")]

        public string? ConfirmPassword { get; set; }
        public bool? IsActive { get; set; }= true;
        public List<string>? Roles { get; set; } = new List<string>();
    }
}
