using System;
using System.Security.Cryptography;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using afc_studio.Models;
using afc_studio.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using afc_studio.Models.Objects;
using System.Text;
using CorallJewelry.Controllers;

namespace afc_studio.Controllers
{
    public class APIController : Controller
    {
        private readonly ILogger<APIController> _logger;
        private MainContext db { get; set; }

        public APIController(ILogger<APIController> logger)
        {
            _logger = logger;
            db = new MainContext(new DbContextOptions<MainContext>());
            /*db.Database.EnsureCreated();*/
        }

        public IActionResult Index()
        {
            return View();
        }
        public string welcome_message()
        {
            return "Hello World!!!";
        }
        [HttpPost]
        public string GetDialog(string user)
        {
            User my;
            if (!db.Users.Any(j => j.Name == user))
            {
                string hash = "";
                using (SHA1 sha256Hash = SHA1.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(user + "@@@" + DateTime.Now.ToString());
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty); ;
                }
                db.Users.Add(new User()
                {
                    Type = "unnamed",
                    Name = user,
                    Dialogs = new List<Dialog>()
                    {
                        new Dialog
                        {
                            Name = "dialog_" + user +  "_" + DateTime.Now.Day.ToString() +  DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        }
                    }
                }) ;
                db.SaveChanges();
            }
            my = db.Users.Where(d => d.Name == user).Include(s => s.Dialogs).FirstOrDefault();
            return (my.Dialogs[0].Name);
        }

        [HttpPost]
        public string SendMessage(string text, string dialog, string user, string typeuser = "unnamed", int dialog_num = 0)
        {


            var telega = new Telegram("1001206813:AAFdrMx5RTZy71AKbBy5OVO6FHfyeXNBP4g");
            telega.SendMessage("У вас новое сообщение в чате! Проверьте Панель администратора...", "478950049");
            telega.SendMessage("Ссылка в панель: https://korall56.ru/Admin/Chats", "478950049");


            var mydialog = db.Dialogs.Where(j => j.Name == dialog).Include(g => g.Messages).FirstOrDefault();
            var myuser = db.Users.Where(f => f.Name == user).FirstOrDefault();

            mydialog.Messages.Add(new Message()
            {
                Text = text,
                Time = DateTime.Now,
                User = myuser.Name
            });
            db.SaveChanges();
            return "1";
        }
        [HttpPost]
        public string GetMessages(string dialog)
        {
            if (dialog != null)
            {
                var mydialog = db.Dialogs.Where(j => j.Name == dialog).Include(g => g.Messages).FirstOrDefault();
                return JsonSerializer.Serialize(mydialog.Messages);
            }
            return null;
        }
    }
}