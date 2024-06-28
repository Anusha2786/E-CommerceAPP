using E_CommerceAPP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_CommerceAPP.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Categories entity
            modelBuilder.Entity<Categories>()
                .HasKey(c => c.Category_ID);    
             modelBuilder.Entity<Products>()
        .HasKey(p => p.Product_ID); // Set Product_ID as primary key

            // Configure Products entity
            modelBuilder.Entity<Products>()
                .HasKey(p => p.Product_ID);

            // Configure Reviews entity
            modelBuilder.Entity<Reviews>()
                .HasKey(r => r.Review_ID);

            // Configure Customer entity
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Customer_ID);

            modelBuilder.Entity<Categories>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.Category_ID);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Categories) // Product belongs to one Category
                .WithMany(c => c.Products) // Category has many Products
                .HasForeignKey(p => p.Category_ID); // Foreign key constraint

            // Configure relationships
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.Product_ID);

            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.Customer_ID);
            base.OnModelCreating(modelBuilder);
        }
    }
}
