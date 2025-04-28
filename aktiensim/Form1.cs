using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace aktiensim
{
    public partial class Form1 : Form
    {
        public Panel loginPanel;
        public Form1()
        {
            InitializeComponent();
        }
        public void InitUi()
        {
            loginPanel = new Panel
            {
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Color.LightBlue
            };
            TextBox box = new TextBox
            {
                Size = new Size(100, 30),
                Location = new Point((loginPanel.Width - 80)/2, (loginPanel.Height -80)/2),
                Text = "Email..."
            };
            loginPanel.Controls.Add(box);
            Controls.Add(loginPanel);
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            int BID = 0;
            string email = emailInput.Text;
            string vName = vornameInput.Text;
            string nName = nachnameInput.Text;
            string password = passwortInput.Text;
            string passwdCheck = passwortCheck.Text;
            if (email == "" || vName == "" || nName == "" || password == "" || passwdCheck == "")
            {
                MessageBox.Show("Alle Felder wurden nicht ausgefüllt");
                return;
            }
            if (password != passwdCheck)
            {
                MessageBox.Show("Passwörter stimmen nicht über ein!");
                return;
            }
            string passHash = Hash(password);
            MessageBox.Show(passHash);

            BenutzerAnlegen(email, vName, nName, password, passwdCheck, BID);
        }
        //Credits: https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
        static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public void BenutzerAnlegen(string email, string vName, string nName, string password, string passwdCheck, int BID) 
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            string qry = "INSERT INTO benutzer(Name, Vorname, Email, MitgliedSeit) VALUES(@nName, @vName, @email, @date)";
            string qryInfo = "INSERT INTO logininfo(Email, ID_Benutzer, passwort) VALUES(@email, @benutzerid ,@passwort)";
            string qryRd = "SELECT BenutzerID FROM benutzer WHERE Email = @email";

            using (MySqlCommand cmd = new MySqlCommand(qry, conn)) //Benutzer erstellen
            {
                cmd.Parameters.AddWithValue("nName", nName);
                cmd.Parameters.AddWithValue("vName", vName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("date", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            using(MySqlCommand cmd = new MySqlCommand(qryRd, conn)) //BenutzerID des erstellten Benutzers entnehmen
            {
                cmd.Parameters.AddWithValue("email", email);
                BID = cmd.ExecuteNonQuery();
                MessageBox.Show(cmd.ExecuteNonQuery().ToString());
            }
            using (MySqlCommand cmd = new MySqlCommand(qryInfo, conn))
            {
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("benutzerid", BID); //Lese die Benutzer ID des erstellten Benutzers aus und Füge sie hinzu
                cmd.Parameters.AddWithValue("passwort", password);
                cmd.ExecuteNonQuery();
            }
        }
    }
}