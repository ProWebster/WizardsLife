using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DatabaseManager
{
    public class FriendsManager : BaseManager
    {
        private static FriendsManager instance;

        public static FriendsManager Current
        {
            get
            {
                if (instance == null)
                    instance = new FriendsManager();
                return instance;
            }
        }

        private FriendsManager() : base() { }


        public List<int> GetFromUser(int userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);

            return Get("WHERE UserId1=@UserId OR UserId2=@UserId", param, userId);
        }

        public int CreateRelation(int userId1, int userId2)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                int result = Create(cmd, userId1, userId2);

                Commit(cmd);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        #region HelperMethods


        private List<int> Get(string where, Dictionary<string, object> param, int forUserId)
        {
            List<int> result = new List<int>();

            DataTable dt = GetQuery(string.Format("SELECT * FROM FriendRelation {0}", where), param);

            foreach (DataRow dr in dt.Rows)
            {
                int temp = int.Parse(dr["UserId1"].ToString());

                if (temp == forUserId)
                    temp = int.Parse(dr["UserId2"].ToString());

                result.Add(temp);
            }

            return result;
        }

        private int Create(MySqlCommand cmd, int userId1, int userId2)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO FriendRelation(UserId1, UserId2) 
                                            Values(@UserId1, @UserId2);";
            cmd.CommandText += " SELECT last_insert_id();";

            cmd.Parameters.AddWithValue("UserId1", userId1);
            cmd.Parameters.AddWithValue("UserId2", userId2);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        #endregion
    }
}
