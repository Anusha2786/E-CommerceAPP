using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Delivery
    {
        [Key]
        [Required]  
        public int DeliveryId { get; set; } 
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime Estimateddeliver {  get; set; } 
        [Required]
        public DateTime Deliverydate { get; set; }
        [Required]
        public int  shipmentid { get; set; }
    }
}
