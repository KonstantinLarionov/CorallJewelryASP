using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public double Price { get; set; }
        public string Weight { get; set; }
        public string Stone { get; set; }
        public string Metall { get; set; }
        public List<string> Images { get; set; }
    }
}
