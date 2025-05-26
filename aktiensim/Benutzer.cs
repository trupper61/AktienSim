using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Benutzer
    {
        public string name;
        public string vorname;
        public string email;
        public string benutzerID;
        public int kontoStand;
        public Benutzer (string name, string vorname, string email, string benutzerID, int kontoStand)
        {
            this.name = name;
            this.vorname = vorname;
            this.email = email;
            this.benutzerID = benutzerID;
            this.kontoStand = kontoStand;
        }

        public void AddKonto(string ID_Benutzer, int Kontostand) 
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

        public void UpdateKontoStand() //Wenn der Kontostand des Nutzers verändert wird, soll sich dieser ebenfalls in der Datenbank anpassen.
        {
            
        }
    }
}
