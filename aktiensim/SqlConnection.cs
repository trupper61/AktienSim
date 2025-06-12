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
                cmds.Parameters.AddRange(parameters);
                MySqlDataReader reader = cmds.ExecuteReader();
                return reader;
            }
        }
    }
}
