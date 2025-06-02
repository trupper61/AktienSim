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
        public Aktie aktie;
        private string typ;
        public double anzahl;
        public decimal einzelpreis;
        public Transaktion(int id,Aktie aktie, double anzahl, decimal einzelpreis, string typ)
        {
            this.aktie = aktie;
            this.anzahl = anzahl;
            this.einzelpreis = einzelpreis;
            this.typ = typ;
        }
    }
}
