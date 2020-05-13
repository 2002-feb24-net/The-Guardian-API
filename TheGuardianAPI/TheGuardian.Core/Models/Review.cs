using System;
using System.ComponentModel.DataAnnotations;


namespace TheGuardian.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HospitalId { get; set; }
        [Range(1, 5)]
        public int MedicalStaffRating { get; set; }
        [Range(1, 5)]
        public int ClericalStaffRating { get; set; }
        [Range(1, 5)]
        public int FacilityRating { get; set; }
        [Range(1.00, 5.00)]
        public double OverallRating { get; set; }
        public string WrittenFeedback { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateAdmittance { get; set; }
        public string Reason { get; set; }
        public string ReasonOther { get; set; }
    }
}
