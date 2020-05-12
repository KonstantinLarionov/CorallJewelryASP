using System;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorallJewelry.Models;

namespace CorallJewelry.Entitys
{
    public class BackendContext : DbContext
    {
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Category> Category  { get; set; }
        public DbSet<ItemCatalog> Items { get; set; }
        public BackendContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_coralljewelry2;User=u0959_admcorall2;Password=sOq2e&032;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
