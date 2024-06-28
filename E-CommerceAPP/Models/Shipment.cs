using System;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        //Navigation property to Delivery
        public virtual ICollection<Delivery> Deliveries { get; set; } = [];
    }
}
