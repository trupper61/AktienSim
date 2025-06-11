using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.WinForms;
using System.Windows.Forms;
using System.Drawing;
using System.IO.Pipelines;

namespace aktiensim
{
    public class Aktie
    {
        public int id;
        public string name;
        public string firma;

        public double CurrentValue { get; set; }
        public double LastClose { get; private set; }

        public List<double> timeX;
        public List<double> ValueHistory;
        public FormsPlot plot;

        private int counter;
        private static Random rand = new Random();
        private static AktienVerwaltung stocksManager = new AktienVerwaltung();

        public Aktie(string name, string firma, double startValue, int id, double lastClose = 0)
        {
            this.name = name;
            this.firma = firma;
            this.CurrentValue = startValue;
            this.id = id;
            this.LastClose = lastClose;

            timeX = new List<double>();
            ValueHistory = new List<double>();
            counter = 0;

            plot = new FormsPlot();
            plot.Dock = DockStyle.Fill;
            plot.Plot.Title(firma);
            plot.Plot.HideGrid();

            InitHistory(startValue);
        }

        private void InitHistory(double start)
        {
            ValueHistory.Add(start);
            for (int i = 0; i < 20; i++)
            {
                counter++;
                timeX.Add(counter);
                ValueHistory.Add(ValueHistory.Last() + RandomChange());
            }
            PlotChart();
        }

        private double RandomChange()
        {
            return ValueHistory.Last() * (rand.NextDouble() * 0.02 - 0.01); 
        }

        public void SimulateNextStep(double? overrideNewValue = null)
        {
            counter++;

            if (overrideNewValue.HasValue)
                CurrentValue = overrideNewValue.Value;
            else
                CurrentValue *= 1 + (rand.NextDouble() * 0.02 - 0.01);

            CurrentValue = Math.Round(CurrentValue, 2);

            timeX.Add(counter);
            ValueHistory.Add(CurrentValue);

            if (timeX.Count > 20)
            {
                timeX.RemoveAt(0);
                ValueHistory.RemoveAt(0);
            }
            if (counter % 10 == 0)
                SetLastClose(CurrentValue);
            PlotChart();
            stocksManager.UpdateAktie(this);
        }

        private void PlotChart()
        {
            plot.Plot.Clear();
            plot.Plot.Add.Scatter(timeX.ToArray(), ValueHistory.ToArray(), ScottPlot.Color.FromColor(Color.Blue));

            if (LastClose > 0)
            {
                var hline = plot.Plot.Add.HorizontalLine(LastClose);
                hline.Color = CurrentValue >= LastClose
                    ? ScottPlot.Color.FromColor(Color.Green)
                    : ScottPlot.Color.FromColor(Color.Red);
            }

            plot.Plot.Axes.AutoScale();
            plot.Refresh();
        }

        public void SetLastClose(double value)
        {
            LastClose = value;
        }
    }
}
