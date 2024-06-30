using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E_CommerceAPP.Models;

namespace E_CommerceAPP.Data
{
    public class E_CommerceAPPContext : DbContext
    {
        public E_CommerceAPPContext (DbContextOptions<E_CommerceAPPContext> options)
            : base(options)
        {
        }

        public DbSet<E_CommerceAPP.Models.Customer> Customer { get; set; } = default!;
        public DbSet<E_CommerceAPP.Models.Address> Address { get; set; } = default!;
        public DbSet<E_CommerceAPP.Models.Delivery> Delivery { get; set; } = default!;
        public DbSet<E_CommerceAPP.Models.Shipment> Shipment { get; set; } = default!;
    }
}
