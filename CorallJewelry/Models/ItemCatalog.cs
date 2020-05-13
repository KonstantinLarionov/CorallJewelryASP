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
        public string Price { get; set; }
        public List<Image> Image { get; set; }
        public string NameCategory { get; set; }
        public int IdCatalog { get; set; }
        public string Video { get; set; }
    }
}
