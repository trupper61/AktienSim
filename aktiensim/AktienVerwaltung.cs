using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class AktienVerwaltung
    {
        private string connectionString;
        public AktienVerwaltung(string connectionString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"")
        {
            this.connectionString = connectionString;
        }
        public void AddTransaktion(int aktieId, string typ, double anzahl, decimal einzelpreis, Benutzer activeUser)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO transaktion (aktie_ID, typ, anzahl, einzelpreis, zeitpunkt, depot_ID) VALUES(@aktie_ID, @typ, @anzahl, @einzelpreis, @zeitpunkt, @depot_ID)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@aktie_ID", aktieId);
            cmd.Parameters.AddWithValue("@typ", typ);
            cmd.Parameters.AddWithValue("@anzahl", anzahl);
            cmd.Parameters.AddWithValue("@einzelpreis", einzelpreis);
            Depot userDepot = activeUser.depotList.FirstOrDefault();
            cmd.Parameters.AddWithValue("@depot_id", userDepot.ID);
            cmd.Parameters.AddWithValue("@zeitpunkt", DateTime.Now);
            activeUser.kontoStand -= Convert.ToInt32(Convert.ToDecimal(anzahl) * einzelpreis);
            cmd.ExecuteNonQuery();

        }
        public void CreateDepot(string name, int benutzerID)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO depot (benutzer_id, name, erstellt) VALUES(@benutzer_id, @name, @erstellt)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@benutzer_id", benutzerID);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@erstellt", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
        public Aktie LoadAktieByID(int aktieID)
        {
            Aktie aktie = null;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT aktienID, Firma, Name, Wert, letzterschluss FROM aktiendaten WHERE aktienID = @aktieID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@aktieID", aktieID);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string name = reader["Name"].ToString();
                double wert = Convert.ToDouble(reader["Wert"]);
                double letzterschluss = Convert.ToDouble(reader["letzterschluss"]);
                string firma = reader["Firma"].ToString();
                aktie = new Aktie(name, firma, wert, aktieID, letzterschluss);
            }
            return aktie;
        }
        public List<Aktie> GetAktienByDepot(int depotID)
        {
            var aktienListe = new List<Aktie>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT aktie_ID FROM transaktion WHERE depot_ID = @depotID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@depotID", depotID);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string aktieID= reader["aktie_ID"].ToString();
                Aktie aktie = LoadAktieByID(Convert.ToInt32(aktieID));
                if (aktie != null)
                {
                    aktienListe.Add(aktie);
                }
            }
            return aktienListe;
        }
        public List<Depot> GetUserDepot(int benutzerID)
        {
            List<Depot> depotList = new List<Depot>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT id, name, erstellt FROM depot WHERE benutzer_id = @benutzer_id";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@benutzer_id", benutzerID);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["id"].ToString();
                string name = reader["name"].ToString();
                Depot depot = new Depot(Convert.ToInt32(id), name);
                if (depot != null)
                    depotList.Add(depot);
            }
            return depotList;
        }
        public void UpdateAktie(Aktie aktie)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE aktiendaten SET Wert = @Wert, letzterschluss = @letzterschluss WHERE Firma = @Firma";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Wert", aktie.CurrentValue);
            cmd.Parameters.AddWithValue("@letzterschluss", aktie.LastClose);
            cmd.Parameters.AddWithValue("@Firma", aktie.firma);
            cmd.ExecuteNonQuery();
        }
        public Aktie LadeAktie(string firma)
        {
            Aktie aktie = null;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT aktienID,Firma, Name, Wert, letzterschluss FROM aktiendaten WHERE Firma = @Firma";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Firma", firma);
            MySqlDataReader reader = cmd.ExecuteReader();
                
            while (reader.Read())
            {
                string name = reader["Name"].ToString();
                double wert = Convert.ToDouble(reader["Wert"]);
                double letzterschluss = Convert.ToDouble(reader["letzterschluss"]);
                string id = reader["aktienID"].ToString();
                aktie = new Aktie(name, firma, wert, Convert.ToInt32(id),letzterschluss);
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

        public List<Transaktion> LadeTransaktionenFürDepot(int depotId)
        {
            List<Transaktion> transaktionen = new List<Transaktion>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = @"SELECT id, aktie_ID, typ, anzahl, einzelpreis, zeitpunkt 
                            FROM transaktion
                            WHERE depot_ID = @depotId 
                            ORDER BY zeitpunkt DESC";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@depotId", depotId);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                int aktieId = reader.GetInt32("aktie_ID");
                string typ = reader.GetString("typ");
                double anzahl = reader.GetDouble("anzahl");
                decimal einzelpreis = reader.GetDecimal("einzelpreis");
                DateTime zeitpunkt = reader.GetDateTime("zeitpunkt");

                Transaktion transaktion = new Transaktion(id, aktieId, anzahl, einzelpreis, typ, zeitpunkt);
                transaktionen.Add(transaktion);
            }
            return transaktionen;
        }

        public void AktieAnlegen(string firma, string name, double startWert)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO aktiendaten (Firma, Name, Wert, letzterschluss) VALUES(@Firma, @Name, @Wert, @letzterschluss)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Firma", firma);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Wert", startWert);
            cmd.Parameters.AddWithValue("@letzterschluss", startWert);
            cmd.ExecuteNonQuery();
        }
    }
}
