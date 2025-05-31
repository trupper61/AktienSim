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
        public List<double> timeX;
        public double CurrentValue { get; private set; }
        public string name;
        public string firma;
        public FormsPlot plot;
        public double LastClose { get; private set; }
        public List<double> ValueHistory { get; private set; }

        private int counter;
        public static int id = 0;
        private static Random rand = new Random();
        private Timer nextStep;
        private static Timer nextLastClose;
        private static AktienVerwaltung stocksManager = new AktienVerwaltung();
        public Aktie(string name, string firma,double startValue, double lastClose = 0)
        {
            this.name = name;
            this.firma = firma;
            this.LastClose = lastClose;
            CurrentValue = startValue;
            timeX = new List<double>();
            ValueHistory = new List<double> ();
            ValueHistory.Add(startValue);
            plot = new FormsPlot();
            plot.Dock = DockStyle.Fill;
            counter = 0;
            id++;
            InitializeChartData();
            nextStep = new Timer();
            nextStep.Interval = 10000;
            nextStep.Tick += NextStep_Tick;
            nextStep.Start();
            nextLastClose = new Timer();
            nextLastClose.Interval = 120000;
            nextLastClose.Tick += NextLastClose_Tick;
            nextLastClose.Start();
        }
        private void NextLastClose_Tick(object sender, EventArgs e)
        {
            SetLastClose(CurrentValue);
        }
        private void NextStep_Tick(object sender, EventArgs e)
        {
            SimulateNextStep();
        }
        public double RandomChange()
        {
            double changePercent = rand.NextDouble() * 0.02 - 0.01; // Cahnge either -1% | +1%
            return ValueHistory.Last() * changePercent;
        }
        private void InitializeChartData()
        {
            for (int i = 0; i < 20; i++)
            {
                counter++;
                timeX.Add(counter);
                ValueHistory.Add(ValueHistory.Last() + RandomChange());
            }
            SimulateNextStep();
        }
        public void SimulateNextStep()
        {
            counter++;
            double changePercent = rand.NextDouble() * 0.02 - 0.01; // Change either -1% | +1%
            CurrentValue *= (1 + changePercent);
            CurrentValue = Math.Round(CurrentValue, 2);

            timeX.Add(counter);
            ValueHistory.Add(CurrentValue);
            
            if (timeX.Count > 20)
            {
                timeX.RemoveAt(0);
                ValueHistory.RemoveAt(0);
            }
            plot.Plot.Clear();
            plot.Plot.Add.Scatter(timeX, ValueHistory, ScottPlot.Color.FromColor(Color.Blue));

            if (LastClose > 0)
            {
                var hline = plot.Plot.Add.HorizontalLine(LastClose);
                hline.Color = CurrentValue >= LastClose ? ScottPlot.Color.FromColor(Color.Green) : ScottPlot.Color.FromColor(Color.Red);
            }
            plot.Plot.Axes.AutoScale();
            plot.Refresh();
            stocksManager.UpdateAktie(this);
        }
        public void SetLastClose(double value)
        {
            LastClose = value;
        }
    }
}
