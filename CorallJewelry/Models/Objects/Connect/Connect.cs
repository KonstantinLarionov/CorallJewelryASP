using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace afc_studio.Models.Objects.Connect
{
    public static class Connect
    {
#if DEBUG
        public static string Main_connect { get; } = "Server=localhost;Database=u0893839_chat;User=root;Password=root;";
#else
        public static string Main_connect { get; } = "Server=localhost;Database=u0893839_chat;User=u0893_adminDB;Password=snRq40~6;";
#endif
    }
}
