using E_CommerceAPP.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class Payment

    {
        [Key]
        [Required]
        public int paymentId { get; set; }
        [Required]
        public DateTime paymentdate { get; set; }
        [Required]
        public string? paymenttype { get; set; }
        [Required]
        public decimal? amount { get; set; }

        public string? paymentstatus { get; set; }
        // Navigation properties
        public virtual ICollection<Orderlist> Orderlists { get; set; }

    }
}
