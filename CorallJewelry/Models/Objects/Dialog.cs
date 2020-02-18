using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace afc_studio.Models.Objects
{
    public class Dialog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
