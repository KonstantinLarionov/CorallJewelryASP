using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models
{
    public enum TypeProduct
    {
        Кольца, Цепи, Кресты, Запонки, Брелки, Браслеты, Значки, Печатки, Подвески, Серьги
    }
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public double Price { get; set; }
        public string Weight { get; set; }
        public string Stone { get; set; }
        public string Metall { get; set; }
        public string Type { get; set; }
        public List<Image> Images { get; set; }
    }
}
