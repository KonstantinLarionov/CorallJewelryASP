using ChatModule.Models.Chat.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModule.Models.Chat.Entitys
{
    public class ChatContext : DbContext
    {
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public ChatContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //prod
            //optionsBuilder.UseMySql("Server=localhost;Database=u1810457_onlyChat;User=u1810457_admcorall;Password=sOq2e&032;");
            //dev
#if DEBUG
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_coralljewelry;User=root;Password=root;Convert Zero Datetime=True");
#else
            optionsBuilder.UseMySql("Server=localhost;Database=u1810457_coralljewelry;User=u1810457_corall;Password=sOq2e&032;Convert Zero Datetime=True");
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}
