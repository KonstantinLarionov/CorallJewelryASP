using afc_studio.Models.Entitys;
using afc_studio.Models.Objects;
using CorallJewelry.Entitys;
using CorallJewelry.Models;
using CorallJewelry.Views.Home;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CorallJewelry.Controllers.Executors.Admin
{
    public class AllExecutors
    {
        private static BackendContext db { get; set; }

        private static MainContext chat = new MainContext(new DbContextOptions<MainContext>());
        public static class LoginExecutor
        {
            public static bool OnAuth(string login, string password, HttpContext httpContext)
            {
                db = Accessor.GetDbContext();

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
        public class ProductsExecutor
        {
            public static List<Product> GetProducts(string type)
            {
                db = Accessor.GetDbContext();

                List<Product> products = new List<Product>();
                if (type == "all")
                {
                    products = db.Products.Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    products = db.Products.Where(x => x.Type == type).Include(x => x.Images).OrderByDescending(x => x.Id).ToList();
                }

                return products;
            }

            public void AddProducts(List<IFormFile> images, string name, string about, double price,
                string weight, string stone, string metall, string type, string video)
            {
                db = Accessor.GetDbContext();

                List<CorallJewelry.Models.Image> imagesAdd = LoadImage(images);

                Product product = new Product()
                {
                    Name = name,
                    About = about,
                    Price = price,
                    Weight = weight,
                    Stone = stone,
                    Metall = metall,
                    Type = type,
                    Images = imagesAdd,
                    Video = video,
                    Date = DateTime.Now
                };
                db.Products.Add(product);
                db.SaveChanges();
            }
            public static void DeleteAllImagesFromProduct(int id)
            {
                db = Accessor.GetDbContext();

                var image = db.Image.Where(x => x.Id == id).FirstOrDefault();
                db.Image.Remove(image);
                db.SaveChanges();
            }
            public static void DeleteImage(int id)
            {
                db = Accessor.GetDbContext();
                var image = db.Image.Where(x => x.Id == id).FirstOrDefault();
                db.Image.Remove(image);
                db.SaveChanges();
            }
            public static void DeleteProduct(int id, List<IFormFile> allImages)
            {
                db = Accessor.GetDbContext();
                var products = db.Products.Where(x => x.Id == id).Include(y => y.Images).FirstOrDefault();
                for (int i = 0; i < products.Images.Count; i++)
                {
                    DeleteImage(products.Images[i].Id);
                    //db.Image.Remove();
                }
                //foreach(var product in products)
                var product = db.Products.Where(x => x.Id == id).FirstOrDefault();
                db.Products.Remove(product);
                db.SaveChanges();
            }

            public void EditProduct(int id, List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type, string video)
            {
                db = Accessor.GetDbContext();
                db = new BackendContext(new DbContextOptions<BackendContext>());


                var product = db.Products.Where(x => x.Id == id).FirstOrDefault();

                if (images != null && images.Count != 0)
                {
                    var imagesAll = LoadImage(images);
                    product.Images = imagesAll;
                }
                product.About = about;
                product.Metall = metall;
                product.Name = name;
                product.Price = price;
                product.Stone = stone;
                product.Type = type;
                product.Weight = weight;
                product.Video = video;
                product.Date = DateTime.Now;
                db.Products.Update(product);
                db.SaveChanges();

            }
            private List<CorallJewelry.Models.Image> LoadImage(List<IFormFile> images)
            {
                db = Accessor.GetDbContext();
                List<CorallJewelry.Models.Image> imagesAdd = new List<CorallJewelry.Models.Image>();
                if (images != null && images.Count() != 0)
                {
                    for (int i = 0; i < images.Count(); i++)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products_FULL", images[i].FileName);

                        if (!System.IO.File.Exists(path))
                        {
                            using (var stream = System.IO.File.Open(path, FileMode.Create))
                            {
                                images[i].CopyTo(stream);

                            }
                        }

                        Resizer(path);
                        imagesAdd.Add(new CorallJewelry.Models.Image() { Name = images[i].FileName });
                    }
                    //foreach (var file in images)
                    //{
                    //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products_FULL", file.FileName);

                    //    using (var stream = new FileStream(path, FileMode.Create))
                    //    {
                    //        file.CopyTo(stream);
                    //    }

                    //    Resizer(path);
                    //    imagesAdd.Add(new CorallJewelry.Models.Image() { Name = file.FileName });
                    // 
                    //}
                }
                return imagesAdd;
            }
            private static void Resizer(string inputPath)
            {
                db = Accessor.GetDbContext();
                const int size = 600;
                const int quality = 75;

                using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        if (image.Width <= size)
                        {
                            FileInfo filea = new FileInfo(inputPath);
                            image.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", filea.Name));
                            return;
                        }
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        if (image.Height <= size)
                        {
                            FileInfo filea = new FileInfo(inputPath);
                            image.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", filea.Name));
                            return;
                        }
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    var resized = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    FileInfo file = new FileInfo(inputPath);

                    resized.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", file.Name));
                }
            }

        }
        public static class PriceExecutor
        {
            public static List<PriceList> GetAllPriceLists()
            {
                db = Accessor.GetDbContext();
                var list = db.PriceLists.Include(a => a.Prices).ToList();
                return list;
            }

            public static void AddPrice(int idList, string name, string money)
            {
                db = Accessor.GetDbContext();
                db.PriceLists.Where(x => x.Id == idList).FirstOrDefault().Prices.Add(new Price() { Money = money, Name = name });
                db.SaveChanges();
            }

            public static void EditPrice(int id, string name, string money)
            {
                db = Accessor.GetDbContext();
                var price = db.Prices.Where(x => x.Id == id).FirstOrDefault();
                price.Money = money;
                price.Name = name;
                db.SaveChanges();
            }

            public static void DeletePrice(int id)
            {
                db = Accessor.GetDbContext();
                var price = db.Prices.Where(x => x.Id == id).FirstOrDefault();
                db.Prices.Remove(price);
                db.SaveChanges();
            }

            public static void DeletePriceList(int id)
            {
                db = Accessor.GetDbContext();
                var price = db.PriceLists.Where(x => x.Id == id).FirstOrDefault();
                db.PriceLists.Remove(price);
                db.SaveChanges();
            }

            public static void CreateList(string category)
            {
                db = Accessor.GetDbContext();
                db.PriceLists.Add(new PriceList() { Category = category });
                db.SaveChanges();
            }
        }
        public static class RequestExecutor
        {
            public static List<Request> GetRequest()
            {
                db = Accessor.GetDbContext();
                var request = db.Requests.ToList();
                return request;
            }

            public static void DeleteRequest(int id)
            {
                db = Accessor.GetDbContext();
                var request = db.Requests.Where(x => x.Id == id).FirstOrDefault();
                db.Requests.Remove(request);
                db.SaveChanges();
            }
        }
        public static class ContactExecutor
        {
            public static Contacts GetContact()
            {
                db = Accessor.GetDbContext();
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
            public static void EditContact(string email, string phone, string vk, string ok, string inst, string addressTown, string street, int id = 1)
            {
                db = Accessor.GetDbContext();
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
        public static class ChatExecutor
        {
            private static List<Dialog> GetDialogs()
            {
                db = Accessor.GetDbContext();
                var dialogs = chat.Dialogs.OrderByDescending(x => x.Id).ToList();
                return dialogs;
            }
            public static ChatPage GetModel()
            {
                db = Accessor.GetDbContext();
                ChatPage chatPage = new ChatPage();
                chatPage.Dialogs = GetDialogs();
                return chatPage;
            }
        }
        public static class CatalogsExecutor
        {
            #region Catalogs
            public static List<Catalog> GetCatalogs()
            {
                db = Accessor.GetDbContext();
                var catalogs = db.Catalogs.ToList();
                return catalogs;
            }
            public static void AddCatalog(string name)
            {
                db = Accessor.GetDbContext();
                Catalog catalog = new Catalog()
                {
                    Name = name,
                    DateCreate = DateTime.Now
                };
                db.Catalogs.Add(catalog);
                db.SaveChanges();
            }
            public static void EditCatalog(int id, string name)
            {
                db = Accessor.GetDbContext();
                db.Catalogs.Where(x => x.Id == id).FirstOrDefault().Name = name;
                db.SaveChanges();
            }
            public static void RemoveCatalog(int id)
            {
                db = Accessor.GetDbContext();
                var catalog = db.Catalogs.Where(x => x.Id == id).FirstOrDefault();
                db.Catalogs.Remove(catalog);
                db.SaveChanges();
            }
            #endregion

            #region Category
            public static List<Category> GetCategories(int id)
            {
                db = Accessor.GetDbContext();
                var categ = db.Catalogs.Where(x => x.Id == id).Include(x => x.Category).FirstOrDefault();
                return categ.Category;
                
            }
            public static void AddCategory(int idCatalog, string name)
            {
                db = Accessor.GetDbContext();
                Category category = new Category()
                {
                    Name = name,
                };
                var catalog = db.Catalogs.Where(x => x.Id == idCatalog).FirstOrDefault();
                catalog.Category = new List<Category>{ category };
                //catalog.Category.Add(category);
                db.SaveChanges();
            }
            public static void RemoveCategory(int idCatalog, int idCategory)
            {
                db = Accessor.GetDbContext();
                //var catolog = db.Catalogs.Where(x => x.Id == idCatalog).Include(x => x.Category).FirstOrDefault();
                //var category = catolog.Category.Where(x => x.Id == idCategory).FirstOrDefault();
                //catolog.Category.Remove(category);
                //db.SaveChanges();
                var catalog = db.Category.Where(x => x.Id == idCategory).ToList();
                foreach (var category in catalog)
                {
                    db.Category.Remove(category);
                }
                db.SaveChanges();
            }
            #endregion

            #region Items
            public static List<ItemCatalog> GetItems(int idCatalog, string name)
            {
                db = Accessor.GetDbContext();
                var items = db.Items.Where(x => x.IdCatalog == idCatalog && x.NameCategory == name).Include(x => x.Image).ToList();
                return items;
            }
            public static void AddItem(int idCatalog, string nameCategory, IFormFile image, string nameItem, string article, string about, string price)
            {
                db = Accessor.GetDbContext();
                List<IFormFile> imgs = new List<IFormFile>();
                imgs.Add(image);
                ItemCatalog item = new ItemCatalog()
                {
                    IdCatalog = idCatalog,
                    Name = nameItem,
                    NameCategory = nameCategory,
                    Price = price,
                    Article = article,
                    About = about,
                    Image = LoadImage(imgs)
                };
                db.Items.Add(item);
                db.SaveChanges();
            }
            public static void RemoveItem(int idCatalog, string nameCat, string name)
            {
                db = Accessor.GetDbContext();
                var item = db.Items.Where(x => x.IdCatalog == idCatalog && x.NameCategory == nameCat && x.Name == name).FirstOrDefault();
                var items = db.Items.Where(x => x.IdCatalog == idCatalog && x.NameCategory == nameCat && x.Name == name).Include(e => e.Image).FirstOrDefault();

                /*foreach (var soloitem in items)
                {
                    db.Items.Remove(soloitem);
                }*/
                foreach (var soloitem in items.Image)
                {
                    db.Image.Remove(soloitem);
                }
                //db.Image.Remove(items.Image[0]);
                db.Items.Remove(items);
                db.SaveChanges();
            }
            public static void EditItem(int id, string nameCat, string name, double price, string article, string about, IFormFile image)
            {
                db = Accessor.GetDbContext();
                var item = db.Items.Where(x => x.Id == id).Include(e=>e.Image).FirstOrDefault();
                List<IFormFile> imgs = new List<IFormFile>();
                imgs.Add(image);
                item.Name = name; 
                item.Price = price.ToString(); 
                item.Article = article; 
                item.About = about;
                if (image != null)
                {
                    foreach (var soloitem in item.Image)
                    {
                        db.Image.Remove(soloitem);
                    }
                    db.SaveChanges();
                    var imgList = LoadImage(imgs);

                    item.Image = imgList;
                }
                db.Update(item);
                db.SaveChanges();
            }

            private static List<CorallJewelry.Models.Image> LoadImage(List<IFormFile> images)
            {
                db = Accessor.GetDbContext();
                List<CorallJewelry.Models.Image> imagesAdd = new List<CorallJewelry.Models.Image>();
                if (images != null && images.Count() != 0)
                {
                    foreach (var file in images)
                    {
                        if (file != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products_FULL", file.FileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            Resizer(path);
                            imagesAdd.Add(new CorallJewelry.Models.Image() { Name = file.FileName });
                        }
                    }
                }
                return imagesAdd;
            }
            private static void Resizer(string inputPath)
            {
                db = Accessor.GetDbContext();
                const int size = 600;
                const int quality = 75;

                using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        if (image.Width <= size)
                        {
                            FileInfo filea = new FileInfo(inputPath);
                            image.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", filea.Name));
                            return;
                        }
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        if (image.Height <= size)
                        {
                            FileInfo filea = new FileInfo(inputPath);
                            image.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", filea.Name));
                            return;
                        }
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    var resized = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    FileInfo file = new FileInfo(inputPath);

                    resized.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\products", file.Name));
                }
            }
            #endregion
        }
    }
}
