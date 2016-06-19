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
    public class UserManager : BaseManager
    {
        private static UserManager instance;

        public static UserManager Current
        {
            get
            {
                if (instance == null)
                    instance = new UserManager();
                return instance;
            }
        }

        private UserManager() : base() { }

        public User Get(int id)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);

            return Get("WHERE Id=@Id", param).FirstOrDefault();
        }

        public User GetFromUsername(string username)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Username", username);

            return Get("WHERE Username=@Username AND Deleted IS NULL", param).FirstOrDefault();
        }

        public List<User> GetFromIds(List<int> userIds)
        {
            return Get("WHERE Deleted IS NULL AND Id IN (" + string.Join(",", userIds) + ")");
        }

        public User GetFromCharName(string charName)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("CharName", charName);

            return Get("WHERE CONCAT(CharName,' ',CharFamilyName) = @CharName AND Deleted IS NULL", param).FirstOrDefault();
        }

        public int Create(User u)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                int result = Create(cmd, u);

                Commit(cmd);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        public bool Update(User u)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                Update(cmd, u);

                Commit(cmd);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region HelperMethods

        private List<User> Get(string where, Dictionary<string, object> param = null)
        {
            List<User> result = new List<User>();

            DataTable dt = GetQuery(string.Format("SELECT * FROM Users {0}", where), param);

            foreach (DataRow dr in dt.Rows)
            {
                User temp = new User();
                temp.Id = int.Parse(dr["Id"].ToString());
                temp.Username = dr["Username"].ToString();
                temp.Password = dr["Password"].ToString();
                temp.Email = dr["Email"].ToString();
                temp.Status = (User.UserStatus)int.Parse(dr["Status"].ToString());

                if (!string.IsNullOrWhiteSpace(dr["Deleted"].ToString()))
                    temp.Deleted = DateTime.Parse(dr["Deleted"].ToString());

                temp.CharName = dr["CharName"].ToString();
                temp.CharFamilyName = dr["CharFamilyName"].ToString();
                temp.CharGender = (User.GenderType)Int32.Parse(dr["CharGender"].ToString());
                temp.CharBloodStatus = (User.BloodStatusType)Int32.Parse(dr["CharBloodStatus"].ToString());
                temp.House = (User.Houses)Int32.Parse(dr["House"].ToString());

                result.Add(temp);
            }

            return result;
        }

        private int Create(MySqlCommand cmd, User u)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO Users(Username, Password, Email) 
                                            Values(@Username, @Password, @Email);";
            cmd.CommandText += " SELECT last_insert_id();";
            
            cmd.Parameters.AddWithValue("Username", u.Username);
            cmd.Parameters.AddWithValue("Password", u.Password);
            cmd.Parameters.AddWithValue("Email", u.Email);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        private void Update(MySqlCommand cmd, User u)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"UPDATE Users SET Username = @Username, Password = @Password, Email = @Email, Status = @Status, Deleted = @Deleted, CharName=@CharName, CharFamilyName=@CharFamilyName, CharGender=@CharGender, CharBloodStatus=@CharBloodStatus, House=@House WHERE Id=@Id;";

            cmd.Parameters.AddWithValue("Id", u.Id);
            cmd.Parameters.AddWithValue("Username", u.Username);
            cmd.Parameters.AddWithValue("Password", u.Password);
            cmd.Parameters.AddWithValue("Email", u.Email);
            cmd.Parameters.AddWithValue("Status", u.Status);

            if (u.Deleted.HasValue)
                cmd.Parameters.AddWithValue("Deleted", u.Deleted);
            else
                cmd.Parameters.AddWithValue("Deleted", DBNull.Value);

            cmd.Parameters.AddWithValue("CharName", u.CharName);
            cmd.Parameters.AddWithValue("CharFamilyName", u.CharFamilyName);
            cmd.Parameters.AddWithValue("CharGender", u.CharGender);
            cmd.Parameters.AddWithValue("CharBloodStatus", u.CharBloodStatus);
            cmd.Parameters.AddWithValue("House", u.House);

            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
