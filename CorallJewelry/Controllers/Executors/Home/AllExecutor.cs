using CorallJewelry.Entitys;
using CorallJewelry.Models;
using CorallJewelry.Models.FrontModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Controllers.Executors.Home
{
    public static class AllExecutors
    {
        private static FrontendContext db { get; set; } = new FrontendContext(new DbContextOptions<FrontendContext>());
        public static class IndexExecutor
        {
            public static List<Product> GetProducts(string type)
            {
                var products = db.Products.Where(x => x.Type == type).Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                return products;
            }

            public static Product GetProduct(int id)
            {
                var prod = db.Products.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefault();
                return prod;
            }

            private static Contacts GetContact()
            {
                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static IndexModel GetIndex()
            {
                IndexModel index = new IndexModel();
                index.Contacts = GetContact();
                index.Products = db.Products.OrderByDescending(x => x.Id).Include(x=>x.Images).Take(6).ToList();
                return index;
            }
        }
        public static class PriceExecutor 
        {
            public static List<PriceList> GetAllPriceLists()
            {
                var list = db.PriceLists.Include(a => a.Prices).ToList();
                return list;
            }
        }
        public static class RequestExecutor 
        {
            public static void SendRequest(string cont, string text)
            {
                db.Requests.Add(new Request() { Contact = cont, Message = text, Date = DateTime.Now }) ;
                db.SaveChanges();
            }
        }
        public static class ContactExecutor
        {
            public static Contacts GetContact()
            {
                var contacts = db.Contacts.OrderByDescending(x => x.Id).FirstOrDefault();
                return contacts;
            }
        }
    }
}
