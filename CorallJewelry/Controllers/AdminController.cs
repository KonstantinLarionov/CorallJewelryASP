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

namespace CorallJewelry.Controllers
{
    public class AdminController : Controller
    {
        #region Header
        private readonly ILogger<HomeController> _logger;
        private BackendContext db { get; set; }
        public AdminController(ILogger<HomeController> logger)
        {
            db = new BackendContext(new DbContextOptions<BackendContext>());
            _logger = logger;

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
        public IActionResult Chats()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            var cont = AllExecutors.ContactExecutor.GetContact();
            return View(cont);
        }
        public IActionResult Prices()
        {
            var price = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View(price);
        }
        public IActionResult Requests()
        {
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
            AllExecutors.ProductsExecutor.AddProducts(images, name, about, price, weight, stone, metall, type);
            var model = AllExecutors.ProductsExecutor.GetProducts("all");
            return View("Products", model);
        }
        [HttpPost]
        public IActionResult EditProducts(int id, List<IFormFile> images, string name, string about, double price, string weight, string stone, string metall, string type,string action)
        {
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
            AllExecutors.ContactExecutor.EditContact(EmailInfo,PhoneInfo,VKLink,OKLink,InstagramLink,addressT,addressS);
            var model = AllExecutors.ContactExecutor.GetContact();
            return View("Contacts", model);
        }
        #endregion

        #region PostPrice
        [HttpPost]
        public IActionResult AddPriceList(string namePrice)
        {
            AllExecutors.PriceExecutor.CreateList(namePrice);
            var model = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View("Prices", model);
        }
        public IActionResult AddPrice(string name, string price, int idList)
        {
            AllExecutors.PriceExecutor.AddPrice(idList,name,price);
            var model = AllExecutors.PriceExecutor.GetAllPriceLists();
            return View("Prices", model);
        }

        [HttpPost]
        public IActionResult EditPrices(string action, int idlist, string name, string price, int idprice)
        {
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
            AllExecutors.RequestExecutor.DeleteRequest(id);
            var model = AllExecutors.RequestExecutor.GetRequest();
            return View("Requests",model);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
