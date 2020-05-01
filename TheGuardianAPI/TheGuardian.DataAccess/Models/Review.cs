using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.DataAccess
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public double MedicalStaffRating { get; set; }

        public double ClericalStaffRating { get; set; }

        public double FacilityRating { get; set; }

        public double OverallRating { get; set; }

        public string WrittenFeedback { get; set; }

        public DateTime DateSubmitted { get; set; }

        public DateTime DateAdmittance { get; set; }

        public int ReasonId { get; set; }

        public Reason Reason { get; set; }

        public User User { get; set; }

        public Hospital Hospital { get; set; }
    }
}
