using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class DepotVerwaltung
    {
        private readonly MySqlConnection connection;
        public DepotVerwaltung(MySqlConnection connection)
        {
            this.connection = connection;
        }
        public void CreateDepot(string name, int benutzerID)
        {
            string query = "INSERT INTO depot (benutzer_id, name, erstellt) VALUES(@benutzer_id, @name, @erstellt)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@benutzer_id", benutzerID);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@erstellt", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

        }
        public List<Depot> GetUserDepot(int benutzerID)
        {
            List<Depot> depotList = new List<Depot>();

            string query = "SELECT id, name, erstellt FROM depot WHERE benutzer_id = @benutzer_id";
            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@benutzer_id", benutzerID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id"].ToString();
                            string name = reader["name"].ToString();
                            Depot depot = new Depot(Convert.ToInt32(id), name);
                            if (depot != null)
                                depotList.Add(depot);
                        }
                    }
                }
            }
            return depotList;
        }
    }
}
