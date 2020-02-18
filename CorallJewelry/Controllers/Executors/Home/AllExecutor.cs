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
            public static IndexModel GetModel()
            {
                IndexModel index = new IndexModel();
                index.Contacts = GetContact();
                index.Products = db.Products.OrderByDescending(x => x.Id).Include(x => x.Images).Take(6).ToList();
                return index;
            }
        }
        public static class PriceExecutor
        {
            private static List<PriceList> GetAllPriceLists()
            {
                var list = db.PriceLists.Include(a => a.Prices).ToList();
                return list;
            }
            private static Contacts GetContact()
            {
                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static PriceModel GetModel()
            {
                PriceModel priceModel = new PriceModel();
                priceModel.PriceLists = GetAllPriceLists();
                priceModel.Contacts = GetContact();
                return priceModel;
            }
        }
        public static class ContactExecutor
        {
            private static Contacts GetContact()
            {
                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static BaseFrontend GetModel()
            {
                BaseFrontend index = new BaseFrontend();
                index.Contacts = GetContact();
                return index;
            }
            public static void SendRequest(string contact, string message)
            {
                Request request = new Request();
                request.Contact = contact;
                request.Date = DateTime.Now;
                request.Message = message;
                db.Requests.Add(request);
                db.SaveChanges();
            }
        }
        public static class ProductsExecutor
        {
            private static Contacts GetContact()
            {
                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            private static List<Product> GetProducts(string type)
            {
               
                    var prod = db.Products.Where(x => x.Type == type).Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                    return prod;
                
            }
            public static ProductsModel GetModel(string type)
            {
                ProductsModel products = new ProductsModel();
                products.Products = GetProducts(type);
                products.Contacts = GetContact();
                return products;
            }
            private static Product GetProduct(int id)
            {
                var prod = db.Products.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefault();
                return prod;
            }
            public static ProductModel GetModel(int id)
            {
                ProductModel product = new ProductModel();
                product.Contacts = GetContact();
                product.Product = GetProduct(id);
                return product;
            }
        }
    }
}
