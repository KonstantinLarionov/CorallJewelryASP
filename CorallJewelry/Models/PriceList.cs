using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public class PriceList 
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public List<Price> Prices { get; set; }
    }
}
