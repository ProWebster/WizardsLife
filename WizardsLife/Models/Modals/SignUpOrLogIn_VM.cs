using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.Modals
{
    public class SignUpOrLogIn_VM
    {
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }

        public string SignUpUsername { get; set; }
        public string SignUpEmail { get; set; }
        public string SignUpPassword { get; set; }
        public string SignUpRepeatPassword { get; set; }
    }
}