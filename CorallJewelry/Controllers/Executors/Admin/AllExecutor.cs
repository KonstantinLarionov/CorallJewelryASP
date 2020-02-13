using CorallJewelry.Entitys;
using CorallJewelry.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Controllers.Executors.Admin
{
    public static class AllExecutors
    {
        private static BackendContext db { get; set; } = new BackendContext(new DbContextOptions<BackendContext>());
        public static class LoginExecutor
        {
            public static bool OnAuth(string login, string password, HttpContext httpContext)
            {
                var admin = db.Users.Where(u => u.Login == login && u.Password == password && u.Type == TypeUser.Admin).ToList();
                if (admin.Count != 0)
                {
                    httpContext.Session.SetString("login", login);
                    httpContext.Session.SetString("password", password);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static class ProductsExecutor
        {
            public static List<Product> GetProducts(string type)
            {
                List<Product> products = new List<Product>();
                if (type == "all")
                {
                    products = db.Products.Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                }
                else {
                    products = db.Products.Where(x => x.Type == type).Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                }
             
                return products;
            }

            public static void AddProducts(List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type)
            {
                List<Image> imagesAdd = LoadImage(images);
               
                Product product = new Product()
                {
                    Name = name,
                    About = about,
                    Price = price,
                    Weight = weight,
                    Stone = stone,
                    Metall = metall,
                    Type = type,
                    Images = imagesAdd
                };
                db.Products.Add(product);
                db.SaveChanges();
            }

            public static void DeleteProduct(int id)
            {
                var product = db.Products.Where(x=> x.Id == id).FirstOrDefault();
                db.Products.Remove(product);
                db.SaveChanges();
            }

            public static void EditProduct(int id, List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type)
            {
                var imagesAll = LoadImage(images);
                var product = db.Products.Where(x => x.Id == id).FirstOrDefault();
                product.About = about;
                product.Images = imagesAll;
                product.Metall = metall;
                product.Name = name;
                product.Price = price;
                product.Stone = stone;
                product.Type = type;
                product.Weight = weight;
                db.SaveChanges();
            }

            private static List<Image> LoadImage(List<IFormFile> images)
            {
                List<Image> imagesAdd = new List<Image>();
                if (images != null && images.Count() != 0)
                {
                    foreach (var file in images)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", file.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        imagesAdd.Add(new Image() { Name = file.FileName });
                    }
                }
                return imagesAdd;
            }
        }
        public static class PriceExecutor 
        {
            public static List<PriceList> GetAllPriceLists()
            {
                var list = db.PriceLists.Include(a => a.Prices).ToList();
                return list;
            }

            public static void AddPrice(int idList, string name, string money)
            {
                db.PriceLists.Where(x=>x.Id == idList).FirstOrDefault().Prices.Add(new Price() { Money = money, Name = name });
                db.SaveChanges();
            }

            public static void EditPrice(int id, string name, string money) 
            {
                var price = db.Prices.Where(x => x.Id == id).FirstOrDefault();
                price.Money = money;
                price.Name = name;
                db.SaveChanges();
            }

            public static void DeletePrice(int id)
            { 
                var price = db.Prices.Where(x => x.Id == id).FirstOrDefault();
                db.Prices.Remove(price);
                db.SaveChanges();
            }

            public static void DeletePriceList(int id)
            {
                var price = db.PriceLists.Where(x => x.Id == id).FirstOrDefault();
                db.PriceLists.Remove(price);
                db.SaveChanges();
            }

            public static void CreateList(string category)
            {
                db.PriceLists.Add(new PriceList() { Category = category }) ;
                db.SaveChanges();
            }
        }
        public static class RequestExecutor 
        {
            public static List<Request> GetRequest()
            {
                var request = db.Requests.ToList();
                return request;
            }

            public static void DeleteRequest(int id)
            {
                var request = db.Requests.Where(x => x.Id == id).FirstOrDefault();
                db.Requests.Remove(request);
                db.SaveChanges();
            }
        }
        public static class ContactExecutor
        {
            public static Contacts GetContact()
            {
                var contacts = db.Contacts.ToList();
                if (contacts.Count != 0)
                {
                    return contacts.OrderByDescending(x => x.Id).FirstOrDefault();
                }
                else 
                {
                    db.Contacts.Add(new Contacts() { AddressStreet = "", AddressTown = "", Email = "", Inst = "", OK = "", Phone = "", VK = "" });
                    db.SaveChanges();

                    return new Contacts() { Inst = "", OK = "", Phone = "", VK = "", Email = "", AddressTown = "", AddressStreet = "" };
                }

               
            }
            public static void EditContact( string email, string phone, string vk, string ok, string inst, string addressTown, string street, int id = 1)
            {
               
                    var contact = db.Contacts.Where(x => x.Id == id).FirstOrDefault();

                    contact = db.Contacts.Where(x => x.Id == id).FirstOrDefault();
                    contact.Inst = inst;
                    contact.OK = ok;
                    contact.Phone = phone;
                    contact.VK = vk;
                    contact.Email = email;
                    contact.AddressTown = addressTown;
                    contact.AddressStreet = street;
                    db.SaveChanges();
               
            }
        }
    }
}
