using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.DataAccess
{ 
    public partial class GuardianContext: DbContext
    {
        public GuardianContext()
        {
        }

        public GuardianContext(DbContextOptions<GuardianContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reason> Reasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reason>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReasonDescription).HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(30);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(30);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Address).IsRequired().HasMaxLength(100);
                entity.Property(u => u.City).IsRequired().HasMaxLength(100);
                entity.Property(u => u.State).IsRequired().HasMaxLength(30);
                entity.Property(u => u.Zip).IsRequired();
                entity.Property(u => u.AccessLevel).HasDefaultValue("User");
                entity.Property(r => r.AccountDate)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now()");
                entity.Property(u => u.AccountVerified).HasDefaultValue(false);
            });
            
            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired().HasMaxLength(30);
                entity.Property(h => h.Address).IsRequired().HasMaxLength(100);
                entity.Property(h => h.City).IsRequired().HasMaxLength(100);
                entity.Property(h => h.State).IsRequired().HasMaxLength(30);
                entity.Property(h => h.Zip).IsRequired();
                entity.Property(h => h.Phone).IsRequired().HasMaxLength(20);
                entity.Property(h => h.Website).IsRequired().HasMaxLength(100);
            });
            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.DateAdmittance)
                    .HasColumnType("timestamp");
                entity.Property(r => r.DateSubmitted)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now()");
                entity.HasOne(r => r.Hospital).WithMany(h => h.Reviews).HasForeignKey(r => r.HospitalId);
                entity.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId);
                entity.Property(r => r.MedicalStaffRating).IsRequired();
                entity.Property(r => r.ClericalStaffRating).IsRequired();
                entity.Property(r => r.FacilityRating).IsRequired();
                entity.HasOne(r => r.Reason);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
