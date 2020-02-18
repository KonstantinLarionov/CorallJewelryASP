using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace afc_studio.Models.Objects
{
    public class User
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<Dialog> Dialogs { get; set; } = new List<Dialog>();
    }
}
