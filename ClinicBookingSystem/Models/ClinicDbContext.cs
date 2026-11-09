using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Models
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }

    }
}
