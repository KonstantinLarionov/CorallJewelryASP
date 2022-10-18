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
#if DEBUG
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_onlyChat;User=root;Password=root;");
#else
            optionsBuilder.UseMySql("Server=localhost;Database=u1810457_onlyChat;User=u1810457_admcorall;Password=sOq2e&032;");
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
