using System;
using System.Collections.Generic;


namespace TheGuardian.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public bool AccessLevel { get; set; }

        public bool AccountVerified { get; set; }

        public DateTime AccountDate { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
