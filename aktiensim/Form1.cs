﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using ScottPlot.WinForms;

namespace aktiensim
{
    public partial class Form1 : Form
    {
        Panel loginPanel, registerPanel;
        TextBox vNameInput;
        TextBox nNameInput;
        TextBox loginEmailInput, loginPasswordInput;
        TextBox passwdInput;
        TextBox passwdCheckInput;
        FlowLayoutPanel flowLayoutPanel;
        Panel homePanel;
        public Benutzer activeUser;
        FormsPlot plot;
        List<Aktie> stonks;
        public Form1()
        {
            InitializeComponent();
            InitLoginUi();
            InitRegisterUI();
            InitUI();
            stonks = new List<Aktie>() { new Aktie("DAX"), new Aktie("DHL"), new Aktie("Lufthansa")};
        }
        public void InitUI()
        {
            flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Size = new Size(100, this.ClientSize.Height),
                FlowDirection = FlowDirection.TopDown,
                BackColor = Color.DarkCyan,
                Visible = false
            };
            Label menuLb = new Label
            {
                Text = "Menü",
                AutoSize = true,
                Font = new Font("Arial", 15, FontStyle.Bold),
                ForeColor = Color.White
            };
            flowLayoutPanel.Controls.Add(menuLb);
            Button homeBtn = new Button
            {
                Text = "Home",
                Size = new Size(80, 40),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Sans-Serif", 10)
            };
            homeBtn.Click += (s, e) =>
            {
                homePanel.Controls.Clear();
                ShowGraphs();
                Timer timer1 = new Timer();
                timer1.Tick += timer_Tick;
                timer1.Interval = 200;
                timer1.Start();
            };
            flowLayoutPanel.Controls.Add(homeBtn);
            Button profileBtn = new Button
            {
                Text = "Profile",
                Size = new Size(80, 40),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Sans-Serif", 10)
            };
            profileBtn.Click += (s, e) =>
            {
                homePanel.Controls.Clear();
                Label lb = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(15, 10),
                    Text = $"Hallo, {activeUser.vorname} {activeUser.name}"
                };
                homePanel.Controls.Add(lb);
            };
            flowLayoutPanel.Controls.Add(profileBtn);

