using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class SqlConnection
    {
        

        public SqlConnection(string connString)
        {
        }

        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            using (MySqlConnection myconnection = new MySqlConnection(connString))
            {
                myconnection.Open();
                MySqlCommand cmds = new MySqlCommand(sql, myconnection);
                cmds.Parameters.AddRange(parameters);
                return cmds.ExecuteNonQuery();
            }
        }

        public static MySqlDataReader ExecuteNonQueryReader(string sql, params MySqlParameter[] parameters)
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";

            MySqlConnection myconnection = new MySqlConnection(connString);
            myconnection.Open();
            using (MySqlCommand cmds = new MySqlCommand(sql, myconnection)) 
            {
                if(parameters != null) 
                {
                    cmds.Parameters.AddRange(parameters);
                }
                MySqlDataReader reader = cmds.ExecuteReader();
                return reader;
            }
        }
        public static int ExecuteInsertWithId(string sql, params MySqlParameter[] parameters)
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null) 
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }
    }
}
