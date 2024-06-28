using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Payment
    {
        [Required]
        public int paymentId { get; set; }
        public DateTime paymentdate { get; set; }
        public string? paymenttype { get; set; }
        public decimal? amount { get; set; }
        public string? paymentstatus { get; set; }
     
    }
}
