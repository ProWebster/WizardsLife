using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entity
{
    public class OwlConversation
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public List<int> UserIds { get; set; }

        public DateTime Created { get; set; }

        private List<Owl> mOwls;
        public List<Owl> Owls
        {
            get
            {
                if (mOwls == null)
                    mOwls = Lib.DatabaseManager.OwlManager.Current.GetOwls(Id);

                return mOwls;
            }
            set { mOwls = value; }
        }


        public bool MustRead(int userId)
        {
            return Lib.DatabaseManager.OwlManager.Current.MustRead(Id, userId);
        }
    }
}
