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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(20);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(35);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(16);
                entity.Property(u => u.Address).IsRequired().HasMaxLength(35);
                entity.Property(u => u.City).IsRequired().HasMaxLength(25);
                entity.Property(u => u.State).IsRequired().HasMaxLength(2);
                entity.Property(u => u.Zip).IsRequired();
                entity.Property(u => u.AccessLevel).HasDefaultValue(false); // False => User, True => Admin
                entity.Property(r => r.AccountDate).HasColumnType("timestamp").HasDefaultValueSql("now()");
                entity.Property(u => u.AccountVerified).HasDefaultValue(false);
                entity.HasData(
                    new User
                    {
                        Id = 1, 
                        FirstName = "Super",
                        LastName = "Admin",
                        Email = "superadmin@gmail.com",
                        Password = "R3vTra1n1ng",
                        Address = "1001 S Center St",
                        City = "Arlington",
                        State = "TX",
                        Zip = 76010,
                        AccessLevel = true,
                        AccountVerified = true,
                        Reviews = new List<Review>() 
                    });
            });
            
            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).IsRequired().HasMaxLength(80);
                entity.Property(h => h.Address).IsRequired().HasMaxLength(35);
                entity.Property(h => h.City).IsRequired().HasMaxLength(25);
                entity.Property(h => h.State).IsRequired().HasMaxLength(2);
                entity.Property(h => h.Zip).IsRequired();
                entity.Property(h => h.Phone).IsRequired().HasMaxLength(15);
                entity.Property(h => h.Website).IsRequired().HasMaxLength(100);
                entity.HasData( 
                    new Hospital
                    {
                        Id = 1,
                        Name = "Baylor Scott & White Heart and Vascular Hospital",
                        Address = "621 North Hall Street",
                        City = "Dallas",
                        State = "TX",
                        Zip = 75226,
                        Phone = "(214) 820-0600",
                        Website = "http://www.baylorhearthospital.com/handler.cfm?event=practice,main",
                        Reviews = new List<Review>()
                    }
                );
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.UserId).IsRequired();
                entity.Property(r => r.HospitalId).IsRequired();
                entity.HasOne(r => r.User).WithMany(r => r.Reviews).HasForeignKey(r => r.UserId);
                entity.HasOne(r => r.Hospital).WithMany(r => r.Reviews).HasForeignKey(r => r.HospitalId);
                entity.Property(r => r.DateAdmittance).IsRequired().HasColumnType("timestamp");
                entity.Property(r => r.DateSubmitted).HasColumnType("timestamp").HasDefaultValueSql("now()");
                entity.Property(r => r.MedicalStaffRating).IsRequired();
                entity.Property(r => r.ClericalStaffRating).IsRequired();
                entity.Property(r => r.FacilityRating).IsRequired();
                entity.Property(r => r.WrittenFeedback).IsRequired().HasMaxLength(500);
                entity.Property(r => r.Reason).IsRequired().HasMaxLength(12);
                entity.Property(r => r.ReasonOther).HasMaxLength(50);
                /*entity.HasData(
                    new Review
                    {
                        Id = 1,
                        UserId = 1,
                        HospitalId = 1,
                        DateAdmittance = DateTime.Now,
                        MedicalStaffRating = 5,
                        ClericalStaffRating = 5,
                        FacilityRating = 5,
                        WrittenFeedback = "Greatest hospital on the planet. Five stars. Totally not biased.",
                        Reason = "Surgery"
                    });*/
            });
            Console.WriteLine("Well hello there!");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
