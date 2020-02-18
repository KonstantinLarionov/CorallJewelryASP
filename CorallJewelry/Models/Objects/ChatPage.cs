using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace afc_studio.Models.Objects
{
    public class ChatPage
    {
        public List<Dialog> Dialogs { get; set; }
        public Dialog SelectedDialog { get; set; }
    }
}
