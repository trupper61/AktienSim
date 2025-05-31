using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
        public Panel kaufPanel;
        Benutzerverwaltung benutzerverwaltung = new Benutzerverwaltung();
        public Form1()
        {
            InitializeComponent();
            InitLoginUi();
            InitRegisterUI();
            InitUI();
            AktienVerwaltung stonkManage = new AktienVerwaltung("server=localhost;database=aktiensimdb;uid=root;password=\"\"");
            stonks = stonkManage.LadeAlleAktien(); // Loads all stonks in Database
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
                ShowHomePanel();
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
                int y = 10;
                homePanel.Controls.Clear();
                Label lb = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(15, y),
                    Text = $"Hallo, {benutzerverwaltung.ReturnActiveUser(activeUser).vorname} {benutzerverwaltung.ReturnActiveUser(activeUser).name}"
                };
                Label kontostand = new Label
                {
                    AutoSize = true,
                    ForeColor = Color.Green,
                    Font = new Font("Arial", 12),
                    Location = new Point(lb.Location.X, y + 20),
                    Text = $"Ihr Kontostand: {benutzerverwaltung.ReturnActiveUser(activeUser).kontoStand}"
                };
                homePanel.Controls.Add(lb);
                homePanel.Controls.Add(kontostand);

                Button button = new Button
                {
                    AutoSize = true,
                    Size = new Size(100, 20),
                    Font = new Font("Arial", 12),
                    Location = new Point(lb.Location.X + 160, y),
                    Text = $"Bearbeiten"
                };
                homePanel.Controls.Add(button);
            };
            flowLayoutPanel.Controls.Add(profileBtn);

            Button depotBtn = new Button
            {
                Text = "Depot",
                Size = new Size(80, 40),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Sans-Serif", 10)
            };
            depotBtn.Click += (s, e) =>
            {
                homePanel.Controls.Clear();

                Label dplabel = new Label()
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(15, 10),
                    Text = "Depot"
                };
                homePanel.Controls.Add(dplabel);

                Button geldBtn = new Button()
                {
                    AutoSize = true,
                    Size = new Size(100, 20),
                    Font = new Font("Arial", 12),
                    Location = new Point(dplabel.Location.X + 160, 10),
                    Text = $"Geld Hinzufuegen(Test)"
                };
                geldBtn.Click += (f, g) =>
                {
                    benutzerverwaltung.ReturnActiveUser(activeUser).GeldHinzufuegen(100);
                };
                homePanel.Controls.Add(geldBtn);
            };
            flowLayoutPanel.Controls.Add(depotBtn);

            homePanel = new Panel
            {
                Size = new Size(this.Size.Width - flowLayoutPanel.Width, this.Size.Height),
                Location = new Point(flowLayoutPanel.Right, 0),
                BackColor = Color.LightCyan,
                Visible = false
            };
            kaufPanel = new Panel
            {
                Size = new Size(300, 200),
                Location = new Point((homePanel.Width - 300) / 2, (homePanel.Height - 300) / 2),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            this.Controls.Add(kaufPanel);
            Controls.Add(homePanel);
            Controls.Add(flowLayoutPanel);
            this.Resize += OnResize;
        }
        public void OnResize(object sender, EventArgs e)
        {
            homePanel.Width = this.ClientSize.Width - flowLayoutPanel.Width;
            homePanel.Height = this.ClientSize.Height;
            if (registerPanel.Visible)
                InitRegisterUI();
            else if (loginPanel.Visible)
                InitLoginUi();
        }
        public void ShowHomePanel()
        {
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };
            homePanel.Controls.Add(flp);
            foreach(var stock in stonks)
            {
                Panel chartPanel = new Panel
                {
                    Size = new Size(300, 250),
                    BorderStyle = BorderStyle.FixedSingle
                };
                stock.plot.Plot.Title(stock.firma, 15);
                stock.plot.Plot.HideGrid();
                Label priceLb = new Label
                {
                    Text = $"Current Price: {stock.CurrentValue}€",
                    Dock = DockStyle.Bottom,
                    Height = 20
                };
                chartPanel.Controls.Add(priceLb);
                stock.plot.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        ShowKaufPanel(stock);
                    }
                };
                chartPanel.Controls.Add(stock.plot);
                flp.Controls.Add(chartPanel);
            }
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
            string passHash = benutzerverwaltung.Hash(password);
            MessageBox.Show(passHash);
            benutzerverwaltung.BenutzerAnlegen(email, vName, nName, passHash, BID, loginID, activeUser);
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
            benutzerverwaltung.BenutzerEinloggen(email, password, loginEmailInput.Text, loginPasswordInput.Text, activeUser, loginPanel, flowLayoutPanel, homePanel);
        }
        //Credits: https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
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
        public void ShowKaufPanel(Aktie aktie)
        {
            kaufPanel.Controls.Clear();
            kaufPanel.Visible = true;
            kaufPanel.BringToFront();
            Label nameLb = new Label
            {
                Text = $"Name: {aktie.firma}",
                Location = new Point(10, 10),
                AutoSize = true
            };
            kaufPanel.Controls.Add(nameLb);
            Label firmaLb = new Label
            {
                Text = $"Firma: {aktie.name}",
                Location = new Point(10, 40),
                AutoSize = true
            };
            kaufPanel.Controls.Add(firmaLb);
            Label preisLb = new Label
            {
                Text = $"Aktueller Preis {aktie.CurrentValue:f2}€",
                Location = new Point(10, 70),
                AutoSize = true
            };
            kaufPanel.Controls.Add(preisLb);
            Label anteilLb = new Label
            {
                Text = "Anzahl kaufen:",
                Location = new Point(10, 100),
                AutoSize = true
            };
            kaufPanel.Controls.Add(anteilLb);
            NumericUpDown anteilNum = new NumericUpDown
            {
                Location = new Point(110, 100),
                Minimum = 1,
                Maximum = 10000,
                Value = 1,
                Width = 80
            };
            kaufPanel.Controls.Add(anteilNum);
            Button kaufBtn = new Button
            {
                Text = "Kaufen",
                Location = new Point(10, 140),
                Width = 180,
                Height = 30
            };
            kaufBtn.Click += (s, e) =>
            {
                MessageBox.Show($"Kaufe {anteilNum.Value} Anteile der Aktie {aktie.firma}. Insgesamt Preis: {Convert.ToDecimal(aktie.CurrentValue) * anteilNum.Value:f2}€");
                kaufPanel.Visible = false;
            };
            kaufPanel.Controls.Add(kaufBtn);
        }
        public void addAktienGesellschaft(string Firma, string Name, string Wert) // Fügt die beliebiege Aktie zur Datenbank hinzu
        {
            string connString = "server=localhost;database=aktiensimdb;uid=root;password=\"\"";
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            string chckqry = "SELECT Firma FROM aktiendaten WHERE Firma = @Firma";
            string qry = "INSERT INTO aktiendaten(Firma, Name, Wert) VALUES(@Firma, @Name, @Wert)";

            string chkFirma; string chkName; string chkWert;

            using (MySqlCommand cmdCheck = new MySqlCommand(chckqry, conn)) 
            {
                cmdCheck.Parameters.AddWithValue("Firma", Firma);
                //cmdCheck.ExecuteNonQuery();
                MySqlDataReader reader = cmdCheck.ExecuteReader();

                if(reader.Read()) 
                {
                    chkFirma = reader["Firma"].ToString();

                    if (chkFirma == Firma)
                    {
                        return;
                    }
                }
            }
            conn.Close();
            conn.Open();
            using(MySqlCommand cmd = new MySqlCommand(qry, conn)) 
            {
                cmd.Parameters.AddWithValue("Firma", Firma);
                cmd.Parameters.AddWithValue("Name", Name);
                cmd.Parameters.AddWithValue("Wert", Wert);
                cmd.ExecuteNonQuery();
            }
        }
    }
}