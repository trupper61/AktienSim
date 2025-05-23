using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    internal class Benutzerverwaltung
    {
        public void BenutzerAnlegen(string email, string vName, string nName, string password, string BID, string loginID)
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            string qry = "INSERT INTO benutzer(Name, Vorname, Email, MitgliedSeit) VALUES(@nName, @vName, @email, @date)";
            string qryLogUpdate = "UPDATE benutzer SET ID_Login = @loginID WHERE Email = @email";
            string qryInfo = "INSERT INTO logininfo(Email, ID_Benutzer, passwort) VALUES(@email, @benutzerid, @passwort)";
            string qryRd = "SELECT * FROM benutzer WHERE Email = @email";
            string qryRdLogIn = "SELECT LoginID FROM logininfo WHERE Email = @email";

            using (MySqlCommand cmd = new MySqlCommand(qry, conn)) //Benutzer erstellen mit allen essenziellen Daten
            {
                cmd.Parameters.AddWithValue("nName", nName);
                cmd.Parameters.AddWithValue("vName", vName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("date", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            using (MySqlConnection connection = new MySqlConnection(connString)) //BenutzerID des erstellten Benutzers entnehmen
            {
                connection.Open();
                MySqlCommand cmds = new MySqlCommand(qryRd, connection);

                cmds.Parameters.AddWithValue("email", email);
                MySqlDataReader reader = cmds.ExecuteReader();

                if (reader.Read())
                {
                    BID = reader["BenutzerID"].ToString();
                }
            }
            using (MySqlConnection connection = new MySqlConnection(connString)) //Logininfo ergänzen
            {
                connection.Open();

                using (MySqlCommand cmd = new MySqlCommand(qryInfo, conn))
                {
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("benutzerid", BID);
                    cmd.Parameters.AddWithValue("passwort", password);
                    cmd.ExecuteNonQuery();
                }
            }
            //LoginId in benutzer hinzufügen
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                MySqlCommand cmds = new MySqlCommand(qryRdLogIn, connection);

                cmds.Parameters.AddWithValue("email", email);
                MySqlDataReader reader = cmds.ExecuteReader();

                if (reader.Read())
                {
                    loginID = reader["LoginID"].ToString();
                }
            }
            using (MySqlCommand cmd = new MySqlCommand(qryLogUpdate, conn)) //Loginid updaten
            {
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("loginID", loginID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
