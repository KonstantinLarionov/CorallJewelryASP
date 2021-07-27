using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorallJewelry.Models.Helpers
{
    static public class Mailler
    {
        static public async Task SendEmailAsync(string letter, string subject)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("AFCStudio", "afc.studio@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", "ruslanlesnov924@mail.ru"));
            emailMessage.Subject = "У вас новая заявка!";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<p>Пользователь " + subject + " Отправил вам сообщение: </p>" + letter + "<p>Проверьте Панель администратора...<br> Ссылка в панель: https://korall56.ru/admin/requests/?token=YliB0kTebedEdMakR </p>"
            };
            using (var client = new SmtpClient())
            {
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("smtp.yandex.ru", 587, false);
                await client.AuthenticateAsync("afc.studio@yandex.ru", "lollipop321123");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}