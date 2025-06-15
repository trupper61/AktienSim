using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class EreignisVerwaltung
    {
        private readonly MySqlConnection connection;

        public EreignisVerwaltung(MySqlConnection conn)
        {
            this.connection = conn;
        }

        public List<Ereigniss> LadeAktiveEreignisse(string typ)
        {
            List<Ereigniss> ereignisse = new List<Ereigniss>();
            string query = "SELECT * FROM ereignisse WHERE typ = @typ AND aktiv = 1";
            using (var myMan = new MySqlManager())
            {
                using (var cmd = new MySqlCommand(query, myMan.Connection))
                { 
                cmd.Parameters.AddWithValue("@typ", typ);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int ID = Convert.ToInt32(reader["id"]);
                            string Name = reader["name"].ToString();
                            double EinflussProzent = Convert.ToDouble(reader["einfluss_prozent"]);
                            string Beschreibung = reader["beschreibung"].ToString();
                            string Typ = reader["typ"].ToString();

                            ereignisse.Add(new Ereigniss(ID, Name, EinflussProzent, Beschreibung, typ));
                        }
                    }
                }
            }
            return ereignisse;
        }
    }
}
