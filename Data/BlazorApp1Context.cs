using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.Models.Entity;

namespace BlazorApp1.Data
{
    public class BlazorApp1Context : DbContext
    {
        public BlazorApp1Context (DbContextOptions<BlazorApp1Context> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Goods> Goods { get; set; } = default!;
        public DbSet<Store> Store { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<SubOrder> SubOrders { get; set; } = default!;
    }
}
