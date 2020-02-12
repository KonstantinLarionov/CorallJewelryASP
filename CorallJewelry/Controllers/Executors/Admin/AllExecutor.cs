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
                var products = db.Products.Where(x=>x.Type == type).OrderByDescending(x=>x.Id).ToList();
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
    }
}
