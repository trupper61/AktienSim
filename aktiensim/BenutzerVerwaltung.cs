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
        static Benutzer user;
        private readonly MySqlConnection connection;
        private string connectionString = "server=localhost;database=aktiensimdb;uid=root;password=";
        public Benutzerverwaltung(MySqlConnection connection)
        {
            this.connection = connection;
        }
        public void BenutzerAnlegen(string email, string vName, string nName, string password, string BID, string loginID, Benutzer activeUser)
        {
            string qry = "INSERT INTO benutzer(Name, Vorname, Email, MitgliedSeit) VALUES(@nName, @vName, @email, @date)";
            string qryLogUpdate = "UPDATE benutzer SET ID_Login = @loginID WHERE Email = @email";
            string qryInfo = "INSERT INTO logininfo(Email, ID_Benutzer, passwort) VALUES(@email, @benutzerid, @passwort)";
            string qryRd = "SELECT * FROM benutzer WHERE Email = @email";
            string qryRdLogIn = "SELECT LoginID FROM logininfo WHERE Email = @email";
            using (MySqlCommand cmd = new MySqlCommand(qry, connection)) //Benutzer erstellen mit allen essenziellen Daten
            {
                cmd.Parameters.AddWithValue("nName", nName);
                cmd.Parameters.AddWithValue("vName", vName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("date", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            using (MySqlCommand cmd = new MySqlCommand(qryRd, connection))
            {
                cmd.Parameters.AddWithValue("email", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        BID = reader["BenutzerID"].ToString();
                    }
                }
            }
            using (MySqlCommand cmd = new MySqlCommand(qryInfo, connection))
            {
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("benutzerid", BID);
                cmd.Parameters.AddWithValue("passwort", password);
                cmd.ExecuteNonQuery();
            }
            //LoginId in benutzer hinzufügen
            using (MySqlCommand cmd = new MySqlCommand(qryRdLogIn, connection))
            {
                cmd.Parameters.AddWithValue("email", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        loginID = reader["LoginID"].ToString();
                    }
                }
            }
            using (MySqlCommand cmd = new MySqlCommand(qryLogUpdate, connection)) //Loginid updaten
            {
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("loginID", loginID);
                cmd.ExecuteNonQuery();
            }

            Benutzer user = new Benutzer(nName, vName, email, BID, 0, null, Kredite.CreditRating.C, 1);
            user.AddKonto(BID, 0, Kredite.CreditRating.C, 1);

            string konIdQry = "SELECT KontoID FROM konto WHERE ID_Benutzer = @ID_Benutzer";
            string konIdUpdateQry = "UPDATE benutzer SET ID_Konto = @ID_Konto WHERE BenutzerID = @ID_Benutzer";
            string KontoID = "";
            using (MySqlCommand command = new MySqlCommand(konIdQry, connection))
            {
                command.Parameters.AddWithValue("ID_Benutzer", BID);

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        KontoID = reader["KontoID"].ToString();
                    }
                }
            }
            using (MySqlCommand command2 = new MySqlCommand(konIdUpdateQry, connection))
            {
                command2.Parameters.AddWithValue("ID_Benutzer", BID);
                command2.Parameters.AddWithValue("ID_Konto", KontoID);

                command2.ExecuteNonQuery();
            }
        }

        public void BenutzerEinloggen(string email, string password, string inputEmail, string inputPassword, Benutzer activeUser, Panel loginPanel, Panel flowLayoutPanel, Panel homePanel)
        {

            string qryRd = "SELECT * FROM logininfo WHERE Email = @email";


            using (MySqlCommand cmd = new MySqlCommand(qryRd, connection))
            {
                cmd.Parameters.AddWithValue("email", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    string passwordRead = null;
                    while (reader.Read())
                    {
                        email = reader["Email"].ToString();
                        passwordRead = reader["passwort"].ToString();
                    }
                    reader.Close();

                    if (email == null || password == null)
                    {
                        MessageBox.Show("No Data");
                        return;
                    }
                    string passHash = Hash(password);
                    if (passwordRead == null)
                    {
                        MessageBox.Show("Please Register!");
                        return;
                    }
                    if (inputEmail == email && passHash == passwordRead)
                    {
                        MessageBox.Show("Login erfolgreich!");
                        loginPanel.Visible = false;
                        flowLayoutPanel.Visible = true;
                        if (homePanel == null)
                            return;
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
                }
            }
            //Eingabe des Nutzers sollen geholt werden

        }

        public string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        public int GetBenutzerKontostand(int kontoID)
        {
            int kontostand = 0;
            string query = "SELECT Kontostand FROM konto WHERE KontoID = @kontoID";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@kontoID", kontoID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kontostand = Convert.ToInt32(reader["Kontostand"]);

                        }
                    }
                }
            }
            return kontostand;
        }
        public List<Benutzer> LadeAlleBenutzer()
        {
            List<Benutzer> benutzer = new List<Benutzer>();
            string query = "SELECT BenutzerID, Name, Vorname, Email, ID_Konto, KreditRating, KreditScore FROM benutzer, konto";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader["BenutzerID"].ToString();
                        string name = reader["Name"].ToString();
                        string vorname = reader["Vorname"].ToString();
                        string email = reader["Email"].ToString();
                        int kontoId = Convert.ToInt32(reader["ID_Konto"]);
                        int kontostand = GetBenutzerKontostand(kontoId);
                        Kredite.CreditRating rating = (Kredite.CreditRating)Enum.Parse(typeof(Kredite.CreditRating), reader["KreditRating"].ToString());
                        int score = Convert.ToInt32(reader["KreditScore"]);
                        Benutzer user = new Benutzer(name, vorname, email, id, kontostand, null, rating, score);
                        if (user != null)
                            benutzer.Add(user);
                    }
                }
            }
            return benutzer;
        }
        public Benutzer GetUserByInput(string input)
        {
            string query = @"
                SELECT BenutzerID, Name, Vorname, Email, KreditRating, KreditScore
                FROM benutzer, konto
                WHERE Email = @input 
                    OR Name = @input 
                    OR Vorname = @input
                LIMIT 1";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@input", input);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader["BenutzerID"].ToString();
                        string name = reader["Name"].ToString();
                        string vorname = reader["Vorname"].ToString();
                        string email = reader["Email"].ToString();
                        Kredite.CreditRating rating = (Kredite.CreditRating)Enum.Parse(typeof(Kredite.CreditRating), reader["KreditRating"].ToString());
                        int score = Convert.ToInt32(reader["KreditScore"]);
                        return new Benutzer(name, vorname, email, id, 0, null, rating, score);
                    }
                }
            }
            return null;
        }
        public Benutzer GetUserByEMail(string givenEmail)
        {
            string email = null, benutzerID = null, name = null, vName = null;
            Kredite.CreditRating rating = Kredite.CreditRating.C;
            int score = 0;
            string sql = $"SELECT BenutzerID, Name, Vorname, Email, KreditRating, KreditScore FROM benutzer, konto WHERE Email = @email";

            using (MySqlCommand cmds = new MySqlCommand(sql, connection))
            {
                cmds.Parameters.AddWithValue("@email", givenEmail);
                using (MySqlDataReader reader = cmds.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        email = reader["Email"].ToString();
                        benutzerID = reader["BenutzerID"].ToString();
                        name = reader["Name"].ToString();
                        vName = reader["Vorname"].ToString();

                        rating = (Kredite.CreditRating)Enum.Parse(typeof(Kredite.CreditRating), reader["KreditRating"].ToString());
                        score = Convert.ToInt32(reader["KreditScore"]);
                    }
                }
            }
            if (email != null && givenEmail == email)
            {
                Benutzer user = new Benutzer(name, vName, email, benutzerID, 0, null, rating, score);
                return user;
            }
            else
            {
                return null;
            }
        }
        public Benutzer GetBenutzerById(int id)
        {
            string query = "SELECT * FROM benutzer, konto WHERE benutzerID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string benutzerId = reader["BenutzerID"].ToString();
                        string vorname = reader["Vorname"].ToString();
                        string name = reader["Name"].ToString();
                        string email = reader["Email"].ToString();
                        int kontoId = Convert.ToInt32(reader["ID_Konto"]);
                        int konto = GetBenutzerKontostand(kontoId);
                        Kredite.CreditRating rating = (Kredite.CreditRating)Enum.Parse(typeof(Kredite.CreditRating), reader["KreditRating"].ToString());
                        int score = Convert.ToInt32(reader["KreditScore"]);
                        return new Benutzer(name, vorname, email, benutzerId, konto, null, rating, score); // TODO
                    }
                }
            }
            return null;
        }
        public Benutzer ReturnActiveUser(Benutzer activeUser)
        {
            activeUser = user;
            return activeUser;
        }

        public void UpdateBenutzerDaten(string Vorname, string Nachname, string Email, string benutzerID)
        {
            string qry = "UPDATE benutzer SET Vorname = @Vorname WHERE BenutzerID = @benutzerID; UPDATE benutzer SET Name = @Nachname WHERE BenutzerID = @benutzerID; UPDATE benutzer SET Email = @Email WHERE BenutzerID = @benutzerID";
            using (MySqlCommand cmd = new MySqlCommand(qry, connection))
            {
                cmd.Parameters.AddWithValue("Vorname", Vorname);
                cmd.Parameters.AddWithValue("Nachname", Nachname);
                cmd.Parameters.AddWithValue("Email", Email);
                cmd.Parameters.AddWithValue("benutzerID", benutzerID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
