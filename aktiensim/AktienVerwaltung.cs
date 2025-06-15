using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class AktienVerwaltung
    {
        private readonly MySqlConnection connection;

        public AktienVerwaltung(MySqlConnection connection)
        {
            this.connection = connection;
        }
        public Aktie LoadAktieByID(int aktieID)
        {
            Aktie aktie = null;
            using (var myMan = new MySqlManager())
            { 
            string query = "SELECT aktienID, Firma, Name, Wert, letzterschluss FROM aktiendaten WHERE aktienID = @aktieID";
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@aktieID", aktieID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            double wert = Convert.ToDouble(reader["Wert"]);
                            double letzterschluss = Convert.ToDouble(reader["letzterschluss"]);
                            string firma = reader["Firma"].ToString();
                            aktie = new Aktie(name, firma, wert, aktieID, letzterschluss);
                        }
                    }
                }
            }
            return aktie;
        }
        public List<Aktie> GetAktienByDepot(int depotID)
        {
            var aktienListe = new List<Aktie>();
            string query = "SELECT aktie_ID FROM transaktion WHERE depot_ID = @depotID";
            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@depotID", depotID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string aktieID = reader["aktie_ID"].ToString();
                            Aktie aktie = LoadAktieByID(Convert.ToInt32(aktieID));
                            if (aktie != null)
                            {
                                aktienListe.Add(aktie);
                            }
                        }
                    }
                }
            }
            return aktienListe;
        }
        public void UpdateAktie(Aktie aktie)
        {
            string query = "UPDATE aktiendaten SET Wert = @Wert, letzterschluss = @letzterschluss WHERE Firma = @Firma";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Wert", aktie.CurrentValue);
                cmd.Parameters.AddWithValue("@letzterschluss", aktie.LastClose);
                cmd.Parameters.AddWithValue("@Firma", aktie.firma);
                cmd.ExecuteNonQuery();
            }
        }
        public Aktie LadeAktie(string firma)
        {
            Aktie aktie = null;
            using (var myMan = new MySqlManager())
            { 
            string query = "SELECT aktienID,Firma, Name, Wert, letzterschluss FROM aktiendaten WHERE Firma = @Firma";
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@Firma", firma);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            double wert = Convert.ToDouble(reader["Wert"]);
                            double letzterschluss = Convert.ToDouble(reader["letzterschluss"]);
                            string id = reader["aktienID"].ToString();
                            aktie = new Aktie(name, firma, wert, Convert.ToInt32(id), letzterschluss);
                        }
                    }
                }
            }
            return aktie;
        }
        public List<Aktie> LadeAlleAktien()
        {
            var aktienListe = new List<Aktie>();
            string query = "SELECT Firma FROM aktiendaten";
            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firma = reader["Firma"].ToString();
                            Aktie aktie = LadeAktie(firma);
                            if (aktie != null)
                            {
                                aktienListe.Add(aktie);
                            }
                        }
                    }
                }
            }
            return aktienListe;
        }


        public string[] GetUpdateAktien(int id)
        {
            string[] daten = new string[2];

            string query = "SELECT Wert, letzterschluss FROM aktiendaten WHERE aktienID = @id";
            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            daten[0] = reader["Wert"].ToString();
                            daten[1] = reader["letzterschluss"].ToString();

                        }
                    }
                }
            }
            return daten;
        }
        public void AktieAnlegen(string firma, string name, double startWert)
        {
            string query = "INSERT INTO aktiendaten (Firma, Name, Wert, letzterschluss) VALUES(@Firma, @Name, @Wert, @letzterschluss)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Firma", firma);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Wert", startWert);
                cmd.Parameters.AddWithValue("@letzterschluss", startWert);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
