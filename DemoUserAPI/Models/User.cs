using System;
using System.ComponentModel.DataAnnotations;

namespace DemoUserAPI.Models
{
    public class User
    {
        [Key]
        public long ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long IdentityNumber { get; set; }

        public Address Address { get; set; }
    }
}
