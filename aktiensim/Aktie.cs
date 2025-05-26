using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.WinForms;
using System.Windows.Forms;
using System.Drawing;

namespace aktiensim
{
    public class Aktie
    {
        public List<double> timeX;
        public List<double> amountY;
        public string name;
        public FormsPlot plot;
        int counter;
        public Aktie(string name)
        {
            this.name = name;
            timeX = new List<double>();
            amountY = new List<double>();
            plot = new FormsPlot();
            counter = 0;
        }
        public void UpdateChart()
        {
            counter++;
            Random rand = new Random();
            amountY.Add(rand.NextDouble() * rand.Next(1, 50));
            timeX.Add(counter);
            if (counter == 20)
            {
                counter--;
                amountY.RemoveAt(0);
            }
            plot.Plot.Add.Scatter(timeX, amountY, ScottPlot.Color.FromColor(Color.Red));
            plot.Plot.Axes.AutoScale();
            plot.Refresh();
        }
    }
}
