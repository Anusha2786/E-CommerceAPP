using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        [Required]
        public int ShipmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Status { get; set; }

        [Required]
        public DateTime EstimatedDelivery { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [ForeignKey("ShipmentID")]
        public virtual Shipment? Shipment { get; set; }
    }
}
