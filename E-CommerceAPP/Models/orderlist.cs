using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPP.Models
{
    public class orderlist
    {
        [Required]
        public int orderid { get; set; }
        public string orderstatus { get; set; }
        public DateTime orderdate{ get; set; }
        public int Quantity { get; set; }
    


    }
}
