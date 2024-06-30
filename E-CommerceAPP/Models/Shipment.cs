using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Shipment
    {
        [Key]   
        [Required]
        public int shipmentId { get; set; }
        public DateTime shipmentdate { get; set; }
    }
}
