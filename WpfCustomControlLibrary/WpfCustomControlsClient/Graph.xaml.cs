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
            //WPFGraphSeries series1 = new WPFGraphSeries();
            //graph.Series.Add(series1);
            
            graph.MaxYRange = 2;
            graph.MinYRange = -2;
            graph.IntervalYRange = 0.2;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            graph.Series[0].Points.Clear();
            DateTime end = DateTime.Now;
            Console.WriteLine("Clear:"+(start - end).Milliseconds);

            start = DateTime.Now;
            for (int i = 0; i < 2; i++)
            {
                WPFGraphDataPoint point2 = new WPFGraphDataPoint();
                point2.X = 20;
                point2.Y = 1;
                graph.Series[0].Points.Add(point2);
            }
            end = DateTime.Now;
            Console.WriteLine("AddAll:" + (start - end).Milliseconds);
           
        }
    }
}
