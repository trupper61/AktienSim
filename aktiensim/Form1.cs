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
            List<Aktie> neueAktien = new List<Aktie>
{
    new Aktie("SAP", "SAP SE", 125.45, 1),
    new Aktie("BMW", "Bayerische Motoren Werke AG", 98.30, 2),
    new Aktie("Volkswagen", "Volkswagen AG", 118.70, 4),
    new Aktie("DeutscheBank", "Deutsche Bank AG", 118.70, 5),
    new Aktie ("Siemens", "Siemens AG", 143.10, 6),
    new Aktie("DeutschePost", "Deutsche Post AG", 42.60, 9),
    new Aktie("Merck", "Merck KGaA", 158.40, 10),
    new Aktie("Microsoft", "Microsoft Corporation", 328.10, 12),
};
            using (var myMan = new MySqlManager())
            {
                //foreach (Aktie a in neueAktien)
                //{
                //    myMan.Aktien.AktieAnlegen(a.firma, a.name, a.CurrentValue);
                //}
                stonks = myMan.Aktien.LadeAlleAktien();
            }
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
                homePanel.BackgroundImage = Properties.Resources.backround;
                homePanel.Controls.Clear();
                kreditPanel.Visible = false;

                PictureBox Logo = new PictureBox
                {
                    BackgroundImage = Properties.Resources.logo,
                    Size = new Size(364, 128),
                    Location = new Point(homePanel.Location.X / 2, homePanel.Location.Y / 2),
                    BackColor = Color.Transparent,
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                homePanel.Controls.Add(Logo);

                Button buttonLogout = new Button
                {
                    Size = new Size(50, 50),
                    Location = new Point(440, 0),
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
                homePanel.BackgroundImage = null;
                PictureBox profilbild = new PictureBox()
                {
                    Size = new Size(80, 80),
                    Location = new Point(195, 50),
                    Image = Properties.Resources.profile,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                homePanel.Controls.Add(profilbild);
                LoadActiveUser();
                Console.WriteLine($"{activeUser.ToString()}");
                Label lb = new Label
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(profilbild.Location.X - 30, profilbild.Location.Y + 90),
                    Text = $"Hallo, {activeUser.vorname} {activeUser.name}"
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
                    homePanel.BackgroundImage = Properties.Resources.backround;
                    homePanel.Controls.Clear();
                    LoadActiveUser();
                    Label kontostand = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Green,
                        BackColor = Color.Transparent,
                        Image = Properties.Resources.kontostand,
                        Font = new Font("Arial", 16, FontStyle.Bold),
                        Location = new Point(0, y),
                        Text = $"Ihr Kontostand: {activeUser.kontoStand}",
                    };
                    homePanel.Controls.Add(kontostand);
                    kontostand.BringToFront();

                    Button kreditverwaltung = new Button
                    {
                        AutoSize = true,
                        Size = new Size(100, 20),
                        Font = new Font("Arial", 12),
                        Location = new Point(kontostand.Location.X, kontostand.Location.Y + 50),
                        Text = $"Kredite Verwalten"
                    };
                    homePanel.Controls.Add(kreditverwaltung);

                    Button umsaetze = new Button
                    {
                        AutoSize = true,
                        Size = new Size(100, 20),
                        Font = new Font("Arial", 12),
                        Location = new Point(kontostand.Location.X, kontostand.Location.Y + 80),
                        Text = $"Umsätze"
                    };
                    umsaetze.Click += (s2, e2) =>
                    {
                        homePanel.Controls.Clear();
                        Label titel = new Label
                        {
                            Text = "Alle Überweisungen",
                            Font = new Font("Arial", 14, FontStyle.Bold),
                            Location = new Point(10, 10),
                            Size = new Size(400, 30)
                        };
                        homePanel.Controls.Add(titel);

                        ListBox listBox = new ListBox
                        {
                            Location = new Point(10, 50),
                            Size = new Size(500, 300),
                            Font = new Font("Arial", 10)
                        };
                        LoadActiveUser();
                        using (var myMan = new MySqlManager())
                        { 
                            var ueberweisungen = myMan.Transaktion.LadeUeberweisungenFürBenutzer(Convert.ToInt32(activeUser.benutzerID));

                            foreach (var ue in ueberweisungen)
                            {
                                string typ = ue.AbsenderID == Convert.ToInt32(activeUser.benutzerID) ? "Gesendet an" : "Empfangen von";
                                int andereID = ue.AbsenderID == Convert.ToInt32(activeUser.benutzerID) ? ue.EmpfaengerID : ue.AbsenderID;
                                var anderePerson = myMan.Benutzer.GetBenutzerById(andereID);
                                string name = $"{anderePerson.vorname} {anderePerson.name}";
                                string zeile = $"{typ} {name}: {ue.Betrag:F2}€ am {ue.DateTime:g}";

                                listBox.Items.Add(zeile);
                            }
                        }
                        if (listBox.Items.Count == 0)
                            listBox.Items.Add("Keine Überweisungen gefunden.");
                        
                        homePanel.Controls.Add(listBox);

                    };
                    homePanel.Controls.Add(umsaetze);

                    kreditverwaltung.Click += (o, i) =>
                    {
                        homePanel.BackgroundImage = Properties.Resources.backround;
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
                        LoadActiveUser();
                        Kredite.HoleKrediteAusDatenbank(activeUser);
                        Kredite.RefreshDataGridView(aktiveKredite, activeUser);
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
                    homePanel.BackgroundImage = Properties.Resources.backround;
                    LoadActiveUser();
                    Label vorname = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Wheat,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y),
                        Text = $"Vorname: {activeUser.vorname}",
                    };
                    homePanel.Controls.Add(vorname);
                    Label nachname = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Wheat,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y + 20),
                        Text = $"Name: {activeUser.name}",
                    };
                    homePanel.Controls.Add (nachname);
                    Label email = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Wheat,
                        Font = new Font("Arial", 12),
                        Location = new Point(0, y + 40),
                        Text = $"Email: {activeUser.email}",
                    };
                    homePanel.Controls.Add(email);
                    Label benutzerID = new Label
                    {
                        AutoSize = true,
                        ForeColor = Color.Black,
                        BackColor = Color.Wheat,
                        Font = new Font("Arial", 8),
                        Location = new Point(0, y + 330),
                        Text = $"BenutzerID: {activeUser.benutzerID}",
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
                        LoadActiveUser();
                        TextBox vname = new TextBox
                        {
                            Text = $"{activeUser.vorname}",
                            Font = new Font("Arial", 12),
                            ForeColor = Color.Black,
                            Location = new Point(0, y),
                            Size = new Size(200, 22)
                        };
                        homePanel.Controls.Add(vname);

                        TextBox nname = new TextBox
                        {
                            Text = $"{activeUser.name}",
                            Font = new Font("Arial", 12),
                            ForeColor = Color.Black,
                            Location = new Point(0, y + 30),
                            Size = new Size(200, 22)
                        };
                        homePanel.Controls.Add(nname);

                        TextBox emailBox = new TextBox
                        {
                            Text = $"{activeUser.email}",
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
                            LoadActiveUser();
                            using (var myMan = new MySqlManager())
                            {
                                myMan.Benutzer.UpdateBenutzerDaten(vname.Text, nname.Text, emailBox.Text, activeUser.benutzerID);
                                homePanel.Controls.Remove(fertig);
                                homePanel.Controls.Remove(vname);
                                homePanel.Controls.Remove(nname);
                                homePanel.Controls.Remove(emailBox);
                                button.Show();
                                vorname.Show();
                                nachname.Show();
                                email.Show();
                                MessageBox.Show("Loggen sie sich erneut ein, damit die Änderungen wirksam werden!");
                            }
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
                homePanel.BackgroundImage = null;
                kreditPanel.Visible = false;
                Label dplabel = new Label()
                {
                    AutoSize = true,
                    Font = new Font("Arial", 12),
                    Location = new Point(15, 10),
                    Text = "Depot"
                };
                homePanel.Controls.Add(dplabel);

                TextBox depotTb = new TextBox()
                {
                    Location = new Point(15, 60)
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
                    Text = "Depot erstellen",
                    AutoSize = true
                };
                createDepot.Click += (h, i) =>
                {
                    string name = depotTb.Text.Trim();
                    LoadActiveUser();
                    if (!string.IsNullOrEmpty(name))
                    {
                        int userId = Convert.ToInt32(activeUser.benutzerID);
                        using (var myMan = new MySqlManager())
                        {
                            myMan.Depot.CreateDepot(name, userId);
                            MessageBox.Show($"Depot '{name}' erstellt");
                        }
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

                homePanel.Resize += (f, g) =>
                {
                    if (aktienImDepotPanel != null)
                    {
                        depotListBox.Size = new Size(homePanel.Width - depotListBox.Width, homePanel.Height - 120);
                        aktienImDepotPanel.Size = new Size(homePanel.Width - depotListBox.Width - 50, homePanel.Height - 120);
                    }
                };

                homePanel.Controls.Add(aktienImDepotPanel);

                void LoadUserDepots()
                {
                    depotListBox.Items.Clear();
                    using (var myMan = new MySqlManager())
                    {
                        LoadActiveUser();
                        int userId = Convert.ToInt32(activeUser.benutzerID);

                        var depots = myMan.Depot.GetUserDepot(userId);
                        foreach (var depot in depots)
                            depotListBox.Items.Add(depot);
                    }
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
                        using (var myMan = new MySqlManager())
                        {
                            var transaktionen = myMan.Transaktion.LadeTransaktionenFürDepot(selectedDepot.ID);
                            foreach (var transaktion in transaktionen)
                            {
                                Aktie aktie = myMan.Aktien.LoadAktieByID(transaktion.aktieID);

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
                                    ShowVerkaufPanel(transaktion);
                                };
                                aktienImDepotPanel.Controls.Add(aktienLabel);
                                yPos += 30;
                            }
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
            Button ueberweisungBtn = new Button
            {
                Text = "Überweisung",
                Size = new Size(80, 40),
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Sans-Serif", 10)
            };
            ueberweisungBtn.Click += (s2, e2) =>
            {
                homePanel.Controls.Clear();
                ShowUeberweisungPanel();
            };
            flowLayoutPanel.Controls.Add(ueberweisungBtn);

            Button nextDayBtn = new Button
            {
                //Text = "Nächster Tag...",
                //Location = new Point(10, 10),
                Width = 45,
                Height = 45,
                BackgroundImage = Properties.Resources.nxtButton1,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            flowLayoutPanel.Controls.Add(nextDayBtn);
            nextDayBtn.Click += (s2, e2) =>
            {
                SimuliereNächstenTag();
            };
            Button nextWeekBtn = new Button
            {
                Width = 45,
                Height = 45,
                BackgroundImage = Properties.Resources.nxtButton2,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            flowLayoutPanel.Controls.Add(nextWeekBtn);

            nextWeekBtn.Click += (s2, e2) =>
            {
                for (int i = 0; i < 7; i++)
                {
                    SimuliereNächstenTag();
                    if (i == 6)
                    {
                        using (var myMan = new MySqlManager())
                        {
                            foreach (Benutzer benutzer in myMan.Benutzer.LadeAlleBenutzer())
                            {
                                List<Kredite> geloeschteKredite = new List<Kredite>();

                                foreach (Kredite kr in benutzer.kredite)
                                {
                                    kr.Laufzeit--;
                                    kr.Restschuld -= kr.zuZahlendeRate;
                                    benutzer.GeldAbziehen(kr.zuZahlendeRate);
                                    kr.UpdateKreditStatus(kr);
                                    if (kr.Laufzeit > 0)
                                    {
                                        geloeschteKredite.Add(kr);
                                    }
                                    else
                                    {
                                        kr.KreditLoeschen();
                                    }
                                    
                                }
                                if (benutzer.kredite.Count == 0)
                                {
                                    benutzer.GeldAbziehen(0);
                                }
                                benutzer.kredite = geloeschteKredite;
                            }

                        }
                    }
                }

            };

            homePanel = new Panel
            {
                Size = new Size(this.Size.Width - flowLayoutPanel.Width, this.Size.Height),
                Location = new Point(flowLayoutPanel.Right, 0),
                BackColor = Color.LightCyan,
                Visible = false,
                BackgroundImage = Properties.Resources.backround,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            PictureBox registerLogo = new PictureBox
            {
                BackgroundImage = Properties.Resources.logo,
                Size = new Size(182, 64),
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            homePanel.Controls.Add(registerLogo);
            kaufPanel = new Panel
            {
                Size = new Size(350, 300),
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
            if (kaufPanel != null && homePanel != null)
            {
                kaufPanel.Location = new Point((homePanel.Width - kaufPanel.Width) / 2, (homePanel.Height - kaufPanel.Height) / 2);
            }
        }
        public void ShowUeberweisungPanel()
        {
            Label empfaengerLb = new Label { Text = "Empfänger (Name, Vorname oder E-Mail):", Dock = DockStyle.Top };
            homePanel.Controls.Add(empfaengerLb);
            TextBox empfaengerTxt = new TextBox { Dock = DockStyle.Top };
            homePanel.Controls.Add(empfaengerTxt);
            ListBox vorschlaegeLst = new ListBox
            {
                Height = 100,
                Dock = DockStyle.Top,
                Visible = false
            };
            homePanel.Controls.Add(vorschlaegeLst);
            empfaengerTxt.TextChanged += (s, e) =>
            {
                string eingabe = empfaengerTxt.Text.Trim();
                if (eingabe.Length < 2)
                {
                    vorschlaegeLst.Visible = false;
                    return;
                }
                using (var myMan = new MySqlManager())
                {
                    var treffer = myMan.Benutzer.LadeAlleBenutzer().Where(b => b.email.ToLower().Contains(eingabe));
                    if (treffer.Count() == 0)
                    {
                        vorschlaegeLst.Visible = false;
                        return;
                    }
                    vorschlaegeLst.Items.Clear();
                    foreach (var b in treffer)
                    {
                        vorschlaegeLst.Items.Add($"{b.name}, {b.vorname}: {b.email}");
                    }
                    vorschlaegeLst.Visible = true;
                }
            };
            vorschlaegeLst.SelectedIndexChanged += (s, e) =>
            {
                if (vorschlaegeLst.SelectedItem != null)
                {
                    string selectedText = vorschlaegeLst.SelectedItem.ToString();
                    var parts = selectedText.Split(new string[] { ": "}, StringSplitOptions.None);
                    if(parts.Length == 2)
                    {
                        empfaengerTxt.Text = parts[1];
                    }
                    else
                    {
                        empfaengerTxt.Text = selectedText;
                    }
                    vorschlaegeLst.Visible = false;
                }
            };

            Label betragLb = new Label { Text = "Betrag (€):", Dock = DockStyle.Top };
            homePanel.Controls.Add(betragLb);
            TextBox betragTxt = new TextBox { Dock = DockStyle.Top };
            homePanel.Controls.Add(betragTxt);
            Button sendenBtn = new Button { Text = "Überweisen", Dock = DockStyle.Top };
            homePanel.Controls.Add(sendenBtn);
            Label statusLb = new Label { Text = "", ForeColor = Color.Red, Dock = DockStyle.Top };
            homePanel.Controls.Add(statusLb);

            sendenBtn.Click += (s, e) =>
            {
                string empfaengerInput = empfaengerTxt.Text.Trim();
                if (!double.TryParse(betragTxt.Text.Trim(), out double betrag) || betrag <= 0)
                {
                    statusLb.Text = "Ungültiger Betrag";
                    return;
                }
                using (var myMan = new MySqlManager())
                {
                    Benutzer empfaenger = myMan.Benutzer.GetUserByInput(empfaengerInput);
                    if (empfaenger == null)
                    {
                        statusLb.Text = "Benutzer nicht gefunden.";
                        return;
                    }
                    LoadActiveUser();
                    if (activeUser.kontoStand < betrag)
                    {
                        statusLb.Text = "Nicht genügend Guthaben.";
                        return;
                    }
                    myMan.Transaktion.AddÜberweisung(activeUser, empfaenger, betrag);
                    activeUser.GeldAbziehen(betrag);
                    empfaenger.GeldHinzufuegen(betrag);

                    myMan.Benutzer.UpdateBenutzerDaten(activeUser.vorname, activeUser.name, activeUser.email, activeUser.benutzerID);
                    myMan.Benutzer.UpdateBenutzerDaten(empfaenger.vorname, empfaenger.name, empfaenger.email, empfaenger.benutzerID);
                    statusLb.ForeColor = Color.Green;
                    statusLb.Text = $"Überweisung erfolgreich an {empfaenger.name}, {empfaenger.vorname}";
                }
            };
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
                BackColor = Color.LightBlue,
                BackgroundImage = Properties.Resources.backround,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            PictureBox registerLogo = new PictureBox
            {
                BackgroundImage = Properties.Resources.logo,
                Size = new Size(182, 64),
                Location = new Point(400, 10),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            loginPanel.Controls.Add(registerLogo);
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
                Visible = false,
                BackgroundImage = Properties.Resources.backround,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            PictureBox registerLogo = new PictureBox
            {
                BackgroundImage = Properties.Resources.logo,
                Size = new Size(182, 64),
                Location = new Point(400, 10),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            registerPanel.Controls.Add(registerLogo);

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
            using (var myMan = new MySqlManager())
            {
                string passHash = myMan.Benutzer.Hash(password);
                if (!email.Contains("@") || !email.Contains(".com"))
                {
                    MessageBox.Show("Keine gültige Email!");
                    return;
                }
                if (nName.Any(char.IsDigit) || vName.Any(char.IsDigit))
                {
                    MessageBox.Show("Ungültiger Name!");
                    return;
                }
                myMan.Benutzer.BenutzerAnlegen(email, vName, nName, passHash, BID, loginID, activeUser);
                MessageBox.Show("Bitte, logen Sie sich ein");
                registerPanel.Visible = false;
                loginPanel.Visible = true;
                
            }
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
            using (var myMan = new MySqlManager())
            {
                myMan.Benutzer.BenutzerEinloggen(email, password, loginEmailInput.Text, loginPasswordInput.Text, activeUser, loginPanel, flowLayoutPanel, homePanel);
            }
            LoadActiveUser();
            if(activeUser != null) 
            {
                Kredite.HoleKrediteAusDatenbank(activeUser);
            }
            homePanel.Controls.Clear();
            PictureBox Logo = new PictureBox
            {
                BackgroundImage = Properties.Resources.logo,
                Size = new Size(364, 128),
                Location = new Point(homePanel.Location.X / 2, homePanel.Location.Y / 2),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            homePanel.Controls.Add(Logo);

            Button buttonLogout = new Button
            {
                Size = new Size(50, 50),
                Location = new Point(440, 0),
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

            Benutzer aNutzer = activeUser;
            }
        
 
        public void ShowKaufPanel(Aktie aktie)
        {
            kaufPanel.Controls.Clear();
            kaufPanel.Visible = true;
            kaufPanel.BringToFront();

            LoadActiveUser();
            using (var myMan = new MySqlManager())
            {
                var depots = myMan.Depot.GetUserDepot(Convert.ToInt32(activeUser.benutzerID));
                if (depots.Count == 0)
                {
                    Label info = new Label
                    {
                        Text = "Du hast noch kein Depot. Bitte benenne dein neues Depot:",
                        Location = new Point(10, 10),
                        AutoSize = true
                    };
                    kaufPanel.Controls.Add(info);
                    TextBox depotNameTxt = new TextBox
                    {
                        Location = new Point(10, 40),
                        Width = 200
                    };
                    kaufPanel.Controls.Add(depotNameTxt);
                    Button createDepotBtn = new Button
                    {
                        Text = "Depot erstellen",
                        Location = new Point(10, 70),
                        Width = 120
                    };
                    kaufPanel.Controls.Add(createDepotBtn);
                    createDepotBtn.Click += (s, e) =>
                    {
                        string depotName = depotNameTxt.Text.Trim();
                        if (string.IsNullOrEmpty(depotName))
                        {
                            MessageBox.Show("Bitte gib einen gültigen Namen ein.");
                            return;
                        }
                        myMan.Depot.CreateDepot(depotName, Convert.ToInt32(activeUser.benutzerID));
                        ShowKaufPanel(aktie);

                    };
                    Button abbrechBtn = new Button
                    {
                        Text = "Abbrechen",
                        Location = new Point(140, 70),
                        Width = 80
                    };
                    abbrechBtn.Click += (s, e) => kaufPanel.Visible = false;
                    kaufPanel.Controls.Add(abbrechBtn);
                    return;
                }

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

                Label depotLb = new Label
                {
                    Text = "Depot wählen:",
                    Location = new Point(110, 130),
                    AutoSize = true
                };
                kaufPanel.Controls.Add(depotLb);
                ComboBox depotBox = new ComboBox
                {
                    Location = new Point(110, 160),
                    Width = 180,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                foreach (var depot in depots)
                {
                    depotBox.Items.Add(new { depot.ID, depot.name });
                }
                depotBox.DisplayMember = "name";
                depotBox.SelectedIndex = 0;
                kaufPanel.Controls.Add(depotBox);

                Button kaufBtn = new Button
                {
                    Text = "Kaufen",
                    Location = new Point(10, 200),
                    Width = 120,
                    Height = 30
                };
                Button closeBtn = new Button
                {
                    Text = "Abbrechen",
                    Location = new Point(220, 200),
                    Width = 120,
                    Height = 30
                };
                closeBtn.Click += (s, e) => kaufPanel.Visible = false;
                kaufPanel.Controls.Add(closeBtn);
                kaufBtn.Click += (s, e) =>
                {
                    if (depotBox.SelectedItem == null) return;
                    DialogResult result = MessageBox.Show($"Kaufe {anteilNum.Value} Anteile der Aktie {aktie.firma}.\nGesamtpreis: {aktie.CurrentValue * Convert.ToDouble(anteilNum.Value):f2}€", "Kauf bestätigen", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK && activeUser.kontoStand > aktie.CurrentValue * Convert.ToDouble(anteilNum.Value))
                    {
                        LoadActiveUser();
                        myMan.Transaktion.AddTransaktion(aktie.id, "Kauf", Convert.ToDouble(anteilNum.Value), aktie.CurrentValue, activeUser);
                        //activeUser.GeldAbziehen(aktie.CurrentValue * Convert.ToDouble(anteilNum.Value));
                        MessageBox.Show("Kauf erfolgreich durchgeführt.");
                    }
                    else if(result != DialogResult.OK)
                    {
                        MessageBox.Show("Kauf abgebrochen");
                    }
                    else 
                    {
                        MessageBox.Show("Kein Geld!");
                    }
                    
                    kaufPanel.Visible = false;
                };
                kaufPanel.Controls.Add(kaufBtn);
            }
        }
        public void ShowKreditPanel(DataGridView aktiveKredite)
        {
            LoadActiveUser();
            Kredite kredit = new Kredite(0, 0, 0, 0, activeUser, 0);
            LoadActiveUser();
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

            if (activeUser.rating == Kredite.CreditRating.D)
            {
                auswahlMenge.Maximum = 2000;
                auswahlLaufzeit.Maximum = 6;
            }
            else if (activeUser.rating == Kredite.CreditRating.C)
            {
                auswahlMenge.Maximum = 5000;
                auswahlLaufzeit.Maximum = 18;
            }
            else if (activeUser.rating == Kredite.CreditRating.B)
            {
                auswahlMenge.Maximum = 7500;
                auswahlLaufzeit.Maximum = 24;
            }
            else if (activeUser.rating == Kredite.CreditRating.A)
            {
                auswahlMenge.Maximum = 10000;
                auswahlLaufzeit.Maximum = 48;
            }

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
                LoadActiveUser();
                if(activeUser.rating == Kredite.CreditRating.D && activeUser.kredite.Count != 0) 
                {
                    MessageBox.Show("Du darfst aufgrund deines Credit-Ratings nicht mehr als 1 Kredit aufnehmen!");
                    kreditPanel.Visible = false;
                }
                else 
                {
                    kredit.Betrag = (double)auswahlMenge.Value;
                    kredit.Restschuld = (double)auswahlMenge.Value * (1 + (double)kredit.bestimmeZinssatz() / 100);
                    kredit.Laufzeit = (int)auswahlLaufzeit.Value;
                    kredit.KreditHinzufuegen(kredit.Betrag, kredit.Zinssatz, kredit.Restschuld, kredit.Laufzeit, activeUser, aktiveKredite, kredit);
                }
                kreditPanel.Visible = false;
            }; 
        }
        public void ShowVerkaufPanel(Transaktion transaktion)
        {
            Aktie aktie;
            using (var myMan = new MySqlManager())
            {
                aktie = myMan.Aktien.LoadAktieByID(transaktion.aktieID);
            }
            Panel verkaufPanel = new Panel
            {
                Size = new Size(350, 240),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            verkaufPanel.Location = new Point((homePanel.Width - verkaufPanel.Width) / 2,(homePanel.Height - verkaufPanel.Height) / 2);

            Label info = new Label
            {
                Text = $"Verkauf: {aktie.firma}\n" +
                       $"Anteile: {transaktion.anzahl:F2}\n" +
                       $"Kaufpreis: {transaktion.einzelpreis:F2}€\n" +
                       $"Aktuell: {aktie.CurrentValue:F2}€",
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("Arial", 10)
            };
            verkaufPanel.Controls.Add(info);

            double diff = aktie.CurrentValue - Convert.ToDouble(transaktion.einzelpreis);
            double diffPercent = diff / (double)transaktion.einzelpreis * 100;

            Label diffLabel = new Label
            {
                Text = $"Veränderung: {diffPercent:F2}%",
                ForeColor = diff >= 0 ? Color.Green : Color.Red,
                AutoSize = true,
                Location = new Point(10, 80)
            };
            verkaufPanel.Controls.Add(diffLabel);

            Label mengeLabel = new Label
            {
                Text = "Menge:",
                Location = new Point(10, 115),
                AutoSize = true
            };
            verkaufPanel.Controls.Add(mengeLabel);

            TextBox mengeTb = new TextBox
            {
                Location = new Point(70, 112),
                Width = 50
            };
            verkaufPanel.Controls.Add(mengeTb);

            Button verkaufenBtn = new Button
            {
                Text = "Verkaufen",
                Location = new Point(10, 160),
                Width = 100,
                Height = 30,
                BackColor = Color.LightGreen
            };
            verkaufenBtn.Click += (s, e) =>
            {
                if (double.TryParse(mengeTb.Text, out double menge) && menge > 0 && menge <= transaktion.anzahl)
                {
                    double erloes = menge * aktie.CurrentValue;

                    // Geld gutschreiben
                    LoadActiveUser();
                    activeUser.GeldHinzufuegen(erloes);

                    transaktion.anzahl -= menge;
                    using (var myMan = new MySqlManager())
                    {
                        myMan.Transaktion.AktualisiereTransaktion(transaktion);
                    }
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
            Button abbrechenBtn = new Button
            {
                Text = "Abbrechen",
                Location = new Point(170, 160),
                Width = 140,
                Height = 30,
                BackColor = Color.LightGray
            };
            abbrechenBtn.Click += (s, e) =>
            {
                homePanel.Controls.Remove(verkaufPanel);
            };
            verkaufPanel.Controls.Add(abbrechenBtn);
            homePanel.Controls.Add(verkaufPanel);
            verkaufPanel.BringToFront();
            homePanel.Resize += (s, e) =>
            {
                if (verkaufPanel != null)
                {
                    verkaufPanel.Location = new Point((homePanel.Width - verkaufPanel.Width) / 2, (homePanel.Height - verkaufPanel.Height) / 2);
                }
            };
        }

        public void SimuliereNächstenTag()
        {
            using (var myMan = new MySqlManager())
            { 
            var alleBenutzer = myMan.Benutzer.LadeAlleBenutzer();
            var aktuellerBenutzer = myMan.Benutzer.ReturnActiveUser(activeUser);

            Dictionary<int, int> nachfrage = new Dictionary<int, int>();
            Random rand = new Random();
            foreach(var benutzer in alleBenutzer)
            {
                if (benutzer.benutzerID == aktuellerBenutzer.benutzerID)
                    continue;
                var depots = myMan.Depot.GetUserDepot(Convert.ToInt32(benutzer.benutzerID));
                if (depots.Count == 0)
                {
                    myMan.Depot.CreateDepot("Standarddepot", Convert.ToInt32(benutzer.benutzerID));
                    depots = myMan.Depot.GetUserDepot(Convert.ToInt32(benutzer.benutzerID));
                }
                if (benutzer.GetKontoStand() < 15) // Unterstützungs geld
                    benutzer.GeldHinzufuegen(20.00);
                foreach(var depot in depots)
                {
                        int maxAktionProTag = 2;
                        int aktionHeute = 0;
                        foreach (var aktie in stonks)
                        {
                            int choice = rand.Next(0, 3); // 0 = nichts, 1 = kaufen, 2 = verkaufen
                            if (aktionHeute >= maxAktionProTag) break;
                            switch (choice)
                            {
                            case 1:
                                int mengeKauf = Convert.ToInt32(rand.NextDouble() * 5);
                                    if (mengeKauf < 2)
                                        continue;
                                double kosten = mengeKauf * aktie.CurrentValue;
                                if(benutzer.kontoStand >= kosten)
                                {
                                    myMan.Transaktion.AddTransaktion(aktie.id, "Kauf", mengeKauf, aktie.CurrentValue, benutzer);
                                    benutzer.UpdateKontoStand(kosten, benutzer.benutzerID);
                                    nachfrage.TryGetValue(aktie.id, out int wert);
                                    nachfrage[aktie.id] = wert + 1; 
                                }
                                break;
                            case 2:
                                var transaktion = myMan.Transaktion.LadeTransaktionenFürDepot(depot.ID).Where(t => t.aktieID == aktie.id && t.anzahl > 0).ToList();
                                if (transaktion.Any())
                                {
                                    var trans = transaktion[rand.Next(transaktion.Count)];
                                    double verkaufsMenge = Math.Min(trans.anzahl, Math.Round(rand.NextDouble() * 5, 2));
                                    if (verkaufsMenge > 0.5)
                                    {
                                        double erloes = verkaufsMenge * aktie.CurrentValue;
                                        if (trans.anzahl - verkaufsMenge <= 2) 
                                        {
                                            verkaufsMenge = trans.anzahl;
                                        } 
                                        trans.anzahl -= verkaufsMenge;
                                        if (trans.anzahl <= 0)
                                        {
                                            myMan.Transaktion.LöscheTransaktion(trans.id);
                                        }
                                        else
                                        {
                                            myMan.Transaktion.AktualisiereTransaktion(trans);
                                        }
                                        benutzer.GeldHinzufuegen((int)erloes);
                                        nachfrage.TryGetValue(aktie.id, out int wert);
                                        nachfrage[aktie.id] = wert - 1;
                                    }
                                }
                                break;
                        }
                            aktionHeute++;
                    }
                }
            }
                foreach (var aktie in stonks)
                {
                    nachfrage.TryGetValue(aktie.id, out int delta);
                    double prozent = Math.Min(5, Math.Abs(delta) * 0.01); // max 5% veränderung
                    if (delta > 0) // steigt
                    {
                        aktie.CurrentValue *= (double)(1 + prozent);
                        AddEreignisse(aktie);
                        aktie.SimulateNextStep(aktie.CurrentValue);
                    }
                    else if (delta < 0) // fällt
                    {
                        aktie.CurrentValue *= (double)(1 - prozent);
                        AddEreignisse(aktie);
                        aktie.SimulateNextStep(aktie.CurrentValue);
                    }
                    else
                    {
                        AddEreignisse(aktie);
                        aktie.SimulateNextStep();
                    }
                    myMan.Aktien.UpdateAktie(aktie);
                }
            }
        }
        public void AddEreignisse(Aktie aktie)
        {
            using (var myMan = new MySqlManager())
            { 
                var globaleEvents = myMan.Ereignis.LadeAktiveEreignisse("global");
                var lokaleEvents = myMan.Ereignis.LadeAktiveEreignisse("lokal");
                Random rand = new Random();
                List<Ereigniss> ereignisse = new List<Ereigniss>();
                if (rand.NextDouble() < 0.005 && globaleEvents.Any()) // 0.5 % für globale 
                    ereignisse.Add(globaleEvents[rand.Next(globaleEvents.Count)]); 
                if (rand.NextDouble() < 0.01 && lokaleEvents.Any()) // 0.1 % für lokale
                    ereignisse.Add(lokaleEvents[rand.Next(lokaleEvents.Count)]);
                foreach (Ereigniss e in ereignisse)
                {
                    aktie.CurrentValue *= 1 + e.EinflussProzent;
                    Console.WriteLine($"Ereignis für {aktie.name}: {e.Name} – {e.Beschreibung} ({e.EinflussProzent:P})");
                }
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
        public void LoadActiveUser()
        {
            using (var myMan = new MySqlManager())
            {

                activeUser = myMan.Benutzer.ReturnActiveUser(activeUser);
                if (activeUser != null)
                {
                    activeUser.kontoStand = activeUser.GetKontoStand();
                    Kredite.HoleKrediteAusDatenbank(activeUser);
                }
                else
                {
                    MessageBox.Show("Falsche Logindaten!");
                }
            }
        }
    }
}