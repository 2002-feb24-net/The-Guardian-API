using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.Core.Models
{
    public class Hospital
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public string Phone { get; set; }

        public string Website { get; set; }

        public double AggMedicalStaffRating { get; set; }

        public double AggClericalStaffRating { get; set; }

        public double AggFacilityRating { get; set; }

        public double AggOverallRating { get { return (AggMedicalStaffRating + AggClericalStaffRating + AggFacilityRating) / 3.0; } }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public void UpdateAggregateRatings()
        {
            double aggMedRating = 0, aggCleRating = 0, aggFacRating = 0;
            foreach (var review in Reviews)
            {
                aggMedRating += review.MedicalStaffRating;
                aggCleRating += review.ClericalStaffRating;
                aggFacRating += review.FacilityRating;
            }
            AggMedicalStaffRating = aggMedRating / Reviews.Count;
            AggClericalStaffRating = aggCleRating / Reviews.Count;
            AggFacilityRating = aggFacRating / Reviews.Count;
        }
    }
}
