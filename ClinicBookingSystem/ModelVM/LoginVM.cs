
namespace ClinicBookingSystem.ModelVM
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@gmail\\.com$", ErrorMessage = "Email must be a valid Gmail address.")]
        public string? Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

    }
}
