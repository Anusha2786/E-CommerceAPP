using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Addrees
    {
        [Key]
        public int AddressId { get; set; }  // Primary key

        public string AddressName { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}

