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
            optionsBuilder.UseMySql("Server=localhost;Database=u0959678_onlyChat;User=root;Password=root;");
            //optionsBuilder.UseMySql("Server=localhost;Database=u0959678_onlyChat;User=u0959_admcorall;Password=sOq2e&032;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
