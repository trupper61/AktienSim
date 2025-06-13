using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math.EC.Endo;
using Org.BouncyCastle.Tls;
using ScottPlot.Hatches;
using ScottPlot.Rendering.RenderActions;
using ScottPlot.WinForms;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        List<Aktie> stonks;
        public Panel kaufPanel;
        public Panel kreditPanel;
        public Button depotBtn;
        private Dictionary<int, Aktie> alleAktien = new Dictionary<int, Aktie>();
        public bool activeUserLoginFlag = false;
        public Form1()
        {
            InitializeComponent();
            InitLoginUi();
            InitRegisterUI();
            InitUI();
            stonks = MySqlManager.AktienVerwaltung.LadeAlleAktien(); // Loads all stonks in Database
        }
        public void InitStocks()
        {
            for (int i = 0; i < 20; i++)
            {
                SimuliereNächstenTag();
            }
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
                kreditPanel.Visible = false;
                Button nextDayBtn = new Button
                {
                    Text = "Nächster Tag...",
                    Location = new Point(10, 10),
                    Width = 120
                };
                homePanel.Controls.Add(nextDayBtn);
                nextDayBtn.Click += (s2, e2) =>
                {
                    SimuliereNächstenTag();
                };
                Button nextWeekBtn = new Button
                {
                    Text = "Nächste Woche...",
                    Location = new Point(10, 50),
                    Width = 120
                };
                homePanel.Controls.Add(nextWeekBtn);
                nextWeekBtn.Click += (s2, e2) =>
                {
                    for (int i = 0; i < 7; i++)
                    {
                        SimuliereNächstenTag();
                        if(i == 6) 
                        {
                            if (MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).kredite != null)
                            {
                                foreach (Kredite kr in MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).kredite)
                                {
                                    kr.Laufzeit--;
                                    MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).GeldAbziehen(kr.zuZahlendeRate);
                                }
                            }
                        }
                    }
                    
                };
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
                kreditPanel.Visible = false;
                int y = 10;
                homePanel.Controls.Clear();

                PictureBox profilbild = new PictureBox()
                {
                    Size = new Size(80, 80),
                    Location = new Point(195, 50),
                    Image = Properties.Resources.profile,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                homePanel.Controls.Add(profilbild);

                Label lb = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(profilbild.Location.X - 30, profilbild.Location.Y + 90),
                    Text = $"Hallo, {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).vorname} {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).name}"
                };
                homePanel.Controls.Add(lb);

                PictureBox kontostandBild = new PictureBox()
                {
                    Size = new Size(400, 56),
                    Location = new Point(lb.Location.X - 120, y + 200),
                    Image = Properties.Resources.kontostand2,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                homePanel.Controls.Add(kontostandBild);
                kontostandBild.MouseEnter += MouseEnterEffectKonto;
                kontostandBild.MouseLeave += MouseEnterEffectKontoLeave;
                kontostandBild.MouseClick += (p, l) => 
                {
                    homePanel.Controls.Clear();
                    Label kontostand = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Green,
                        BackColor = Color.Transparent,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y),
                        Text = $"Ihr Kontostand: {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).kontoStand}",
                    };
                    homePanel.Controls.Add(kontostand);
                    kontostand.BringToFront();

                    Label schulden = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Red,
                        Font = new Font("Arial", 12),
                        Location = new Point(kontostand.Location.X, y + 30),
                        Text = $"Ihre Schulden: TBD"
                    };
                    homePanel.Controls.Add(schulden);

                    Button kreditverwaltung = new Button
                    {
                        AutoSize = true,
                        Size = new Size(100, 20),
                        Font = new Font("Arial", 12),
                        Location = new Point(schulden.Location.X, schulden.Location.Y + 60),
                        Text = $"Kredite Verwalten"
                    };
                    homePanel.Controls.Add(kreditverwaltung);

                    Button umsaetze = new Button
                    {
                        AutoSize = true,
                        Size = new Size(100, 20),
                        Font = new Font("Arial", 12),
                        Location = new Point(schulden.Location.X, schulden.Location.Y + 90),
                        Text = $"Umsätze"
                    };
                    homePanel.Controls.Add(umsaetze);

                    kreditverwaltung.Click += (o, i) =>
                    {
                        homePanel.Controls.Clear();

                        Label kreditverwaltunglb = new Label()
                        {
                            AutoSize = true,
                            ForeColor = Color.Black,
                            Font = new Font("Arial", 20),
                            Location = new Point(0, 0),
                            Text = $"Kreditverwaltung"
                        };
                        homePanel.Controls.Add(kreditverwaltunglb);

                        DataGridView aktiveKredite = new DataGridView
                        {
                            Name = "dgvKredite",
                            Size = new Size(500, 250),
                            Location = new Point(0, 100),
                            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                            ReadOnly = true,
                            AllowUserToAddRows = false,
                            AllowUserToDeleteRows = false,
                            SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        };
                        aktiveKredite.Columns.Add("Betrag", "Kreditbetrag");
                        aktiveKredite.Columns.Add("Restschuld", "Restschuld");
                        aktiveKredite.Columns.Add("Zinssatz", "Zinssatz");
                        aktiveKredite.Columns.Add("Startdatum", "Startdatum");
                        aktiveKredite.Columns.Add("Laufzeit", "Laufzeit");
                        homePanel.Controls.Add(aktiveKredite);

                        Button kreditaufnahme = new Button
                        {
                            AutoSize = true,
                            Size = new Size(100, 20),
                            Font = new Font("Arial", 12),
                            Location = new Point(kreditverwaltunglb.Location.X, kreditverwaltunglb.Location.Y + 60),
                            Text = $"Kredit Aufnehmen"
                        };
                        kreditaufnahme.Click += (u, t) =>
                        {
                            ShowKreditPanel(aktiveKredite);
                        };
                        homePanel.Controls.Add(kreditaufnahme);
                        Kredite.RefreshDataGridView(aktiveKredite, MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser));
                    };
                };

                PictureBox benutzerdatenBild = new PictureBox()
                {
                    Size = new Size(400, 56),
                    Location = new Point(lb.Location.X - 120, y + 270),
                    Image = Properties.Resources.benutzerdaten,
                    SizeMode = PictureBoxSizeMode.StretchImage
                    
                };
                homePanel.Controls.Add(benutzerdatenBild);
                benutzerdatenBild.MouseHover += MouseEnterEffectDaten;
                benutzerdatenBild.MouseLeave += MouseEnterEffectDatenLeave;
                benutzerdatenBild.MouseClick += (p, l) =>
                {
                    homePanel.Controls.Clear();

                    Label vorname = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Transparent,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y),
                        Text = $"Vorname: {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).vorname}",
                    };
                    homePanel.Controls.Add(vorname);
                    Label nachname = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Transparent,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y + 20),
                        Text = $"Name: {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).name}",
                    };
                    homePanel.Controls.Add (nachname);
                    Label email = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Transparent,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y + 40),
                        Text = $"Email: {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).email}",
                    };
                    homePanel.Controls.Add(email);
                    Label benutzerID = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Transparent,
                        Font = new Font("Arial", 8),
                        Location = new Point(0, y + 330),
                        Text = $"BenutzerID: {MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).benutzerID}",
                    };
                    homePanel.Controls.Add(benutzerID);

                    Button button = new Button
                    {
                        AutoSize = true,
                        Size = new Size(100, 20),
                        Font = new Font("Arial", 12),
                        Location = new Point(lb.Location.X + 160, y - 10),
                        Text = $"Bearbeiten"
                    };
                    homePanel.Controls.Add(button);
                    button.Click += (o, i) =>
                    {
                        button.Hide();
                        vorname.Hide();
                        nachname.Hide();
                        email.Hide();

                        TextBox vname = new TextBox
                        {
                            Text = $"{MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).vorname}",
                            Font = new Font("Arial", 12),
                            ForeColor = Color.Black,
                            Location = new Point(0, y),
                            Size = new Size(200, 22)
                        };
                        homePanel.Controls.Add(vname);

                        TextBox nname = new TextBox
                        {
                            Text = $"{MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).name}",
                            Font = new Font("Arial", 12),
                            ForeColor = Color.Black,
                            Location = new Point(0, y + 30),
                            Size = new Size(200, 22)
                        };
                        homePanel.Controls.Add(nname);

                        TextBox emailBox = new TextBox
                        {
                            Text = $"{MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).email}",
                            Font = new Font("Arial", 12),
                            ForeColor = Color.Black,
                            Location = new Point(0, y + 60),
                            Size = new Size(200, 22)
                        };
                        homePanel.Controls.Add(emailBox);

                        Button fertig = new Button
                        {
                            AutoSize = true,
                            Size = new Size(100, 20),
                            Font = new Font("Arial", 12),
                            Location = new Point(lb.Location.X + 160, y - 10),
                            Text = $"Fertig"
                        };
                        homePanel.Controls.Add(fertig);
                        
                        fertig.Click += (t, z) =>
                        {
                            MySqlManager.Benutzerverwaltung.UpdateBenutzerDaten(vname.Text, nname.Text, emailBox.Text, MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).benutzerID);
                            homePanel.Controls.Remove(fertig);
                            homePanel.Controls.Remove(vname);
                            homePanel.Controls.Remove(nname);
                            homePanel.Controls.Remove(emailBox);
                            button.Show();
                            vorname.Show();
                            nachname.Show();
                            email.Show();
                            MessageBox.Show("Loggen sie sich erneut ein, damit die Änderungen wirksam werden!");
                        };
                    };
                };

                PictureBox backroundImage = new PictureBox()
                {
                    Size = new Size(603, 132),
                    Location = new Point(0, -30),
                    Image = Properties.Resources.Profilebackround,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                homePanel.Controls.Add(backroundImage);
                backroundImage.SendToBack();

                Button buttonLogout = new Button
                {
                    Size = new Size(50, 50),
                    Location = new Point(lb.Location.X + 280, lb.Location.Y - 130),
                    BackgroundImage = Properties.Resources.Logout
                };
                homePanel.Controls.Add(buttonLogout);
                buttonLogout.Click += (l, p) =>
                {
                    InitLoginUi();
                    activeUser = null;
                    
                    loginPanel.Visible = true;
                    flowLayoutPanel.Visible = false;
                    homePanel.Visible = false;
                };
                buttonLogout.BringToFront();
            };
            flowLayoutPanel.Controls.Add(profileBtn);

            depotBtn = new Button
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
                kreditPanel.Visible = false;
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
                    Text = $"Geld Hinzufügen (Test)"
                };
                geldBtn.Click += (f, g) =>
                {
                    MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).GeldHinzufuegen(100);
                    MessageBox.Show("100€ hinzugefügt");
                };
                homePanel.Controls.Add(geldBtn);

                TextBox depotTb = new TextBox()
                {
                    Location = new Point(200, 60)
                };
                homePanel.Controls.Add(depotTb);

                ListBox depotListBox = new ListBox()
                {
                    Location = new Point(15, 100),
                    Size = new Size(200, homePanel.Height - 120)
                };
                homePanel.Controls.Add(depotListBox);
                Button createDepot = new Button
                {
                    Size = new Size(80, 25),
                    Location = new Point(depotTb.Right + 15, 60),
                    Text = "Depot erstellen"
                };
                createDepot.Click += (h, i) =>
                {
                    string name = depotTb.Text.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        int userId = Convert.ToInt32(MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).benutzerID);
                        MySqlManager.DepotVerwaltung.CreateDepot(name, userId);
                        MessageBox.Show($"Depot '{name}' erstellt");
                        LoadUserDepots();
                    }
                    else
                    {
                        MessageBox.Show("Bitte einen Depotnamen eingeben.");
                    }
                };
                homePanel.Controls.Add(createDepot);


                Panel aktienImDepotPanel = new Panel()
                {
                    Location = new Point(depotListBox.Right + 20, 100),
                    Size = new Size(homePanel.Width - depotListBox.Width - 50, homePanel.Height - 120),
                    AutoScroll = true,
                    BorderStyle = BorderStyle.FixedSingle
                };

                aktienImDepotPanel.Resize += (f, g) =>
                {
                    aktienImDepotPanel.Size = new Size(homePanel.Width - depotListBox.Width - 50, homePanel.Height - 120);
                };

                homePanel.Controls.Add(aktienImDepotPanel);

                void LoadUserDepots()
                {
                    depotListBox.Items.Clear();
                    int userId = Convert.ToInt32(MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).benutzerID);
                    var depots = MySqlManager.DepotVerwaltung.GetUserDepot(userId);
                    foreach (var depot in depots)
                        depotListBox.Items.Add(depot);
                }
                LoadUserDepots();

                // Event beim Wechsel des ausgewählten Depots
                depotListBox.SelectedIndexChanged += (sender, args) =>
                {
                    aktienImDepotPanel.Controls.Clear();
                    var selectedDepot = depotListBox.SelectedItem as Depot;

                    if (selectedDepot != null)
                    {
                        int yPos = 10;
                        var transaktionen = MySqlManager.TransaktionVerwaltung.LadeTransaktionenFürDepot(selectedDepot.ID);

                        foreach (var transaktion in transaktionen)
                        {
                            Aktie aktie = MySqlManager.AktienVerwaltung.LoadAktieByID(transaktion.aktieID);
                            double gesamtwert = transaktion.anzahl * aktie.CurrentValue;

                            double veraenderung = ((aktie.CurrentValue - (double)transaktion.einzelpreis) / (double)transaktion.einzelpreis) * 100;
                            string veraenderungStr = veraenderung >= 0 ? $"+{veraenderung:F2}%" : $"{veraenderung:F2}%";

                            Label aktienLabel = new Label()
                            {
                                Text = $"{aktie.name} ({aktie.firma}) - Anteile: {transaktion.anzahl} - Gesamtwert: {gesamtwert:F2}€ – Veränderung: {veraenderungStr}",
                                Location = new Point(10, yPos),
                                AutoSize = true,
                                Font = new Font("Arial", 10),
                                ForeColor = veraenderung >= 0 ? Color.Green : Color.Red,
                                Tag = transaktion
                            };
                            aktienLabel.Click += (s2, e2) =>
                            {
                                if (depotListBox.SelectedItems.Count == 0) return;
                                MessageBox.Show("Hello");
                                ShowVerkaufPanel(transaktion);
                            };
                            aktienImDepotPanel.Controls.Add(aktienLabel);
                            yPos += 30;
                        }
                    }
                };
            };
            flowLayoutPanel.Controls.Add(depotBtn);
            Button aktienBtn = new Button
            {
                Text = "Aktien",
                Size = new Size(80, 40),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Sans-Serif", 10)
            };
            aktienBtn.Click += (s2, e2) =>
            {
                homePanel.Controls.Clear();
                ShowHomePanel();
            };
            flowLayoutPanel.Controls.Add(aktienBtn);
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
            kreditPanel = new Panel
            {
                Size = new Size(300, 200),
                Location = new Point((homePanel.Width - 315) / 2, (homePanel.Height - 300) / 2),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            this.Controls.Add(kreditPanel);
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
            //foreach (var stock in stonks)
            //{
            //    string[] update = stonkManager.GetUpdateAktien(stock.id);
            //    stock.CurrentValue = Convert.ToDouble(update[0]);
            //    stock.SetLastClose(Convert.ToDouble(update[1]));
            //}

          
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
            string passHash = MySqlManager.Benutzerverwaltung.Hash(password);
            MessageBox.Show(passHash);
            MySqlManager.Benutzerverwaltung.BenutzerAnlegen(email, vName, nName, passHash, BID, loginID, activeUser);
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
            MySqlManager.Benutzerverwaltung.BenutzerEinloggen(email, password, loginEmailInput.Text, loginPasswordInput.Text, activeUser, loginPanel, flowLayoutPanel, homePanel);
            if(MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser) != null) 
            {
                Kredite.HoleKrediteAusDatenbank(MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser));
            }
            homePanel.Controls.Clear();
            Benutzer aNutzer = MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser);
            }
        //Credits: https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
 
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
                DialogResult result = MessageBox.Show($"Kaufe {anteilNum.Value} Anteile der Aktie {aktie.firma}. Insgesamt Preis: {Convert.ToDecimal(aktie.CurrentValue) * anteilNum.Value:f2}€");
                if (result == DialogResult.OK )
                {
                    MySqlManager.TransaktionVerwaltung.AddTransaktion(aktie.id, "Kauf", Convert.ToDouble(anteilNum.Value), Convert.ToDecimal(aktie.CurrentValue), MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser));
                }
                kaufPanel.Visible = false;
            };
            kaufPanel.Controls.Add(kaufBtn);
        }
        public void ShowKreditPanel(DataGridView aktiveKredite)
        {
            Kredite kredit = new Kredite(0, 0, 0, 0, MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser));
            kreditPanel.Controls.Clear();
            kreditPanel.Visible = true;
            kreditPanel.BringToFront();

            Label menge = new Label
            {
                Text = "Betrag: ",
                Location = new Point(10, 10),
                AutoSize = true
            };
            kreditPanel.Controls.Add(menge);

            NumericUpDown auswahlMenge = new NumericUpDown
            {
                Location = new Point(100, 10),
                Minimum = 100,
                Maximum = 10000,
                Value = 100,
                Width = 80
            };
            kreditPanel.Controls.Add(auswahlMenge);

            NumericUpDown auswahlLaufzeit = new NumericUpDown
            {
                Location = new Point(100, 40),
                Minimum = 4,
                Maximum = 48,
                Value = 4,
                Width = 80
            };
            kreditPanel.Controls.Add(auswahlLaufzeit);

            Label zuZahlendeRate = new Label
            {
                
                Text = $"Monatlich Fällig: {Math.Round((auswahlMenge.Value * (1 + (decimal)kredit.bestimmeZinssatz() / 100)) / auswahlLaufzeit.Value, 2)}€",
                Location = new Point(10, 130),
                AutoSize = true
            };
            kreditPanel.Controls.Add(zuZahlendeRate);

            Label schuldig = new Label
            {
                Text = $"Zu begleichender Betrag: {auswahlMenge.Value * (1 + (decimal)kredit.bestimmeZinssatz() / 100)}",
                Location = new Point(10, 100),
                AutoSize = true
            };
            kreditPanel.Controls.Add(schuldig);

            auswahlMenge.ValueChanged += (y, x) =>
            {
                schuldig.Text = $"Zu begleichender Betrag: {auswahlMenge.Value * (1 + (decimal)kredit.bestimmeZinssatz() / 100)}";
                zuZahlendeRate.Text = $"Monatlich Fällig: {Math.Round((auswahlMenge.Value * (1 + (decimal)kredit.bestimmeZinssatz() / 100)) / auswahlLaufzeit.Value, 2)}€";
            };

            auswahlLaufzeit.ValueChanged += (c, v) =>
            {
                zuZahlendeRate.Text = $"Monatlich Fällig: {Math.Round((auswahlMenge.Value * (1 + (decimal)kredit.bestimmeZinssatz() / 100)) / auswahlLaufzeit.Value, 2)}€";
            };
            
            Label laufzeit = new Label
            {
                Text = "Laufzeit(Monate): ",
                Location = new Point(10, 40),
                AutoSize = true
            };
            kreditPanel.Controls.Add (laufzeit);
            
            Label zinssatz = new Label
            {
                Text = $"Zinssatz: { kredit.bestimmeZinssatz().ToString() }%",
                Location = new Point(10, 70),
                AutoSize = true
            };
            kreditPanel.Controls.Add(zinssatz);

            Button kreditAufnehmen = new Button()
            {
                Text = $"Kredit Aufnehmen",
                Location = new Point(10, 160),
                Width = 180,
                Height = 30
            };
            kreditPanel.Controls.Add(kreditAufnehmen);
            kreditAufnehmen.Click += (q, w) =>
            {
                kredit.Betrag = (double)auswahlMenge.Value;
                kredit.Restschuld = (double)auswahlMenge.Value * (1 + (double)kredit.bestimmeZinssatz() / 100);
                kredit.Laufzeit = (int)auswahlLaufzeit.Value;
                kredit.KreditHinzufuegen(kredit.Betrag, kredit.Zinssatz, kredit.Restschuld, kredit.Laufzeit, MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser), aktiveKredite, kredit);

                kreditPanel.Visible = false;
            };
            
            
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
        public void ShowVerkaufPanel(Transaktion transaktion)
        {
            Aktie aktie = MySqlManager.AktienVerwaltung.LoadAktieByID(transaktion.aktieID);
            Panel verkaufPanel = new Panel
            {
                Size = new Size(300, 200),
                Location = new Point(550, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            Label info = new Label
            {
                Text = $"Verkauf: {aktie.firma}\n" +
                       $"Anteile: {transaktion.anzahl:F2}\n" +
                       $"Kaufpreis: {transaktion.einzelpreis:F2}€\n" +
                       $"Aktuell: {aktie.CurrentValue:F2}€",
                AutoSize = true,
                Location = new Point(10, 10)
            };
            verkaufPanel.Controls.Add(info);

            double diff = aktie.CurrentValue - Convert.ToDouble(transaktion.einzelpreis);
            double diffPercent = diff / (double)transaktion.einzelpreis * 100;

            Label diffLabel = new Label
            {
                Text = $"Veränderung: {diffPercent:F2}%",
                ForeColor = diff >= 0 ? Color.Green : Color.Red,
                AutoSize = true,
                Location = new Point(10, 60)
            };
            verkaufPanel.Controls.Add(diffLabel);

            Label mengeLabel = new Label
            {
                Text = "Menge:",
                Location = new Point(10, 90),
                AutoSize = true
            };
            verkaufPanel.Controls.Add(mengeLabel);

            TextBox mengeTb = new TextBox
            {
                Location = new Point(70, 90),
                Width = 50
            };
            verkaufPanel.Controls.Add(mengeTb);

            Button verkaufenBtn = new Button
            {
                Text = "Verkaufen",
                Location = new Point(10, 130),
                Width = 100
            };
            verkaufenBtn.Click += (s, e) =>
            {
                if (double.TryParse(mengeTb.Text, out double menge) && menge > 0 && menge <= transaktion.anzahl)
                {
                    double erloes = menge * aktie.CurrentValue;

                    // Geld gutschreiben
                    MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser).GeldHinzufuegen(Convert.ToInt32(erloes));

                    transaktion.anzahl -= menge;
                    MySqlManager.TransaktionVerwaltung.AktualisiereTransaktion(transaktion);

                    MessageBox.Show($"Du hast {menge} Anteile für {erloes:F2}€ verkauft.");
                    homePanel.Controls.Remove(verkaufPanel);
                    depotBtn.PerformClick(); 
                }
                else
                {
                    MessageBox.Show("Ungültige Menge.");
                }
            };
            verkaufPanel.Controls.Add(verkaufenBtn);

            homePanel.Controls.Add(verkaufPanel);
            verkaufPanel.BringToFront();
        }

        public void SimuliereNächstenTag()
        {
            var alleBenutzer = MySqlManager.Benutzerverwaltung.LadeAlleBenutzer();
            var aktuellerBenutzer = MySqlManager.Benutzerverwaltung.ReturnActiveUser(activeUser);

            Dictionary<int, int> nachfrage = new Dictionary<int, int>();
            Random rand = new Random();
            foreach(var benutzer in alleBenutzer)
            {
                if (benutzer.benutzerID == aktuellerBenutzer.benutzerID)
                    continue;
                var depots = MySqlManager.DepotVerwaltung.GetUserDepot(Convert.ToInt32(benutzer.benutzerID));
                if (depots.Count == 0)
                {
                    MySqlManager.DepotVerwaltung.CreateDepot("Standarddepot", Convert.ToInt32(benutzer.benutzerID));
                    depots = MySqlManager.DepotVerwaltung.GetUserDepot(Convert.ToInt32(benutzer.benutzerID));
                }
                foreach(var depot in depots)
                {
                    foreach (var aktie in stonks)
                    {
                        int choice = rand.Next(0, 3); // 0 = nichts, 1 = kaufen, 2 = verkaufen
                        switch (choice)
                        {
                            case 1:
                                double mengeKauf = Math.Round(rand.NextDouble() * 5, 2);
                                decimal kosten = Convert.ToDecimal(mengeKauf * aktie.CurrentValue);
                                if(benutzer.kontoStand >= (double)kosten)
                                {
                                    MySqlManager.TransaktionVerwaltung.AddTransaktion(aktie.id, "Kauf", mengeKauf, Convert.ToDecimal(aktie.CurrentValue), benutzer);
                                    benutzer.UpdateKontoStand(Convert.ToInt32(kosten), benutzer.benutzerID);
                                    nachfrage.TryGetValue(aktie.id, out int wert);
                                    nachfrage[aktie.id] = wert + 1; 
                                }
                                break;
                            case 2:
                                var transaktion = MySqlManager.TransaktionVerwaltung.LadeTransaktionenFürDepot(depot.ID).Where(t => t.aktieID == aktie.id && t.anzahl > 0).ToList();
                                if (transaktion.Any())
                                {
                                    var trans = transaktion[rand.Next(transaktion.Count)];
                                    double verkaufsMenge = Math.Min(trans.anzahl, Math.Round(rand.NextDouble() * 5, 2));
                                    if (verkaufsMenge > 0)
                                    {
                                        decimal erloes = Convert.ToDecimal(verkaufsMenge * aktie.CurrentValue);
                                        trans.anzahl -= verkaufsMenge;
                                        if (trans.anzahl <= 0)
                                        {
                                            MySqlManager.TransaktionVerwaltung.LöscheTransaktion(trans.id);
                                        }
                                        else
                                        {
                                            MySqlManager.TransaktionVerwaltung.AktualisiereTransaktion(trans);
                                        }
                                        benutzer.GeldHinzufuegen((int)erloes);
                                        nachfrage.TryGetValue(aktie.id, out int wert);
                                        nachfrage[aktie.id] = wert - 1;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            foreach(var aktie in stonks)
            {
                nachfrage.TryGetValue(aktie.id, out int delta);
                double prozent = Math.Min(5, Math.Abs(delta) * 0.01);
                if (delta > 0)
                {
                    aktie.CurrentValue *= (double)(1 + prozent);
                    aktie.SimulateNextStep(aktie.CurrentValue);
                }
                else if (delta < 0)
                {
                    aktie.CurrentValue *= (double)(1 - prozent);
                    aktie.SimulateNextStep(aktie.CurrentValue);
                }
               else
                {
                    aktie.SimulateNextStep();
                }



                    MySqlManager.AktienVerwaltung.UpdateAktie(aktie);

            }
        }
        public void MouseEnterEffectDaten(object sender, EventArgs e) 
        {
            PictureBox pb = sender as PictureBox;

            pb.Image = Properties.Resources.benutzerdatenGlow;
        }
        public void MouseEnterEffectDatenLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            pb.Image = Properties.Resources.benutzerdaten;
        }
        public void MouseEnterEffectKonto(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            pb.Image = Properties.Resources.kontostand2Glow;
        }
        public void MouseEnterEffectKontoLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            pb.Image = Properties.Resources.kontostand2;
        }
    }
}