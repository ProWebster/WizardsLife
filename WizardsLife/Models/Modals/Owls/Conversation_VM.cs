using Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.Modals.Owls
{
    public class Conversation_VM
    {
        public OwlConversation Conversation { get; set; }

        public string ReplyContent { get; set; }
    }
}