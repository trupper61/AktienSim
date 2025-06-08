using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aktiensim
{
    public class Kredite
    {
        public double Betrag;
        public int Zinssatz;
        public double Restschuld;
        public int Laufzeit;
        public Benutzer Benutzer;

        public Kredite(double betrag, int zinssatz, double restschuld, int laufzeit, Benutzer benutzer)
        {
            this.Betrag = betrag;
            this.Zinssatz = zinssatz;
            this.Restschuld = restschuld;
            this.Laufzeit = laufzeit;
            this.Benutzer = benutzer;
        }

        public int bestimmeZinssatz() 
        {
            switch(Benutzer.rating) 
            {
                case CreditRating.A:
                    this.Zinssatz = 3;
                    return this.Zinssatz;
                case CreditRating.B:
                    this.Zinssatz = 5;
                    return this.Zinssatz;
                case CreditRating.C:
                    this.Zinssatz = 10;
                    return this.Zinssatz;
                case CreditRating.D:
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
            string kreditAdd = "INSERT INTO kredite(Betrag, ID_Benutzer, Zinssatz, Restschuld, Laufzeit) VALUES(@betrag, @ID_Benutzer, @zinssatz, @restschuld, @laufzeit)";
            SqlConnection.ExecuteNonQuery(kreditAdd,
                new MySqlParameter("@ID_Benutzer", benutzer.benutzerID),
                new MySqlParameter("@betrag", betrag),
                new MySqlParameter("@zinssatz", zinssatz),
                new MySqlParameter("@restschuld", restschuld),
                new MySqlParameter("@laufzeit", laufzeit));
            benutzer.GeldHinzufuegen(betrag);
            benutzer.kredite.Add(kredit);

            aktiveKredite.Rows.Add(betrag, restschuld, zinssatz, DateTime.Now, laufzeit);
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
