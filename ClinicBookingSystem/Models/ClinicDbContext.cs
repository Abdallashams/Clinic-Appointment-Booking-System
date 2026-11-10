using ClinicBookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClinicBookingSystem.Models
{
    public class ClinicDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Specialty> Specialties => Set<Specialty>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<AvailabilitySlot> AvailabilitySlots => Set<AvailabilitySlot>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------- Role ----------
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Roles");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ---------- ApplicationUser ----------
            modelBuilder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(x => x.FullName).IsRequired().HasMaxLength(150);
                e.Property(x => x.Email).IsRequired().HasMaxLength(256);
                e.Property(x => x.PhoneNumber).HasMaxLength(20);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.HasIndex(x => x.Email).IsUnique();

                // HasRole: ApplicationUser (M) <-> (N) Role  via UserRoles
                e.HasMany(u => u.Roles)
                 .WithMany(r => r.Users)
                 .UsingEntity<UserRole>(
                     j => j.HasOne(ur => ur.Role)
                           .WithMany(r => r.UserRoles)
                           .HasForeignKey(ur => ur.RoleId)
                           .OnDelete(DeleteBehavior.Cascade),
                     j => j.HasOne(ur => ur.User)
                           .WithMany(u => u.UserRoles)
                           .HasForeignKey(ur => ur.UserId)
                           .OnDelete(DeleteBehavior.Cascade),
                     j =>
                     {
                         j.ToTable("UserRoles");
                         //Compsit Key for UserRole
                         j.HasKey(ur => new { ur.UserId, ur.RoleId });
                     });
            });

           


        }
    }
}


