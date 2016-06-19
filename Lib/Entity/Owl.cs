using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entity
{
    public class Owl
    {
        public int Id { get; set; }
        public int OwlConversationId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }

        public DateTime Sent { get; set; }

        private User mUser;

        public User User
        {
            get
            {
                if (mUser == null)
                    mUser = Lib.DatabaseManager.UserManager.Current.Get(UserId);
                return mUser;
            }
            set { mUser = value; }
        }

    }
}
