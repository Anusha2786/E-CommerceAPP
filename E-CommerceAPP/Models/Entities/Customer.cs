namespace E_CommerceAPP.Models.Entities
{
    public class Customer
    {
        public int Customer_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public long Contact_Number { get; set; }
        public string Email_ID { get; set; }
        public string Password { get; set; }
        public string Confirm_Password { get; set; }
        public ICollection<Products> Products { get; set; } // Navigation property if needed

        // Navigation property for Reviews
        public ICollection<Reviews> Reviews { get; set; }
    }
}
