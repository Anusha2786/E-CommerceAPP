using E_CommerceAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceAPP.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
        public DbSet<Orderlist> orderlists { get; set; }
        public DbSet<Payment> paymentlists { get; set; }
        public DbSet<Delivery> deliverylists { get; set; }
        public DbSet<Addrees> addrees { get; set; }
        public DbSet<Shipment> shipment { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)


        {
            modelBuilder.Entity<Orderlist>()
                  .HasOne(o => o.Addrees)  
                  .WithOne()                
                  .HasForeignKey<Orderlist>(o => o.AddressId);

            
            modelBuilder.Entity<Orderlist>()
                .HasMany(o => o.Payments)
                .WithOne()
                .HasForeignKey(p => p.paymentId);








        }

    }
}
