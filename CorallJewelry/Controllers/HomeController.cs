using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CorallJewelry.Models;

namespace CorallJewelry.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Pricelist()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
       
        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #region Service
        public IActionResult Create()
        {
            return View("Service/Create");
        }
        public IActionResult Clearproducts()
        {
            return View("Service/Clearproducts");
        }
        public IActionResult Createchain()
        {
            return View("Service/Createchain");
        }
        public IActionResult Createcopy()
        {
            return View("Service/Createcopy");
        }
        public IActionResult Repair()
        {
            return View("Service/Repair");
        }
        public IActionResult Repairbeads()
        {
            return View("Service/Repairbeads");
        }
        public IActionResult Setstone()
        {
            return View("Service/Setstone");
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
