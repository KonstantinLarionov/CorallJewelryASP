using ChatModule.Models.Chat.Entitys;
using ChatModule.Models.Chat.Objects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace ChatModule
{
    public class Telegram
    {
        private string Token { get; set; }
        private string Api { get; set; } = "https://api.telegram.org/bot";
        private string MethodeSendMessage { get; set; } = "/sendMessage";

        public Telegram(string token)
        {
            Token = token;
        }

        public void SendMessage(string text, string chatId)
        {
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString("https://afcstudio.ru/core/telegram.php?token=" + Token + "&text=" + text + "&chatid=" + chatId);
            }
        }
    }
    public class ChatHub : Hub
    {
        private static ChatContext chat = new ChatContext((new DbContextOptions<ChatContext>()));
        
        /// <summary>
        /// UserHandleMessage
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Send(string message, string login="", string password="", string dialogId="")
        {
            if (login == null || login == "")
            {
                //var telega = new Telegram("1001206813:AAFdrMx5RTZy71AKbBy5OVO6FHfyeXNBP4g");
                //telega.SendMessage("У вас новое сообщение в чате! Проверьте Панель администратора...", "1072967682");
                //telega.SendMessage("Ссылка в панель: https://korall56.ru/Admin/Chats", "1072967682");

                #region History
                User user = null;
                if (chat.Users.Any(x => x.UserIdentity == Context.ConnectionId))
                {
                    user = chat.Users.Where(x => x.UserIdentity == Context.ConnectionId).FirstOrDefault();
                }
                else
                {
                    user = new User() { Date = DateTime.Now, UserIdentity = Context.ConnectionId, IP = this.Context.GetHttpContext().Connection.RemoteIpAddress.ToString() };
                    chat.Users.Add(user);
                    chat.SaveChanges();
                }

                Message messageNew = new Message() { Date = DateTime.Now, Text = message, User = user };

                Dialog dialog = null;
                if (chat.Dialogs.Any(x => x.Identity == user.UserIdentity))
                {
                    dialog = chat.Dialogs.Where(x => x.Identity == user.UserIdentity).Include(x => x.Users).Include(x => x.Messages).FirstOrDefault();
                    dialog.Messages.Add(messageNew);
                    chat.SaveChanges();
                }
                else
                {
                    dialog = new Dialog() { Identity = user.UserIdentity, DateCreate = DateTime.Now, Users = new List<User>() { user }, Messages = new List<Message>() { messageNew } };
                    chat.Dialogs.Add(dialog);
                    chat.SaveChanges();
                }
                #endregion

                foreach (var userD in dialog.Users)
                {
                    await Clients.Client(userD.UserIdentity).SendAsync("Send", JsonSerializer.Serialize(Tuple.Create(user, message)));
                }
            }
            else
            {
                User admin = null;
                if (chat.Users.Any(x => x.Login == login && x.Password == password && x.Role == Role.Admin))
                {
                    admin = chat.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                    admin.UserIdentity = Context.ConnectionId;
                }
                else { return; }

                Message messageNew = new Message() { Date = DateTime.Now, Text = message, User = admin };

                var dialog = chat.Dialogs.Where(x => x.Identity == dialogId).Include(x => x.Users).Include(x => x.Messages).FirstOrDefault();
                if (!dialog.Users.Any(x=>x.Login == admin.Login && x.Password == admin.Password))
                {
                    dialog.Users.Add(admin);
                } 
                dialog.Messages.Add(messageNew);
                chat.SaveChanges();
                foreach (var user in dialog.Users)
                {
                    await Clients.Client(user.UserIdentity).SendAsync("Send", JsonSerializer.Serialize(Tuple.Create(admin, message)));
                }
            }
        }


        /// <summary>
        /// AdminHandleMessage
        /// </summary>
        /// <param name="dialogId">Where recieve message</param>
        /// <param name="message">message</param>
        /// <param name="login">Login admin</param>
        /// <param name="password">Password admin</param>
        /// <returns></returns>
        public async Task SendAdmin(string dialogId, string message, string login, string password)
        {
           
        }
    }
}
