using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace aktiensim
{
    public class Kredite
    {
        public double Betrag;
        public int Zinssatz;
        public double Restschuld;
        public int Laufzeit;
        public double zuZahlendeRate;
        public Benutzer Benutzer;
        public int KreditID;

        public Kredite(double betrag, int zinssatz, double restschuld, int laufzeit, Benutzer benutzer, int kreditID)
        {
            this.Betrag = betrag;
            this.Zinssatz = zinssatz;
            this.Restschuld = restschuld;
            this.Laufzeit = laufzeit;
            this.Benutzer = benutzer;
            this.zuZahlendeRate = restschuld / laufzeit;
            this.KreditID = kreditID;
        }

        public int bestimmeZinssatz() 
        {
            switch(Benutzer.rating) 
            {
                case CreditRating.A:
                    this.Zinssatz = 8;
                    return this.Zinssatz;
                case CreditRating.B:
                    this.Zinssatz = 15;
                    return this.Zinssatz;
                case CreditRating.C:
                    this.Zinssatz = 25;
                    return this.Zinssatz;
                case CreditRating.D:
                    this.Zinssatz = 50;
                    break;
                default:
                    MessageBox.Show("Fehler!");
                    break;
            }
            return 0;
            
        }

        public enum CreditRating 
        {
            A, B, C, D
        }

        public void KreditHinzufuegen(double betrag, int zinssatz, double restschuld, int laufzeit, Benutzer benutzer, DataGridView aktiveKredite, Kredite kredit) 
        {
            string kreditAdd = "INSERT INTO kredite(Betrag, ID_Benutzer, Zinssatz, Restschuld, Laufzeit, Rate) VALUES (@betrag, @ID_Benutzer, @zinssatz, @restschuld, @laufzeit, @Rate); SELECT LAST_INSERT_ID();";


            kredit.zuZahlendeRate = kredit.Restschuld / kredit.Laufzeit;

            int neueId = SqlConnection.ExecuteInsertWithId(kreditAdd,
                new MySqlParameter("@ID_Benutzer", benutzer.benutzerID),
                new MySqlParameter("@betrag", betrag),
                new MySqlParameter("@zinssatz", zinssatz),
                new MySqlParameter("@restschuld", restschuld),
                new MySqlParameter("@laufzeit", laufzeit),
                new MySqlParameter("@rate", kredit.zuZahlendeRate));
            benutzer.GeldHinzufuegen(betrag);
            

            kredit.KreditID = neueId;

            benutzer.kredite.Add(kredit);
            aktiveKredite.Rows.Add(betrag, restschuld, zinssatz, DateTime.Now, laufzeit);

        }

        public static void HoleKrediteAusDatenbank(Benutzer benutzer) 
        {
            string kreditAdd = "SELECT KreditID, Betrag, ID_Benutzer, Zinssatz, Restschuld, Laufzeit, Rate FROM kredite WHERE ID_Benutzer = @ID_Benutzer";

            List<Kredite> kredite = new List<Kredite>();
            using (var MyMan = new MySqlManager()) 
            {
                using (MySqlCommand cmds = new MySqlCommand(kreditAdd, MyMan.Connection))
                {
                    cmds.Parameters.AddWithValue("ID_Benutzer", benutzer.benutzerID);
                    MySqlDataReader reader = cmds.ExecuteReader();
                    while (reader.Read())
                    {
                        Kredite kredit = new Kredite(0, 0, 0, 0, benutzer, 0);
                        kredit.Betrag = Convert.ToDouble(reader["Betrag"]);
                        kredit.Zinssatz = Convert.ToInt32(reader["Zinssatz"]);
                        kredit.Restschuld = Convert.ToDouble(reader["Restschuld"]);
                        kredit.Laufzeit = Convert.ToInt32(reader["Laufzeit"]);
                        kredit.zuZahlendeRate = Convert.ToInt32(reader["Rate"]);
                        kredit.KreditID = Convert.ToInt32(reader["KreditID"]);
                        kredite.Add(kredit);
                    }
                    benutzer.kredite = kredite;
                }

                //Reader liest alle Zeilen der Tabelle und weist die Eigenschaften von Kredit die jeweiligen Werte zu.
                
            }
        }

        public void UpdateKreditStatus(Kredite kredit) //Für die Simulierung einer Woche
        {
            string kreditUpdate = "UPDATE kredite SET laufzeit = @laufzeit WHERE KreditID = @KreditID; UPDATE kredite SET Restschuld = @RestschuldNeu WHERE KreditID = @KreditID";

            SqlConnection.ExecuteNonQuery(kreditUpdate,
                new MySqlParameter("@laufzeit", kredit.Laufzeit),
                new MySqlParameter("@RestschuldNeu", kredit.Restschuld),
                new MySqlParameter("@KreditID", kredit.KreditID));
        }

        public void KreditLoeschen() //Kredit kann abbezahlt werden
        {
            string kreditDelete = "DELETE FROM kredite WHERE Laufzeit = 0;";

            SqlConnection.ExecuteNonQuery(kreditDelete, null);
        }

        public static void RefreshDataGridView(DataGridView aktiveKredite, Benutzer benutzer) 
        {
            foreach(Kredite kred in benutzer.kredite) 
            {
                aktiveKredite.Rows.Add(kred.Betrag, kred.Restschuld, kred.Zinssatz, DateTime.Now, kred.Laufzeit);
            }
            
        }
        
    }
}
