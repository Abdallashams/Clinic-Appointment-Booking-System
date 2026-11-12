namespace ClinicBookingSystem.ModelVM.AvailabilitySlot
{
    public class CreateAvailabilitySlotVM
    {

        [Required(ErrorMessage = "Doctor is required")]
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


    }
}
