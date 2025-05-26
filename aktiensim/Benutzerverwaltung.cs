using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aktiensim
{
    public class Benutzerverwaltung
    {
        Benutzer user;
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

        public void BenutzerEinloggen(string email, string password, string inputEmail, string inputPassword, Benutzer activeUser, Panel loginPanel, Panel flowLayoutPanel, Panel homePanel)
        {
            string passHash = Hash(password);
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            string qryRd = "SELECT * FROM logininfo WHERE Email = @email";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                MySqlCommand cmds = new MySqlCommand(qryRd, connection);
                cmds.Parameters.AddWithValue("email", email);
                MySqlDataReader reader = cmds.ExecuteReader();

                if (reader.Read())
                {
                    email = reader["Email"].ToString();
                    password = reader["passwort"].ToString();
                }

            }
            if (email == null || password == null)
            {
                MessageBox.Show("No Data");
                return;
            }
            if (inputEmail == email && passHash == password)
            {
                MessageBox.Show("Login erfolgreich!");
                loginPanel.Visible = false;
                flowLayoutPanel.Visible = true;
                homePanel.Visible = true;
                Benutzer tmpUser = GetUserByEMail(email);
                if (tmpUser != null)
                {
                    activeUser = tmpUser;
                    user = activeUser;
                }
            }
            else
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
            //Eingabe des Nutzers sollen geholt werden

        }

        public string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public Benutzer GetUserByEMail(string givenEmail)
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();
            string sql = $"SELECT BenutzerID, Name, Vorname, Email FROM benutzer WHERE Email = '{givenEmail}'";
            string email = null, benutzerID = null, name = null, vName = null;
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                MySqlCommand cmds = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmds.ExecuteReader();
                if (reader.Read())
                {
                    email = reader["Email"].ToString();
                    benutzerID = reader["BenutzerID"].ToString();
                    name = reader["Name"].ToString();
                    vName = reader["Vorname"].ToString();
                }
            }
            if (givenEmail == email)
            {
                Benutzer user = new Benutzer(name, vName, email, Convert.ToInt32(benutzerID), 0);
                return user;
            }
            else
            {
                return null;
            }
        }

        public Benutzer ReturnActiveUser(Benutzer activeUser) 
        {
            activeUser = user;
            return activeUser;
        }
    }
}
