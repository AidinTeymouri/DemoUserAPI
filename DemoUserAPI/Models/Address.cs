using System.ComponentModel.DataAnnotations;

namespace DemoUserAPI.Models
{
    public class Address
    {
        [Key]
        public long UserID { get; set; }

        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public User User { get; set; }
    }
}
