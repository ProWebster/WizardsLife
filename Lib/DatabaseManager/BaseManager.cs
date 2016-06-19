using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DatabaseManager
{
    public class BaseManager
    {
        private string connectionString;

        internal BaseManager()
        {
            if (connectionString == null)
                connectionString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        }

        internal DataTable GetQuery(string sql, Dictionary<string, object> param = null)
        {
            DataTable result = new DataTable();
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        if (param != null)
                        {
                            foreach (var p in param)
                            {
                                cmd.Parameters.AddWithValue(p.Key, p.Value);
                            }
                        }

                        result.Load(cmd.ExecuteReader());
                    }

                    conn.Close();
                }

            }
            catch (Exception e)
            {

            }
            return result;
        }

        internal MySqlCommand BeginTransaction()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();

            var trans = conn.BeginTransaction();

            var cmd = conn.CreateCommand();
            cmd.Transaction = trans;

            return cmd;
        }

        internal void Commit(MySqlCommand cmd)
        {
            cmd.Transaction.Commit();

            cmd.Connection.Close();
        }
    }
}
