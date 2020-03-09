using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModule.Models.Chat.Objects
{
    public enum Role
    {
        User, Admin
    }
    public class User
    {
        public int Id { get; set; }

        public string UserIdentity { get; set; }
        public string IP { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public DateTime Date { get; set; }
    }
}
