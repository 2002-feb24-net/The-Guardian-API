using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.Core.Models
{
    public class Hospital
    {
        public Hospital()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int StreetNum { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public double AggMedicalStaffRating { get; set; }

        public double AggClericalStaffRating { get; set; }

        public double AggOverallRating { get; set; }

        public ICollection<Review> Reviews { get; set; }

    }
}
