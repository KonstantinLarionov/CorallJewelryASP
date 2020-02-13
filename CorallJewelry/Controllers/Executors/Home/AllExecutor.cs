using CorallJewelry.Entitys;
using CorallJewelry.Models;
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
        public static class ProductsExecutor
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
