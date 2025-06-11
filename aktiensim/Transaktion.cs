using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Transaktion
    {
        public int id { get; private set; }
        public int aktieID;
        public string typ;
        public double anzahl;
        public decimal einzelpreis;
        public DateTime zeitpunkt;

        public Transaktion(int id, int aktie, double anzahl, decimal einzelpreis, string typ, DateTime zeitpunkt)
        {
            this.id = id;
            this.aktieID = aktie;
            this.anzahl = anzahl;
            this.einzelpreis = einzelpreis;
            this.typ = typ;
            this.zeitpunkt = zeitpunkt;
        }
    }
}