namespace E_CommerceAPP.Models.Entities
{
    public class Reviews
    {
        public int Review_ID { get; set; }
        public int Product_Rating { get; set; }
        public string Comment { get; set; }
        public decimal Product_ID { get; set; }
        public int Customer_ID { get; set; }
    }
}
