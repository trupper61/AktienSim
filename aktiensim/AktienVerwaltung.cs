using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class AktienVerwaltung
    {
        private string connectionString;
        public AktienVerwaltung(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public Aktie LadeAktie(string firma)
        {
            Aktie aktie = null;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT Firma, Name, Wert, letzterschluss FROM aktiendaten WHERE Firma = @Firma";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Firma", firma);
            MySqlDataReader reader = cmd.ExecuteReader();
                
            while (reader.Read())
            {
                string name = reader["Name"].ToString();
                double wert = Convert.ToDouble(reader["Wert"]);
                double letzterschluss = Convert.ToDouble(reader["letzterschluss"]);
                aktie = new Aktie(firma, wert, letzterschluss);
            }
            return aktie;
        }
        public List<Aktie> LadeAlleAktien()
        {
            var aktienListe = new List<Aktie>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT Firma FROM aktiendaten";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();        
            while (reader.Read())
            {
                string firma = reader["Firma"].ToString();
                Aktie aktie = LadeAktie(firma);
                if (aktie != null)
                {
                    aktienListe.Add(aktie);
                }
            } 
            return aktienListe;
        }
        public void AktieAnlegen(string firma, string name, double startWert)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO aktiendaten (Firma, Name, Wert, letzterschluss, aktualisiertAm) VALUES(@Firma, @Name, @Wert, @letzterschluss, @aktualisiertAm)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Firma", firma);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Wert", startWert);
            cmd.Parameters.AddWithValue("@letzterschluss", startWert);
            cmd.Parameters.AddWithValue("@aktualisiertAm", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
    }
}
