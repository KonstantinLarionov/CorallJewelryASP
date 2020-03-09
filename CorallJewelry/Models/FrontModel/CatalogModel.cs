using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models.FrontModel
{
    public class CatalogModel : BaseFrontend
    {
        public List<Catalog> Catalogs { get; set; }
        public List<ItemCatalog> Items { get; set; }
    }
}
