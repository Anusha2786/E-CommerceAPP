using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_CommerceAPP.Models
{
    public class Address
    {
        [Key]
        public int AddressID { get; set; }

        [Required]
        [StringLength(200)]
        public string? Street { get; set; }

        [Required]
        [StringLength(100)]
        public string? City { get; set; }

        [Required]
        [StringLength(100)]
        public string? State { get; set; }

        [Required]
        [StringLength(100)]
        public string? Country { get; set; }

        [Required]
        [StringLength(20)]
        public string? Zipcode { get; set; }

        public int CustomeID {  get; set; }

        //public virtual Customer? Customer { get; set; }

    }
}
