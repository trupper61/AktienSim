using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Ereigniss
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double EinflussProzent { get; set; }
        public string Beschreibung { get; set; }
        public string Typ { get; set; }
        public Ereigniss(int iD, string name, double einflussProzent, string beschreibung, string typ)
        {
            ID = iD;
            Name = name;
            EinflussProzent = einflussProzent;
            Beschreibung = beschreibung;
            Typ = typ;
        }
    }
}
