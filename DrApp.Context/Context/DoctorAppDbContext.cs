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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });
        }
    }
}
