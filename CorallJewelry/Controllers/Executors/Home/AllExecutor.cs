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
        private static BackendContext db { get; set; }

        public static class IndexExecutor
        {
            public static List<Product> GetProducts(string type)
            {
                
                db = Accessor.GetDbContext();

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
                db = Accessor.GetDbContext();

                var prod = db.Products.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefault();
                if (prod.Images.Count == 0)
                {
                    prod.Images.Add(new Image() { Name = "header-logo.png" });
                }
                return prod;
            }

            private static Contacts GetContact()
            {
                db = Accessor.GetDbContext();

                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static IndexModel GetModel()
            {
                db = Accessor.GetDbContext();

                IndexModel index = new IndexModel();
                index.Contacts = GetContact();
                index.Products = db.Products
                    .OrderByDescending(x => x.Id)
                    .Include(x => x.Images)
                    .Take(6)
                    .ToList();
                index.Products.ForEach(x => x.Images = new List<Image> { x.Images.OrderByDescending(x=>x.Id).FirstOrDefault() });
                
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
                db = Accessor.GetDbContext();

                var list = db.PriceLists.Include(a => a.Prices).ToList();
                return list;
            }
            private static Contacts GetContact()
            {
                db = Accessor.GetDbContext();

                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static PriceModel GetModel()
            {
                db = Accessor.GetDbContext();

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
                db = Accessor.GetDbContext();

                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            public static BaseFrontend GetModel()
            {
                db = Accessor.GetDbContext();

                BaseFrontend index = new BaseFrontend();
                index.Contacts = GetContact();
                return index;
            }
            public static void SendRequest(string contact, string message)
            {
                db = Accessor.GetDbContext();

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
                db = Accessor.GetDbContext();

                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
            private static List<Product> GetProducts(string type)
            {
                db = Accessor.GetDbContext();

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
                db = Accessor.GetDbContext();

                ProductsModel products = new ProductsModel();
                products.Products = GetProducts(type);
                products.Contacts = GetContact();
                return products;
            }
            private static Product GetProduct(int id)
            {
                db = Accessor.GetDbContext();

                var prod = db.Products.Where(x => x.Id == id).Include(x => x.Images).FirstOrDefault();

                if (prod.Images.Count == 0)
                {
                    prod.Images.Add(new Image() { Name = "header-logo.png" });
                }

                return prod;
            }
            public static ProductModel GetModel(int id)
            {
                db = Accessor.GetDbContext();

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
                db = Accessor.GetDbContext();

                CatalogModel catalogs = new CatalogModel();
                catalogs.Contacts = GetContact();
                catalogs.Catalogs = db.Catalogs.Include(x=>x.Category).ToList();
                return catalogs;
            }

            public static CatalogModel GetModel(int id, string name)
            {
                db = Accessor.GetDbContext();

                CatalogModel catalogs = new CatalogModel();
                catalogs.Contacts = GetContact();
                catalogs.Catalogs = db.Catalogs.Include(x => x.Category).ToList();
                catalogs.Items = db.Items.Where(x=>x.NameCategory == name && x.IdCatalog == id).Include(x=>x.Image).ToList();
                return catalogs;
            }

            private static Contacts GetContact()
            {
                db = Accessor.GetDbContext();

                var conts = db.Contacts.ToList();
                return conts.Count != 0 ? conts.Last() : new Contacts();
            }
        }
    }
}
