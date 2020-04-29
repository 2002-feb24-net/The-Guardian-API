using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.Api.Models
{
    public class GuardianContext: DbContext
    {
        public GuardianContext(DbContextOptions<GuardianContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.ZipCode).IsRequired();
            });
            
            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired();
                entity.Property(h => h.StreetNum).IsRequired();
                entity.Property(h => h.StreetName).IsRequired();
                entity.Property(h => h.City).IsRequired();
                entity.Property(h => h.State).IsRequired();
                entity.Property(h => h.Zip).IsRequired();
            });
            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.DateAdmittance)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
                entity.HasOne(r => r.Hospital).WithMany(h => h.Reviews).HasForeignKey(r => r.HospitalId);
                entity.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId);
                entity.Property(r => r.MedicalStaffRating).IsRequired();
                entity.Property(r => r.ClericalStaffRating).IsRequired();
                entity.Property(r => r.FacilityRating).IsRequired();
                entity.Property(r => r.Reason).IsRequired();
            });
        }
    }
}
