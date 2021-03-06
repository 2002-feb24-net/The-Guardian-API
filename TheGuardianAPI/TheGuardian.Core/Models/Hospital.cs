﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


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
        [Range(1.00, 5.00)]
        public double AggMedicalStaffRating { get; set; }
        [Range(1.00, 5.00)]
        public double AggClericalStaffRating { get; set; }
        [Range(1.00, 5.00)]
        public double AggFacilityRating { get; set; }
        [Range(1.00, 5.00)]
        public double AggOverallRating { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
