using Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.Modals.Owls
{
    public class Owls_VM
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public List<OwlConversation> Conversations { get; set; }
    }
}