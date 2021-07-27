using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CorallJewelry.Models.Helpers
{
    public class TelegramSender
    {
        private string Token { get; set; }
        private string Chat { get; set; }
        private HttpClient HttpClient { get; set; }

        public TelegramSender(string token, string chat)
        {
            HttpClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Token = token;
            Chat = chat;
        }
        public void SendMessage(string mess) =>
            HttpClient
            .GetAsync($"https://api.telegram.org/bot{Token}/sendMessage?chat_id={Chat}&text={mess}")
            .GetAwaiter()
            .GetResult();
    }
}
