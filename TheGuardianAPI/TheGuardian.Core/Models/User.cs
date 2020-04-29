using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGuardian.Core.Models
{
    public class User
    {
        public User()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int ZipCode { get; set; }

        public string AccessLevel { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
