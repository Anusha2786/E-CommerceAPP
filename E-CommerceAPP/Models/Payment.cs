namespace E_CommerceAPP.Models
{
    public class Payment
    {
        public int paymentid { get; set; }
        public DateTime paymentdate { get; set; }
        public string? paymenttype { get; set; }
        public int paymentstatus { get; set; }
    }
}
