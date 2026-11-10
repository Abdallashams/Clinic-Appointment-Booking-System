namespace ClinicBookingSystem.ModelVM
{
    public class GetApplicationUserVM
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
