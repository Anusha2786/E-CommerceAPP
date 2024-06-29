using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Addrees
    {
        [Key]
        [Required]
        public int AddressId { get; set; }  // Primary key
        [Required]
        public string AddressName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Pincode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}

