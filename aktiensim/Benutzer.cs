using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Benutzer
    {
        public string name;
        public string vorname;
        public string email;
        public int benutzerID;
        public int kontoStand;
        public Benutzer (string name, string vorname, string email, int benutzerID, int kontoStand)
        {
            this.name = name;
            this.vorname = vorname;
            this.email = email;
            this.benutzerID = benutzerID;
            this.kontoStand = kontoStand;
        }
    }
}
