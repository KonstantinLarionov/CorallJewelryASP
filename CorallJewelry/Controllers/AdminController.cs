using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CorallJewelry.Models;
using CorallJewelry.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using CorallJewelry.Controllers.Executors.Admin;
using System.Security.Cryptography;
using afc_studio.Models.Entitys;
using System.Text.Json;
using ChatModule.Models.Chat.Entitys;

namespace CorallJewelry.Controllers
{
    public class AdminController : Controller
    {
        #region Header
        private readonly ILogger<HomeController> _logger;
        private ChatContext chat = new ChatContext((new DbContextOptions<ChatContext>()));
        private BackendContext db { get; set; }
        private MainContext db2 { get; set; }
        public AdminController(ILogger<HomeController> logger)
        {
            db = new BackendContext(new DbContextOptions<BackendContext>());
            _logger = logger;
            db2 = new MainContext(new DbContextOptions<MainContext>());
            if (!db.Users.Any(x => x.Login == "adminCoral" && x.Password == "adminCoralJewelry202056"))
            {
                db.Users.Add(new Models.User() { Login = "adminCoral", Password = "adminCoralJewelry202056", Type = TypeUser.Admin, LastSession = DateTime.Now });
                db.SaveChanges();
            }
        }
        private bool Auth(string login = "", string password = "")
        {
            if (login == "" && password == "")
            {
                login = HttpContext.Session.GetString("login");
                password = HttpContext.Session.GetString("password");
            }
            var admin = db.Users.Where(u => u.Login == login && u.Password == password && u.Type == TypeUser.Admin).ToList();
            if (admin.Count != 0)
            {
                HttpContext.Request.Cookies.Append(new KeyValuePair<string, string>("login", login));
                HttpContext.Request.Cookies.Append(new KeyValuePair<string, string>("password", password));
                HttpContext.Session.SetString("login", login);
                HttpContext.Session.SetString("password", password);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region Get
        public IActionResult Products(string type = "all")
        {
            if (!Auth())
            {
                return View("Login");
            }
            var model = AllExecutors.ProductsExecutor.GetProducts(type);
            return View(model);
        }
        //public IActionResult Chats()
        //{
        //    //if (!Auth())
        //    //{
        //    //    return View("Login");
        //    //}
        //    return View(AllExecutors.ChatExecutor.GetModel());
        //}
        public IActionResult Contacts()
        {
            if (!Auth())
            {
                return View("Login");
            }
            var cont = AllExecutors.ContactExecutor.GetContact();
            return View(cont);
        }

        public IActionResult Catalogs()
        {
            return View(AllExecutors.CatalogsExecutor.GetCatalogs());
        }
        public IActionResult Category(int id)
        {
            ViewData["idCatalog"] = id;
            return View(AllExecutors.CatalogsExecutor.GetCategories(id));
        }
        public IActionResult Items(int id,string name)
        {
            ViewData["idCatalog"] = id;
            ViewData["nameCateg"] = name;
            return View(AllExecutors.CatalogsExecutor.GetItems(id,name));
        }
        public IActionResult Prices()
        {
            if (!Auth())
            {
                return View("Login");
            }
            var price = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View(price);
        }
        public IActionResult Requests()
        {
            if (!Auth())
            {
                return View("Login");
            }
            var req = AllExecutors.RequestExecutor.GetRequest();
            return View(req);
        }
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region PostProduct
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (AllExecutors.LoginExecutor.OnAuth(login, password, HttpContext))
            {
                HttpContext.Response.Cookies.Append("login", login);
                HttpContext.Response.Cookies.Append("password", password);
                var model = AllExecutors.ProductsExecutor.GetProducts("all");
                return View("Products",model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult AddProduct(List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type)
        {
            if (!Auth())
            {
                return View("Login");
            }
            AllExecutors.ProductsExecutor.AddProducts(images, name, about, price, weight, stone, metall, type);
            var model = AllExecutors.ProductsExecutor.GetProducts("all");
            return View("Products", model);
        }
        [HttpPost]
        public IActionResult EditProducts(int id, List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type,string action)
        {
            if (!Auth())
            {
                return View("Login");
            }
            if (action == "edit")
            {

                AllExecutors.ProductsExecutor.EditProduct(id, images, name, about, price, weight, stone, metall, type);
            }
            else if (action == "delete")
            {
                AllExecutors.ProductsExecutor.DeleteProduct(id);
            }
            var model = AllExecutors.ProductsExecutor.GetProducts("all");
            return View("Products", model);
        }
        #endregion

        #region PostContact
        [HttpPost]
        public IActionResult EditContacts(string EmailInfo, string PhoneInfo, string VKLink, string OKLink, string InstagramLink, string addressT, string addressS)
        {
            if (!Auth())
            {
                return View("Login");
            }
            AllExecutors.ContactExecutor.EditContact(EmailInfo,PhoneInfo,VKLink,OKLink,InstagramLink,addressT,addressS);
            var model = AllExecutors.ContactExecutor.GetContact();
            return View("Contacts", model);
        }
        #endregion

        #region PostPrice
        [HttpPost]
        public IActionResult AddPriceList(string namePrice)
        {
            if (!Auth())
            {
                return View("Login");
            }
            AllExecutors.PriceExecutor.CreateList(namePrice);
            var model = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View("Prices", model);
        }
        public IActionResult AddPrice(string name, string price, int idList)
        {
            if (!Auth())
            {
                return View("Login");
            }
            AllExecutors.PriceExecutor.AddPrice(idList,name,price);
            var model = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View("Prices", model);
        }

        [HttpPost]
        public IActionResult EditPrices(string action, int idlist, string name, string price, int idprice)
        {
            if (!Auth())
            {
                return View("Login");
            }
            if (action == "edit")
            {
                AllExecutors.PriceExecutor.EditPrice(idprice, name, price);
            }
            else if (action == "delete")
            {
                AllExecutors.PriceExecutor.DeletePrice(idprice);
            }
            var model = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View("Prices", model);
        }
        #endregion

        #region PostRequest
        [HttpPost]
        public IActionResult DeleteReq(int id)
        {
            if (!Auth())
            {
                return View("Login");
            }
            AllExecutors.RequestExecutor.DeleteRequest(id);
            var model = AllExecutors.RequestExecutor.GetRequest();
            return View("Requests",model);
        }
        #endregion

        #region ChatPost
        [HttpPost]
        public string GetDialog(string user)
        {
            afc_studio.Models.Objects.User my;
            if (!db2.Users.Any(j => j.Name == user))
            {
                db2.Users.Add(new afc_studio.Models.Objects.User()
                {
                    Type = "unnamed",
                    Name = user,
                    Dialogs = new List<afc_studio.Models.Objects.Dialog>()
                    {
                        new afc_studio.Models.Objects.Dialog
                        {
                            Name = "dialog_" + user +  "_" + DateTime.Now.Day.ToString() +  DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        }
                    }
                });
                db2.SaveChanges();
            }
            my = db2.Users.Where(d => d.Name == user).Include(s => s.Dialogs).FirstOrDefault();
            return (my.Dialogs[0].Name);
        }

        [HttpPost]
        public string SendMessage(string text, string dialog, string user, string typeuser = "unnamed", int dialog_num = 0)
        {
            var mydialog = db2.Dialogs.Where(j => j.Name == dialog).Include(g => g.Messages).FirstOrDefault();
            var myuser = "admin";

            mydialog.Messages.Add(new afc_studio.Models.Objects.Message()
            {
                Text = text,
                Time = DateTime.Now,
                User = myuser
            });
            db2.SaveChanges();
            return "1";
        }
        [HttpPost]
        public string GetMessages(string dialog)
        {
            if (dialog != null)
            {
                var mydialog = db2.Dialogs.Where(j => j.Name == dialog).Include(g => g.Messages).FirstOrDefault();
                return JsonSerializer.Serialize(mydialog.Messages);
            }
            return null;
        }
        #endregion

        #region PostCatalogs
        [HttpPost]
        public IActionResult AddCatalogs(string name)
        {
            AllExecutors.CatalogsExecutor.AddCatalog(name);
            return View("Catalogs", AllExecutors.CatalogsExecutor.GetCatalogs());
        }
        [HttpPost]
        public IActionResult RemoveCatalogs(int id)
        {
            AllExecutors.CatalogsExecutor.RemoveCatalog(id);
            return View("Catalogs", AllExecutors.CatalogsExecutor.GetCatalogs());
        }



        [HttpPost]
        public IActionResult AddCategory(int id, string name)
        {
            ViewData["idCatalog"] = id;
            AllExecutors.CatalogsExecutor.AddCategory(id, name);
            return View("Category", AllExecutors.CatalogsExecutor.GetCategories(id));
        }
        [HttpPost]
        public IActionResult RemoveCategory(int id, int idcateg)
        {
            ViewData["idCatalog"] = id;
            AllExecutors.CatalogsExecutor.RemoveCategory(id, idcateg);
            return View("Category", AllExecutors.CatalogsExecutor.GetCategories(id));
        }
        #endregion

        #region PostItem
        [HttpPost]
        public IActionResult AddItem(int id, string nameCat, string name, string price, string article, string about, IFormFile images)
        {
            ViewData["idCatalog"] = id;
            ViewData["nameCateg"] = nameCat;
            AllExecutors.CatalogsExecutor.AddItem(id,nameCat,images,name,article,about,price);
            return View("Items", AllExecutors.CatalogsExecutor.GetItems(id,nameCat));
        }
        [HttpPost]
        public IActionResult EditItem(int id, int idItem, string nameCat, string name, double price, string article, string about, IFormFile images, string action)
        {
            ViewData["idCatalog"] = id;
            ViewData["nameCateg"] = nameCat;
            if (action == "delete")
            {
                AllExecutors.CatalogsExecutor.RemoveItem(id, nameCat, name);
            }
            else if (action == "edit")
            {
                AllExecutors.CatalogsExecutor.EditItem(idItem,  nameCat,  name,  price,  article,  about);
            }
           
            return View("Items",AllExecutors.CatalogsExecutor.GetItems(id,nameCat));
        }
        #endregion

        public IActionResult Chats()
        {
            var dialog = chat.Dialogs.OrderByDescending(x => x.Id).ToList();

            return View(dialog);
        }
        public object GetHistory(string idDialog)
        {
            var di = chat.Dialogs.Where(x => x.Identity == idDialog).Include(x => x.Messages).ThenInclude(x => x.User).FirstOrDefault();
            var json = Json(di.Messages);
            return json.Value;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
