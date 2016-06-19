using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.Modals.Owls
{
    public class CreateConversation_VM
    {
        public string Error { get; set; }

        public List<string> FriendNames { get; set; }

        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}