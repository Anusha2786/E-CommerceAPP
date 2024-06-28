using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; } 
        public string Status { get; set; }
        public DateTime Estimateddeliver {  get; set; } 
        public DateTime Deliverydate { get; set; }
        public int  shipmentid { get; set; }
    }
}
