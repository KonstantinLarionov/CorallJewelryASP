using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public enum TypeUser
    { 
        Admin,
        Guest,
        Operator
    }
    public class User
    {
        public int Id { get; set; }
        public TypeUser Type { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastSession { get; set; }
    }
}
