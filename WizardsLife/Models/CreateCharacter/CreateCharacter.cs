using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardsLife.Models.CreateCharacter
{
    public class CreateCharacter
    {
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public Lib.Entity.User.GenderType Gender { get; set; }
        public Lib.Entity.User.BloodStatusType BloodStatus { get; set; }
    }
}