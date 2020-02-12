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
        public FrontendContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=CorallJewelry;User=u0893_adminDB;Password=snRq40~6;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
