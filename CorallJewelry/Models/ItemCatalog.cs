using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public class ItemCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public string About { get; set; }
        public double Price { get; set; }
        public Image Image { get; set; }
    }
}
