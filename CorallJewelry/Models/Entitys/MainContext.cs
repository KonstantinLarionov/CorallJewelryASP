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
#if DEBUG
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_chatcj;User=root;Password=root;");
#else
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_chatcj;User=u0959_admcorall2;Password=sOq2e&032;");
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
