using DrApp.Context.Entities;
using DrApp.Context.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DrApp.Context.YourNewFolderName
{
    public class DoctorAppDbContext : DbContext
    {
        public DoctorAppDbContext(DbContextOptions<DoctorAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailability { get; set; }   // ADD THIS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Doctor)
               .WithMany()
               .HasForeignKey(a => a.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorAvailability>()
               .HasOne(da => da.Doctor)
               .WithMany()
               .HasForeignKey(da => da.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}