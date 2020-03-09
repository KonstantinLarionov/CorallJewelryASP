﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModule.Models.Chat.Objects
{
    public class Dialog
    {
        public int Id { get; set; }
        public string Identity { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
