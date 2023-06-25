namespace CorallJewelry.Models.Helpers
{
    using MimeKit;
    using MailKit.Net.Smtp;
    using System.Threading.Tasks;
    using Org.BouncyCastle.Crypto.Macs;
    using System.Threading.Tasks.Dataflow;

    namespace SocialApp.Services
    {
        public class EmailService
        {
            /// <summary>
            /// Отправка письма с формы обратной связи на почту
            /// </summary>
            public void SendEmail(string contact, string message)
            {
                using var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Robot", "ruslanlesnov924@mail.ru"));
                emailMessage.To.Add(new MailboxAddress("", "idbbr2iay0zpjivosite@jivo-mail.com"));
                emailMessage.Subject = "Заказ";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $"<p>Контакт: {contact}</p>" +
                           $"<p>Сообщение: {message}</p>"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.mail.ru", 465, true);
                    client.Authenticate("ruslanlesnov924@mail.ru", "h7kBzvEFG0Zm0hjSqZwr");
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }
            }
        }
    }
}
