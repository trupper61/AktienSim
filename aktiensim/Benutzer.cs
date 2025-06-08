using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aktiensim
{
    public class Benutzer
    {
        public string name;
        public string vorname;
        public string email;
        public string benutzerID;
        public double kontoStand;
        public Kredite.CreditRating rating;
        public List<Kredite> kredite;
        public Benutzer (string name, string vorname, string email, string benutzerID, int kontoStand, List<Kredite> Kredite, Kredite.CreditRating Rating)
        {
            this.name = name;
            this.vorname = vorname;
            this.email = email;
            this.benutzerID = benutzerID;
            this.kontoStand = kontoStand;
            GetKontoStand(this);
            Kredite = new List<Kredite>();
            this.kredite = Kredite;
            this.rating = Rating;
        }

        public void AddKonto(string ID_Benutzer, double Kontostand) 
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            string qry = "INSERT INTO konto(ID_Benutzer, Kontostand) VALUES(@ID_Benutzer, @Kontostand)";

            ID_Benutzer = this.benutzerID;
            Kontostand = this.kontoStand;

            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            using(MySqlCommand cmd = new MySqlCommand(qry, conn)) 
            {
                cmd.Parameters.AddWithValue("ID_Benutzer", ID_Benutzer);
                cmd.Parameters.AddWithValue("Kontostand", Kontostand);
                cmd.ExecuteNonQuery();
            }
        }

        public void GeldHinzufuegen(double anzahl) //Die Person erhält Geld für testzwecke.
        {
            this.kontoStand += anzahl;

            double stand = this.kontoStand;
            string BID = this.benutzerID;
            UpdateKontoStand(stand, BID);
        }

        public void UpdateKontoStand(double stand, string BID) //Wenn der Kontostand des Nutzers verändert wird, soll sich dieser ebenfalls in der Datenbank anpassen.
        {
            //Update den Kontostand in der Datenbank.
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            string qry = "UPDATE Konto SET Kontostand = @Kontostand WHERE ID_Benutzer = @ID_Benutzer";

            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            using(MySqlCommand cmd = new MySqlCommand(qry, conn)) 
            {
                cmd.Parameters.AddWithValue("Kontostand", stand);
                cmd.Parameters.AddWithValue("ID_Benutzer", BID);
                cmd.ExecuteNonQuery();
            }
        }
        public void GetKontoStand(Benutzer benutzer) 
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            string qry = "SELECT Kontostand FROM Konto WHERE ID_Benutzer = @ID_Benutzer";

            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            using(MySqlCommand cmd = new MySqlCommand(qry, conn)) 
            {
                cmd.Parameters.AddWithValue("ID_Benutzer", benutzer.benutzerID);
                MySqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    this.kontoStand = Convert.ToInt32(reader["Kontostand"]);
                }
            }
        }
    }
}
