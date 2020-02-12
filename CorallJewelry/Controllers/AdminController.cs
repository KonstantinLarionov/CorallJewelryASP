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
        private readonly ILogger<HomeController> _logger;
        private BackendContext db { get; set; }
        public AdminController(ILogger<HomeController> logger)
        {
            db = new BackendContext(new DbContextOptions<BackendContext>());
            _logger = logger;

            if (!db.Users.Any(x=>x.Login == "adminCoral" && x.Password == "adminCoralJewelry202056"))
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
            var admin = db.Users.Where(u => u.Login == login && u.Password == password && u.Type == TypeUser.Admin ).ToList();
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

        #region Get
        public IActionResult Products()
        {
            if (!Auth())
            {
                return View("Login");
            }

            return View();

        }

        public IActionResult Chats()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }
        public IActionResult Requests()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Post
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (AllExecutors.LoginExecutor.OnAuth(login, password, HttpContext))
            {
                return View("Products");
            }
            else 
            {
                return View();
            }
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
