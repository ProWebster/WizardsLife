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
    public class SortingQuizValueManager : BaseManager
    {
        private static SortingQuizValueManager instance;

        public static SortingQuizValueManager Current
        {
            get
            {
                if (instance == null)
                    instance = new SortingQuizValueManager();
                return instance;
            }
        }

        private SortingQuizValueManager() : base() { }

        
        public List<SortingQuizValue> GetFromUser(int userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);

            return Get("WHERE UserId=@UserId", param);
        }

        public int Create(SortingQuizValue value)
        {
            try
            {
                MySqlCommand cmd = BeginTransaction();

                int result = Create(cmd, value);

                Commit(cmd);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        #region HelperMethods

        private List<SortingQuizValue> Get(string where, Dictionary<string, object> param)
        {
            List<SortingQuizValue> result = new List<SortingQuizValue>();

            DataTable dt = GetQuery(string.Format("SELECT * FROM SortingQuiz_Values {0}", where), param);

            foreach (DataRow dr in dt.Rows)
            {
                SortingQuizValue temp = new SortingQuizValue();
                temp.Id = int.Parse(dr["Id"].ToString());
                temp.UserId = int.Parse(dr["UserId"].ToString());
                temp.QuestionNo = int.Parse(dr["QuestionNo"].ToString());
                temp.AnswerValue = int.Parse(dr["AnswerValue"].ToString());

                result.Add(temp);
            }

            return result;
        }

        private int Create(MySqlCommand cmd, SortingQuizValue value)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"INSERT INTO SortingQuiz_Values(UserId, QuestionNo, AnswerValue) 
                                            Values(@UserId, @QuestionNo, @AnswerValue);";
            cmd.CommandText += " SELECT last_insert_id();";

            cmd.Parameters.AddWithValue("UserId", value.UserId);
            cmd.Parameters.AddWithValue("QuestionNo", value.QuestionNo);
            cmd.Parameters.AddWithValue("AnswerValue", value.AnswerValue);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        private void Update(MySqlCommand cmd, SortingQuizValue value)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = @"UPDATE SortingQuiz_Values SET QuestionNo=@QuestionNo, AnswerValue=@AnswerValue WHERE Id=@Id;";

            cmd.Parameters.AddWithValue("Id", value.Id);
            cmd.Parameters.AddWithValue("QuestionNo", value.QuestionNo);
            cmd.Parameters.AddWithValue("AnswerValue", value.AnswerValue);

            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
