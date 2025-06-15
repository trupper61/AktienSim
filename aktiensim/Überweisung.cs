using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Überweisung
    {
        private int ID;
        public int AbsenderID;
        public int EmpfaengerID;
        public double Betrag;
        public DateTime DateTime;
        public Überweisung(int iD, int absenderID, int empfaengerID, double betrag, DateTime dateTime)
        {
            ID = iD;
            AbsenderID = absenderID;
            EmpfaengerID = empfaengerID;
            Betrag = betrag;
            DateTime = dateTime;
        }
    }
}
