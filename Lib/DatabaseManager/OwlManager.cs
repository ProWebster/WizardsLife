using Lib.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DatabaseManager
{
    public class OwlManager : BaseManager
    {
        private static OwlManager instance;

        public static OwlManager Current
        {
            get
            {
                if (instance == null)
                    instance = new OwlManager();
                return instance;
            }
        }

        private OwlManager() : base() { }


        public OwlConversation GetConversation(int id)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);

            return GetConversations("WHERE Id=@Id", param).FirstOrDefault();
        }

        public List<OwlConversation> GetConversations(int userId)
        {
            string s = @"SELECT DISTINCT c.*, (SELECT o.Sent FROM Owl o WHERE o.OwlConversationId = c.Id ORDER BY o.Sent DESC LIMIT 1) as LatestMessage
                            FROM `owlconversation` c 
                            WHERE c.UserIds LIKE '%," + userId + ",%' OR c.UserIds LIKE '" + userId + ",%' OR c.UserIds LIKE '%," + userId + "' OR c.UserIds LIKE '" + userId + @"'
                            ORDER BY LatestMessage DESC";
            return GetConversations("", null, s);
        }

        public List<Owl> GetOwls(int conversationId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ConversationId", conversationId);

            return GetOwls("WHERE OwlConversationId=@ConversationId", param);
        }


        public int MustReadCount(int userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);

            DataTable dt = GetQuery(string.Format("SELECT count(DISTINCT(OwlConversationId)) as result FROM OwlConversationMustRead WHERE UserId=@UserId AND IsRead=0"), param);

            return int.Parse(dt.Rows[0]["result"].ToString());
        }

        public bool MustRead(int conversationId, int userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ConversationId", conversationId);
            param.Add("UserId", userId);

            DataTable dt = GetQuery(string.Format("SELECT DISTINCT(mr.OwlConversationId) FROM OwlConversationMustRead mr WHERE mr.OwlConversationId=@ConversationId AND mr.UserId=@UserId AND IsRead=0"), param);

            return dt.Rows.Count > 0;
        }

        public void MarkAsRead(int conversationId, int userId)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                cmd.CommandText = "UPDATE OwlConversationMustRead SET IsRead=1 WHERE OwlConversationId=@ConversationId AND UserId=@UserId";
                cmd.Parameters.AddWithValue("ConversationId", conversationId);
                cmd.Parameters.AddWithValue("UserId", userId);

                cmd.ExecuteNonQuery();

                Commit(cmd);
            }
            catch
            {
            }
        }


        public int CreateConversation(OwlConversation conversation, int createdByUserId)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                int result = CreateConversation(cmd, conversation);

                foreach (int userId in conversation.UserIds.Where(x=>x != createdByUserId))
                    CreateConversationMustRead(cmd, result, userId);

                Commit(cmd);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int CreateOwl(Owl owl, OwlConversation conversation)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                int result = CreateOwl(cmd, owl);

                foreach (int userId in conversation.UserIds.Where(x => x != owl.UserId))
                    CreateConversationMustRead(cmd, conversation.Id, userId);

                Commit(cmd);

                return result;
            }
            catch
            {
                return -1;
            }
        }


        #region HelperMethods

        private List<OwlConversation> GetConversations(string where, Dictionary<string, object> param = null, string overrideSql = null)
        {
            List<OwlConversation> result = new List<OwlConversation>();

            DataTable dt = null;

            if (string.IsNullOrWhiteSpace(overrideSql))
                dt = GetQuery(string.Format("SELECT DISTINCT c.* FROM OwlConversation c {0}", where), param);
            else
                dt = GetQuery(overrideSql, param);

            foreach (DataRow dr in dt.Rows)
            {
                OwlConversation temp = new OwlConversation();
                temp.Id = int.Parse(dr["Id"].ToString());
                temp.Subject = dr["Subject"].ToString();
                temp.Created = DateTime.Parse(dr["Created"].ToString());
                temp.UserIds = dr["UserIds"].ToString().Split(',').Select(int.Parse).ToList();

                result.Add(temp);
            }

            return result;
        }

        private int CreateConversation(MySqlCommand cmd, OwlConversation conversation)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO OwlConversation(Subject, UserIds) 
                                            Values(@Subject, @UserIds);";
            cmd.CommandText += " SELECT last_insert_id();";

            cmd.Parameters.AddWithValue("Subject", conversation.Subject);
            cmd.Parameters.AddWithValue("UserIds", string.Join(",", conversation.UserIds));

            return int.Parse(cmd.ExecuteScalar().ToString());
        }


        private int CreateConversationMustRead(MySqlCommand cmd, int conversationId, int userId)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO OwlConversationMustRead(OwlConversationId, UserId) 
                                            Values(@OwlConversationId, @UserId);";
            cmd.CommandText += " SELECT last_insert_id();";

            cmd.Parameters.AddWithValue("OwlConversationId", conversationId);
            cmd.Parameters.AddWithValue("UserId", userId);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }


        private List<Owl> GetOwls(string where, Dictionary<string, object> param)
        {
            List<Owl> result = new List<Owl>();

            DataTable dt = GetQuery(string.Format("SELECT * FROM Owl {0}", where), param);

            foreach (DataRow dr in dt.Rows)
            {
                Owl temp = new Owl();
                temp.Id = int.Parse(dr["Id"].ToString());
                temp.UserId = int.Parse(dr["UserId"].ToString());
                temp.OwlConversationId = int.Parse(dr["OwlConversationId"].ToString());
                temp.Sent = DateTime.Parse(dr["Sent"].ToString());
                temp.Content = dr["Content"].ToString();

                result.Add(temp);
            }

            return result;
        }

        private int CreateOwl(MySqlCommand cmd, Owl owl)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO Owl(UserId, OwlConversationId, Content) 
                                            Values(@UserId, @OwlConversationId, @Content);";
            cmd.CommandText += " SELECT last_insert_id();";

            cmd.Parameters.AddWithValue("UserId", owl.UserId);
            cmd.Parameters.AddWithValue("OwlConversationId", owl.OwlConversationId);
            cmd.Parameters.AddWithValue("Content", owl.Content);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }




        #endregion
    }
}
