using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.DataAccess
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HospitalId { get; set; }
        public int MedicalStaffRating { get; set; }
        public int ClericalStaffRating { get; set; }
        public int FacilityRating { get; set; }
        public double OverallRating { get { return (MedicalStaffRating + ClericalStaffRating + FacilityRating) / 3.0; } }
        public string WrittenFeedback { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateAdmittance { get; set; }
        public string Reason { get; set; }
        public string ReasonOther { get; set; }
        public User User { get; set; }
        public Hospital Hospital { get; set; }
    }
}
