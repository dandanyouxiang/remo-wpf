using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DNBSoft.WPF.WPFGraph;

namespace Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            WPFGraphSeries series1 = new WPFGraphSeries();
            graph.Series.Add(series1);
            WPFGraphPointRenderers.RoundPoint series1PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series1PointRenderer.PointBrush = Brushes.Red;
            series1PointRenderer.PointSize = 5;
            series1.PointRenderer = series1PointRenderer;
            WPFGraphLineRenderers.DashedLine series1LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series1LineRenderer.LineBrush = Brushes.Pink;
            series1.LineRenderer = series1LineRenderer;


            graph.MaxYRange = 20;
            graph.MinYRange = -20;
            graph.IntervalYRange = 2;


            DateTime start = DateTime.Now;
            graph.Series[0].Points.Clear();
            DateTime end = DateTime.Now;
            Console.WriteLine("Clear:" + (start - end).Milliseconds);

            start = DateTime.Now;
            for (int i = 0; i < 2; i++)
            {
                WPFGraphDataPoint point2 = new WPFGraphDataPoint();
                point2.X = DateTime.Now.Ticks;
                point2.Y = 5;
                graph.Series[0].Points.Add(point2);
            }
            graph.MinXRange = 633576818867031250;
            graph.MaxXRange = 633576919967031250;
            graph.IntervalXRange = (graph.MaxXRange - graph.MinXRange) / 5;
            graph.Refresh();
            end = DateTime.Now;
            Console.WriteLine("AddAll:" + (start - end).Milliseconds + " " + DateTime.Now.Ticks);
           

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
