using E_CommerceAPP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_CommerceAPP.Data
{
    public class ecommercedbcontext: DbContext
    {
        public ecommercedbcontext(DbContextOptions<ecommercedbcontext>options) : base(options)
        {
            
        }
        public DbSet<Products>Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Reviews> Reviews{ get; set; }

}
}
