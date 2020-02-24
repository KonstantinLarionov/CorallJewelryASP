using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public class Category
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public List<ItemCatalog> Items { get; set; }
        public List<Category> Categories { get; set; }
    }
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public Image Preview { get; set; }
        public List<Category> Category { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
