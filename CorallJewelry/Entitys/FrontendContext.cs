using CorallJewelry.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Entitys
{
    public class FrontendContext : DbContext
    {
        public  DbSet<Contacts> Contacts { get; private set; }
        public  DbSet<Price> Prices { get; private set; }
        public  DbSet<PriceList> PriceLists { get; private set; }
        public DbSet<Product> Products { get; private set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Catalog> Catalogs { get; private set; }
        public DbSet<ItemCatalog> Items { get; set; }
        public FrontendContext(DbContextOptions options) : base(options)
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
