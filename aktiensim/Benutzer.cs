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
        public Kredite.CreditRating rating; //Vertrauenswürdigkeit des Kreditnehmers - Wird in Tabelle "Konto" gespeichert
        public int score;
        public List<Kredite> kredite;
        public Benutzer (string name, string vorname, string email, string benutzerID, int kontoStand, List<Kredite> Kredite, Kredite.CreditRating Rating, int Score)
        {
            this.name = name;
            this.vorname = vorname;
            this.email = email;
            this.benutzerID = benutzerID;
            this.kontoStand = kontoStand;

            GetKontoStand();
            Kredite = new List<Kredite>();
            this.kredite = Kredite;
            this.rating = Rating;
            this.score = Score;
        }

        public void AddKonto(string ID_Benutzer, double Kontostand, Kredite.CreditRating rating, int score) //Nach Registrierung des Benutzers erhält der Benutzer ein Konto
        {
            string qry = "INSERT INTO konto(ID_Benutzer, Kontostand, KreditRating, KreditScore) VALUES(@ID_Benutzer, @Kontostand, @rating, @score)";

            ID_Benutzer = this.benutzerID;
            Kontostand = this.kontoStand;

            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(qry, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("ID_Benutzer", ID_Benutzer);
                    cmd.Parameters.AddWithValue("Kontostand", Kontostand);
                    cmd.Parameters.AddWithValue("rating", rating);
                    cmd.Parameters.AddWithValue("score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void GeldHinzufuegen(double anzahl)
        {
            this.kontoStand += anzahl;

            double stand = this.kontoStand;
            string BID = this.benutzerID;
            UpdateKontoStand(stand, BID);
        }
        public void GeldAbziehen(double anzahl)
        {
            this.kontoStand -= anzahl;

            double stand = this.kontoStand;
            string BID = this.benutzerID;
            UpdateKontoStand(stand, BID);
            
        }

        public void UpdateKontoStand(double stand, string BID) //Wenn der Kontostand des Nutzers verändert wird, soll sich dieser ebenfalls in der Datenbank anpassen.
        {
            int score = CheckZahlungsfaehigkeit();
            string rating = CheckCreditRating();

            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            string qry = "UPDATE konto SET Kontostand = @Kontostand WHERE ID_Benutzer = @ID_Benutzer; UPDATE konto SET KreditRating = @rating WHERE ID_Benutzer = @ID_Benutzer; UPDATE konto SET KreditScore = @score WHERE ID_Benutzer = @ID_Benutzer;";

            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(qry, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("Kontostand", stand);
                    cmd.Parameters.AddWithValue("ID_Benutzer", BID);
                    cmd.Parameters.AddWithValue("score", score);
                    cmd.Parameters.AddWithValue("rating", rating);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public double GetKontoStand() //Holt aktuellen Wert des Kontos
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            string qry = "SELECT Kontostand, KreditRating, KreditScore FROM Konto WHERE ID_Benutzer = @ID_Benutzer";

            using (var myMan = new MySqlManager())
            {
                using (MySqlCommand cmd = new MySqlCommand(qry, myMan.Connection))
                {
                    cmd.Parameters.AddWithValue("ID_Benutzer", this.benutzerID);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        this.kontoStand = Convert.ToInt32(reader["Kontostand"]);
                        this.rating = (Kredite.CreditRating)Enum.Parse(typeof(Kredite.CreditRating), reader["KreditRating"].ToString());
                        this.score = Convert.ToInt32(reader["KreditScore"]);
                    }
                }
                return this.kontoStand;
            }
        }

        public int CheckZahlungsfaehigkeit() 
        {
            if(this.kontoStand < 0) 
            {
                return this.score -= 10;
            }
            if (this.kontoStand > 0 && this.kredite.Count == 0)
            {
                return this.score += 10;
            }
            return this.score;
        }

        public string CheckCreditRating() 
        {
            if(this.score < 50 && this.kontoStand < -2000) 
            {
                this.rating = Kredite.CreditRating.D;
                return this.rating.ToString();
            }
            else if(this.score > 50 && this.score < 70) 
            {
                this.rating = Kredite.CreditRating.C;
            }
            else if (this.score > 70 && this.score < 90)
            {
                this.rating = Kredite.CreditRating.B;
            }
            else if (this.score > 90)
            {
                this.rating = Kredite.CreditRating.A;
            }
            return this.rating.ToString();
        }
    }
}
