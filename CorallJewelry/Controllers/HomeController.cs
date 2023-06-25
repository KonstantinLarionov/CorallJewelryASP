﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CorallJewelry.Models;
using CorallJewelry.Entitys;
using Microsoft.EntityFrameworkCore;
using CorallJewelry.Models.FrontModel;
using CorallJewelry.Controllers.Executors.Home;
using System.Net;
using afc_studio.Models.Entitys;
using System.Net.Http;
using System.Net.Mail;
using CorallJewelry.Models.Helpers;
using CorallJewelry.Models.Helpers.SocialApp.Services;

namespace CorallJewelry.Controllers
{
    public class Telegram
    {
        private string Token { get; set; }
        private string Api { get; set; } = "https://api.telegram.org/bot";
        private string MethodeSendMessage { get; set; } = "/sendMessage";
        private string Chat { get; set; }
        private HttpClient HttpClient { get; set; }

        public Telegram(string token, string chat = "")
        {
            HttpClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Token = token;
            Chat = chat;
        }

        public void SendMessage(string text, string chatId)
        {
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString("https://api.telegram.org/bot" + Token + "/sendMessage?chat_id=" + chatId + "&text=" + text);
            }
        }


    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private FrontendContext db { get; set; }
        public HomeController(ILogger<HomeController> logger)
        {
            //db = new FrontendContext(new DbContextOptions<FrontendContext>());

            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            var index = AllExecutors.IndexExecutor.GetModel();
            return View(index);
        }
        public IActionResult Contact()
        {
            return View(AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Pricelist()
        {
            return View(AllExecutors.PriceExecutor.GetModel());
        }
        public IActionResult Product(int id)
        {
            return View(AllExecutors.ProductsExecutor.GetModel(id));
        }
        public IActionResult Products(string type = "Кольца")
        {
            return View(AllExecutors.ProductsExecutor.GetModel(type));
        }

        public IActionResult Service()
        {
            return View(AllExecutors.ContactExecutor.GetModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Catalogs()
        {
            return View(AllExecutors.CatalogsExecutor.GetModel());
        }
        public IActionResult CatalogItems(int id, string name)
        {
            return View("Catalogs", AllExecutors.CatalogsExecutor.GetModel(id, name));
        }

        [HttpPost]
        public IActionResult Contact(string contact, string message)
        {
            //AllExecutors.ContactExecutor.SendRequest(contact, message);

            var telega = new Telegram("1001206813:AAFdrMx5RTZy71AKbBy5OVO6FHfyeXNBP4g");
            //telega.SendMessage("У вас новая заявка! Проверьте Панель администратора...", "1072967682");
            //telega.SendMessage("Ссылка в панель: https://korall56.com/Admin/Requests", "1072967682");

            EmailService emailService = new EmailService();
            emailService.SendEmail(contact, message);

            return View(AllExecutors.CatalogsExecutor.GetModel());
        }

        #region Service
        public IActionResult Create()
        {
            return View("Service/Create", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Clearproducts()
        {
            return View("Service/Clearproducts", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Createchain()
        {
            return View("Service/Createchain", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Createcopy()
        {
            return View("Service/Createcopy", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Repair()
        {
            return View("Service/Repair", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Repairbeads()
        {
            return View("Service/Repairbeads", AllExecutors.ContactExecutor.GetModel());
        }
        public IActionResult Setstone()
        {
            return View("Service/Setstone", AllExecutors.ContactExecutor.GetModel());
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
