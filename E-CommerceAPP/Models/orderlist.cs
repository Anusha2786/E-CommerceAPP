using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Orderlist
    {
        
        public int OrderlistId { get; set; }

       
        public string OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public int Quantity { get; set; }

        // Foreign keys
        public int PaymentId { get; set; }
        public int ShipmentId { get; set; }
        public int AddressId { get; set; }

        // Navigation properties
        public virtual Addrees Addrees { get; set; }  // One-to-one with Address

        public virtual ICollection<Payment> Payments { get; set; } // One-to-many with Payment

        public virtual Shipment Shipment { get; set; } // One-to-one with Shipment
    }
}
