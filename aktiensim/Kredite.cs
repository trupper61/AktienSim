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
            Betrag = betrag;
            Zinssatz = zinssatz;
            Restschuld = restschuld;
            Laufzeit = laufzeit;
            Benutzer = benutzer;
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
    }
}
