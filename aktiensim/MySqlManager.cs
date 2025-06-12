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
    public static class MySqlManager
    {
        public static string connectionString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";

        public static class DepotVerwaltung
        {
            public static void CreateDepot(string name, int benutzerID)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "INSERT INTO depot (benutzer_id, name, erstellt) VALUES(@benutzer_id, @name, @erstellt)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@benutzer_id", benutzerID);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@erstellt", DateTime.Now);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            public static List<Depot> GetUserDepot(int benutzerID)
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
                conn.Close();
                return depotList;
            }
        }
        public static class TransaktionVerwaltung
        {
            public static void AddTransaktion(int aktieId, string typ, double anzahl, decimal einzelpreis, Benutzer activeUser)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "INSERT INTO transaktion (aktie_ID, typ, anzahl, einzelpreis, zeitpunkt, depot_ID) VALUES(@aktie_ID, @typ, @anzahl, @einzelpreis, @zeitpunkt, @depot_ID)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@aktie_ID", aktieId);
                cmd.Parameters.AddWithValue("@typ", typ);
                cmd.Parameters.AddWithValue("@anzahl", anzahl);
                cmd.Parameters.AddWithValue("@einzelpreis", einzelpreis);
                Depot userDepot = MySqlManager.DepotVerwaltung.GetUserDepot(Convert.ToInt32(activeUser.benutzerID)).FirstOrDefault();
                cmd.Parameters.AddWithValue("@depot_id", userDepot.ID);
                cmd.Parameters.AddWithValue("@zeitpunkt", DateTime.Now);
                activeUser.kontoStand -= Convert.ToInt32(Convert.ToDecimal(anzahl) * einzelpreis);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            public static void AktualisiereTransaktion(Transaktion t)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "UPDATE transaktion SET anzahl = @anzahl WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@anzahl", t.anzahl);
                cmd.Parameters.AddWithValue("@id", t.id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            public static void LöscheTransaktion(int transId)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "DELETE FROM transaktion WHERE id = @transId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@transId", transId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            public static List<Transaktion> LadeTransaktionenFürDepot(int depotId)
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
                conn.Close();
                return transaktionen;
            }
        }
        public static class AktienVerwaltung
        { 
            public static Aktie LoadAktieByID(int aktieID)
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
                conn.Close();
                return aktie;
            }
            public static List<Aktie> GetAktienByDepot(int depotID)
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
                    string aktieID = reader["aktie_ID"].ToString();
                    Aktie aktie = LoadAktieByID(Convert.ToInt32(aktieID));
                    if (aktie != null)
                    {
                        aktienListe.Add(aktie);
                    }
                }
                conn.Close();
                return aktienListe;
            }
            public static void UpdateAktie(Aktie aktie)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "UPDATE aktiendaten SET Wert = @Wert, letzterschluss = @letzterschluss WHERE Firma = @Firma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Wert", aktie.CurrentValue);
                cmd.Parameters.AddWithValue("@letzterschluss", aktie.LastClose);
                cmd.Parameters.AddWithValue("@Firma", aktie.firma);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            public static Aktie LadeAktie(string firma)
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
                    aktie = new Aktie(name, firma, wert, Convert.ToInt32(id), letzterschluss);
                }
                conn.Close();
                return aktie;
            }
            public static List<Aktie> LadeAlleAktien()
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
                conn.Close();
                return aktienListe;
            }

           
            public static string[] GetUpdateAktien(int id)
            {
                string[] daten = new string[2];

                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "SELECT Wert, letzterschluss FROM aktiendaten WHERE aktienID = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    daten[0] = reader["Wert"].ToString();
                    daten[1] = reader["letzterschluss"].ToString();

                }
                conn.Close();
                return daten;
            }
            public static void AktieAnlegen(string firma, string name, double startWert)
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
                conn.Close();
            }
        }

        public static class Benutzerverwaltung
        {
            static Benutzer user;
            public static void BenutzerAnlegen(string email, string vName, string nName, string password, string BID, string loginID, Benutzer activeUser)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
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
                using (MySqlConnection connection = new MySqlConnection(connectionString)) //BenutzerID des erstellten Benutzers entnehmen
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
                using (MySqlConnection connection = new MySqlConnection(connectionString)) //Logininfo ergänzen
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
                using (MySqlConnection connection = new MySqlConnection(connectionString))
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

                Benutzer user = new Benutzer(nName, vName, email, BID, 0, null, Kredite.CreditRating.C);
                user.AddKonto(BID, 0);

                string konIdQry = "SELECT KontoID FROM konto WHERE ID_Benutzer = @ID_Benutzer";
                string konIdUpdateQry = "UPDATE benutzer SET ID_Konto = @ID_Konto WHERE BenutzerID = @ID_Benutzer";
                string KontoID = "";

                using (MySqlCommand command = new MySqlCommand(konIdQry, conn))
                {
                    command.Parameters.AddWithValue("ID_Benutzer", BID);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        KontoID = reader["KontoID"].ToString();
                    }
                }
                conn.Close();
                conn.Open();
                using (MySqlCommand command2 = new MySqlCommand(konIdUpdateQry, conn))
                {
                    command2.Parameters.AddWithValue("ID_Benutzer", BID);
                    command2.Parameters.AddWithValue("ID_Konto", KontoID);

                    command2.ExecuteNonQuery();
                }
            }

            public static void BenutzerEinloggen(string email, string password, string inputEmail, string inputPassword, Benutzer activeUser, Panel loginPanel, Panel flowLayoutPanel, Panel homePanel)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                string qryRd = "SELECT * FROM logininfo WHERE Email = @email";


                MySqlCommand cmds = new MySqlCommand(qryRd, conn);
                cmds.Parameters.AddWithValue("email", email);
                MySqlDataReader reader = cmds.ExecuteReader();
                string passwordRead = null;
                while (reader.Read())
                {
                    email = reader["Email"].ToString();
                    passwordRead = reader["passwort"].ToString();
                }

                
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
                conn.Close();
                //Eingabe des Nutzers sollen geholt werden

            }

            public static string Hash(string input)
            {
                var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Concat(hash.Select(b => b.ToString("x2")));
            }
            public static int GetBenutzerKontostand(int kontoID)
            {
                int kontostand = 0;
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "SELECT Kontostand FROM konto WHERE KontoID = @kontoID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kontoID", kontoID);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kontostand = Convert.ToInt32(reader["Kontostand"]);
                }
                conn.Close();
                return kontostand;
            }
            public static List<Benutzer> LadeAlleBenutzer()
            {
                List<Benutzer> benutzer = new List<Benutzer>();
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "SELECT BenutzerID, Name, Vorname, Email, ID_Konto FROM benutzer";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader["BenutzerID"].ToString();
                    string name = reader["Name"].ToString();
                    string vorname = reader["Vorname"].ToString();
                    string email = reader["Email"].ToString();
                    int kontoId = Convert.ToInt32(reader["ID_Konto"]);
                    int kontostand = GetBenutzerKontostand(kontoId);
                    Benutzer user = new Benutzer(name, vorname, email, id, kontostand, null, Kredite.CreditRating.C);
                    if (user != null)
                        benutzer.Add(user);
                }
                conn.Close();
                return benutzer;
            }
            public static Benutzer GetUserByInput(string input)
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = @"
                    SELECT BenutzerID, Name, Vorname, Email 
                    FROM benutzer 
                    WHERE Email = @input 
                       OR Name = @input 
                       OR Vorname = @input
                    LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@input", input);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader["BenutzerID"].ToString();
                    string name = reader["Name"].ToString();
                    string vorname = reader["Vorname"].ToString();
                    string email = reader["Email"].ToString();
                    return new Benutzer(name, vorname, email, id, 0, null, Kredite.CreditRating.C);
                }
                return null;
            }
            public static Benutzer GetUserByEMail(string givenEmail)
            {
                MySqlConnection conn2 = new MySqlConnection(connectionString);
                conn2.Open();
                string sql = $"SELECT BenutzerID, Name, Vorname, Email FROM benutzer WHERE Email = '{givenEmail}'";
                string email = null, benutzerID = null, name = null, vName = null;

                MySqlCommand cmds = new MySqlCommand(sql, conn2);
                MySqlDataReader reader = cmds.ExecuteReader();
                if (reader.Read())
                {
                    email = reader["Email"].ToString();
                    benutzerID = reader["BenutzerID"].ToString();
                    name = reader["Name"].ToString();
                    vName = reader["Vorname"].ToString();
                }
                
                conn2.Close();
                if (givenEmail == email)
                {
                    Benutzer user = new Benutzer(name, vName, email, benutzerID, 0, null, Kredite.CreditRating.C);
                    return user;
                }
                else
                {
                    return null;
                }
            }

            public static Benutzer ReturnActiveUser(Benutzer activeUser)
            {
                activeUser = user;
                return activeUser;
            }

            public static void UpdateBenutzerDaten(string Vorname, string Nachname, string Email, string benutzerID)
            {
                string qry = "UPDATE benutzer SET Vorname = @Vorname WHERE BenutzerID = @benutzerID; UPDATE benutzer SET Name = @Nachname WHERE BenutzerID = @benutzerID; UPDATE benutzer SET Email = @Email WHERE BenutzerID = @benutzerID";
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
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
}
