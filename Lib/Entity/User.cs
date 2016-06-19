using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entity
{
    public class User
    {
        public enum UserStatus
        {
            NeedsCharacter = 0,
            NeedsShopping = 1,
            NeedsSorting = 2,
            Ready = 3
        }

        public enum GenderType
        {
            Male = 0,
            Female = 1
        }

        public enum BloodStatusType
        {
            FullBlood = 0,
            HalfBlood = 1,
            Muggler = 2
        }

        public enum Houses
        {
            Slytherin = 0,
            Ravenclaw = 1,
            Gryffindor = 2,
            Hufflepuff = 3
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public DateTime? Deleted { get; set; }

        // Character stuff!
        public string CharName { get; set; }
        public string CharFamilyName { get; set; }
        public GenderType CharGender { get; set; }
        public BloodStatusType CharBloodStatus { get; set; }
        public Houses House { get; set; }

        public string FullCharName { get { return CharName + " " + CharFamilyName; } }
    }
}
