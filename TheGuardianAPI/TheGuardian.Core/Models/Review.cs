using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.Core.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int HospitalId { get; set; }

        public double MedicalStaffRating { get; set; }

        public double ClericalStaffRating { get; set; }

        public double FacilityRating { get; set; }

        public double OverallRating { get; set; }

        public string WrittenFeedback { get; set; }

        public int ReasonId { get; set; }

        public Reason Reason { get; set; }

        public DateTime DateSubmitted { get; set; }

        public DateTime DateAdmittance { get; set; }
        
        public User User { get; set; }

        public Hospital Hospital { get; set; }
    }
}
