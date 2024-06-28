namespace E_CommerceAPP.Models.Entities
{
    public class Reviews
    {
        public int Review_ID { get; set; }
        public int Product_Rating { get; set; }
        public string Comment { get; set; }
        public int Product_ID { get; set; } // Foreign key for Product

        // Navigation property to Product (related entity)
        public Products Product { get; set; }

        public int Customer_ID { get; set; } // Foreign key for Customer

        // Navigation property to Customer (related entity)
        public Customer Customer { get; set; }
    }
}
