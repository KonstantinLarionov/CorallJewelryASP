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
                products.ForEach(x =>
                {
                    if (x.Images.Count == 0)
                    {
                        x.Images.Add(new Image() { Name = "header-logo.png" });
                    }
                });
                return products;
            }

            public static Product GetProduct(int id)
            {
                var prod = db.Products.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefault();
                if (prod.Images.Count == 0)
                {
                    prod.Images.Add(new Image() { Name = "header-logo.png" });
                }
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
                index.Products.ForEach(x =>
                {
                    if (x.Images.Count == 0)
                    {
                        x.Images.Add(new Image() { Name = "header-logo.png" });
                    }
                });
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
                prod.ForEach(x =>
                {
                    if (x.Images.Count == 0)
                    {
                        x.Images.Add(new Image() { Name = "header-logo.png" });
                    }
                });
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

                if (prod.Images.Count == 0)
                {
                    prod.Images.Add(new Image() { Name = "header-logo.png" });
                }

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
        public static class CatalogsExecutor
        {
            public static CatalogModel GetModel()
            {
                CatalogModel catalogs = new CatalogModel();
                catalogs.Contacts = GetContact();
                catalogs.Catalogs = db.Catalogs.Include(x=>x.Category).ToList();
                return catalogs;
            }

            public static CatalogModel GetModel(int id, string name)
            {
                CatalogModel catalogs = new CatalogModel();
                catalogs.Contacts = GetContact();
                catalogs.Catalogs = db.Catalogs.Include(x => x.Category).ToList();
                catalogs.Catalogs.Where(x => x.Id == id).FirstOrDefault().Items = db.ItemsCatalog.Where(x=>x.NameCategory == name && x.IdCatalog == id).ToList();
                return catalogs;
            }

            private static Contacts GetContact()
            {
                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
        }
    }
}
