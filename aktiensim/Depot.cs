using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aktiensim
{
    public class Depot
    {
        public int ID { get; private set; }
        public string name;
        public List<Transaktion> transaktions;
        public Depot(int id, string name) 
        {
            this.ID = id;
            this.name = name;
        }
    }
}