            homePanel = new Panel
            {
                Size = new Size(this.Size.Width - flowLayoutPanel.Width, this.Size.Height),
                Location = new Point(flowLayoutPanel.Right, 0),
                BackColor = Color.LightCyan,
                Visible = false
            };
            Controls.Add(homePanel);
            Controls.Add(flowLayoutPanel);
        }
        public void InitLoginUi()
        {
            loginPanel = new Panel
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.LightBlue
            };
            Label loginLb = new Label
            {
                Text = "Login",
                Font = new Font("Arial", 16),
                Location = new Point(150, 20),
                AutoSize = true
            };
            loginPanel.Controls.Add(loginLb);
            loginEmailInput = new TextBox
            {
                Text = "Email...",
                ForeColor = Color.Gray,
                Location = new Point(100, 60),
                Size = new Size(200, 22)
            };
            loginEmailInput.GotFocus += (s, e) =>
            {
                if (loginEmailInput.Text == "Email...")
                {
                    loginEmailInput.Text = "";
                    loginEmailInput.ForeColor = Color.Black;
                }
            };
            loginEmailInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(loginEmailInput.Text))
                {
                    loginEmailInput.Text = "Email...";
                    loginEmailInput.ForeColor = Color.Gray;
                }
            };
            loginPanel.Controls.Add(loginEmailInput);
            loginPasswordInput = new TextBox
            {
                Text = "Passwort...",
                ForeColor = Color.Gray,
                Location = new Point(100, 90),
                Size = new Size(200, 22),
            };
            loginPasswordInput.GotFocus += (s, e) =>
            {
                if (loginPasswordInput.Text == "Passwort...")
                {
                    loginPasswordInput.Text = "";
                    loginPasswordInput.ForeColor = Color.Black;
                }
            };
            loginPasswordInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(loginPasswordInput.Text))
                {
                    loginPasswordInput.Text = "Passwort...";
                    loginPasswordInput.ForeColor = Color.Gray;
                }
            };
            loginPanel.Controls.Add(loginPasswordInput);
            Button loginBtn = new Button
            {
                Text = "Login",
                Location = new Point(100, 130),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            loginBtn.Click += LoginBtn_Click;
            loginPanel.Controls.Add(loginBtn);
            Button registerBtn = new Button
            {
                Text = "Hier, zum Registrieren",
                Location = new Point(100, 170),
                Size = new Size(200, 30),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            registerBtn.Click += (s, e) =>
            {
                loginPanel.Visible = false;
                registerPanel.Visible = true;
            };
            loginPanel.Controls.Add(registerBtn);
            Controls.Add(loginPanel);
        }
        public void InitRegisterUI()
        {
            registerPanel = new Panel
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.LightGreen,
                Visible = false
            };
            Label registerLabel = new Label
            {
                Text = "Registrieren",
                Font = new Font("Arial", 16),
                Location = new Point(150, 20),
                AutoSize = true
            };
            registerPanel.Controls.Add(registerLabel);
            vNameInput = new TextBox
            {
                Text = "Vorname...",
                ForeColor = Color.Gray,
                Location = new Point(100, 60),
                Size = new Size(200, 22)
            };
            vNameInput.GotFocus += (s, e) =>
            {
                if (vNameInput.Text == "Vorname...")
                {
                    vNameInput.Text = "";
                    vNameInput.ForeColor = Color.Black;
                }
            };
            vNameInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(vNameInput.Text))
                {
                    vNameInput.Text = "Vorname...";
                    vNameInput.ForeColor = Color.Gray;
                }
            };
            registerPanel.Controls.Add(vNameInput);
            nNameInput = new TextBox
            {
                Text = "Nachname...",
                ForeColor = Color.Gray,
                Location = new Point(100, 90),
                Size = new Size(200, 22)
            };
            nNameInput.GotFocus += (s, e) =>
            {
                if (nNameInput.Text == "Nachname...")
                {
                    nNameInput.Text = "";
                    nNameInput.ForeColor = Color.Black;
                }
            };
            nNameInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(nNameInput.Text))
                {
                    nNameInput.Text = "Vorname...";
                    nNameInput.ForeColor = Color.Gray;
                }
            };
            registerPanel.Controls.Add(nNameInput);
            emailInput = new TextBox
            {
                Text = "Email...",
                ForeColor = Color.Gray,
                Location = new Point(100, 120),
                Size = new Size(200, 22)
            };
            emailInput.GotFocus += (s, e) =>
            {
                if (emailInput.Text == "Email...")
                {
                    emailInput.Text = "";
                    emailInput.ForeColor = Color.Black;
                }
            };
            emailInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(emailInput.Text))
                {
                    emailInput.Text = "Email...";
                    emailInput.ForeColor = Color.Gray;
                }
            };
            registerPanel.Controls.Add(emailInput);
            passwdInput = new TextBox
            {
                Text = "Passwort...",
                ForeColor = Color.Gray,
                Location = new Point(100, 150),
                Size = new Size(200, 22)
            };
            passwdInput.GotFocus += (s, e) =>
            {
                if (passwdInput.Text == "Passwort...")
                {
                    passwdInput.Text = "";
                    passwdInput.ForeColor = Color.Black;
                }

            };
            passwdInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(passwdInput.Text))
                {
                    passwdInput.Text = "Passwort...";
                    passwdInput.ForeColor = Color.Gray;
                }

            };
            registerPanel.Controls.Add(passwdInput);
            passwdCheckInput = new TextBox
            {
                Text = "Passwort wiederholen...",
                ForeColor = Color.Gray,
                Location = new Point(100, 180),
                Size = new Size(200, 22)
            };
            passwdCheckInput.GotFocus += (s, e) =>
            {
                if (passwdCheckInput.Text == "Passwort wiederholen...")
                {
                    passwdCheckInput.Text = "";
                    passwdCheckInput.ForeColor = Color.Black;
                }
            };
            passwdCheckInput.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(passwdCheckInput.Text))
                {
                    passwdCheckInput.Text = "Passwort wiederholen...";
                    passwdCheckInput.ForeColor = Color.Gray;
                }
            };
            registerPanel.Controls.Add(passwdCheckInput);
            Button registerBtn = new Button
            {
                Text = "Registrieren",
                Location = new Point(100, 220),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            registerBtn.Click += RegisterBtn_Click;
            registerPanel.Controls.Add(registerBtn);
            Button loginBtn = new Button
            {
                Text = "Zurück zum Login",
                Location = new Point(100, 260),
                Size = new Size(200, 30),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            loginBtn.Click += (s, e) =>
            {
                registerPanel.Visible = false;
                loginPanel.Show();
            };
            registerPanel.Controls.Add(loginBtn);
            Controls.Add(registerPanel);
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string loginID = "";
            string BID = "";
            string email = emailInput.Text;
            string vName = vNameInput.Text;
            string nName = nNameInput.Text;
            string password = passwdInput.Text;
            string passwdCheck = passwdCheckInput.Text;
            if (email == "" || vName == "" || nName == "" || password == "" || passwdCheck == "")
            {
                MessageBox.Show("Alle Felder wurden nicht ausgefüllt!");
                return;
            }
            if (password != passwdCheck)
            {
                MessageBox.Show("Passwörter stimmen nicht überein!");
                return;
            }
            string passHash = Hash(password);
            MessageBox.Show(passHash);
            BenutzerAnlegen(email, vName, nName, passHash, BID, loginID);
            MessageBox.Show("Bitte, logen Sie sich ein");
            registerPanel.Visible = false;
            loginPanel.Visible = true;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            string email = loginEmailInput.Text;
            string password = loginPasswordInput.Text;

            if (email == "" || password == "")
            {
                MessageBox.Show("Alle Felder wurden nicht ausgefüllt!");
                return;
            }
            BenutzerEinloggen(email, password);
        }
        //Credits: https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
        static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public void BenutzerAnlegen(string email, string vName, string nName, string password, string BID, string loginID) 
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            string qry = "INSERT INTO benutzer(Name, Vorname, Email, MitgliedSeit) VALUES(@nName, @vName, @email, @date)";
            string qry2 = "UPDATE benutzer SET ID_Login = '@loginID' WHERE Email = @email";
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

                if(reader.Read()) 
                {
                    loginID = reader["LoginID"].ToString();
                }
            }
            using (MySqlCommand cmd = new MySqlCommand(qry2, conn)) //Benutzer erstellen mit allen essenziellen Daten
            {
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("loginID", loginID);
                cmd.ExecuteNonQuery();
            }
        }

        public void BenutzerEinloggen(string email, string password)
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
            if(loginEmailInput.Text == email && passHash == password) 
            {
                MessageBox.Show("Login erfolgreich!");
                loginPanel.Visible = false;
                flowLayoutPanel.Visible = true;
                homePanel.Visible = true;
                Benutzer tmpUser = GetUserByEMail(email);
                if (tmpUser != null)
                {
                    activeUser = tmpUser;
                }
            }
            else
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
            //Eingabe des Nutzers sollen geholt werden

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
                Benutzer user = new Benutzer(name, vName, email, Convert.ToInt32(benutzerID));
                return user;
            }
            else
            {
                return null;
            }
        }
        public void ShowGraphs()
        {
            int x = 10;
            foreach (Aktie a in stonks)
            {
                a.plot.Plot.HideAxesAndGrid();
                a.plot.Location = new Point(x, 0);
                a.plot.Plot.Title(a.name);
                homePanel.Controls.Add(a.plot);
                x += 160;
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            foreach(Aktie a in stonks)
            {
                a.UpdateChart();
            }
        }
    }
}