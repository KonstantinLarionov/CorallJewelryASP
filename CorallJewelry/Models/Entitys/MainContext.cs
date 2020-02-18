using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using afc_studio.Models.Objects.Connect;
using afc_studio.Models.Objects;

namespace afc_studio.Models.Entitys
{
    public class MainContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<User> Users { get; set; } 

        public MainContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=u0893839_chat;User=u0893_adminDB;Password=snRq40~6;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
