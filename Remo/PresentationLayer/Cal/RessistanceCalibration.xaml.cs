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
using System.Windows.Shapes;
using System.ComponentModel;
using DataAccessLayer;
using DNBSoft.WPF.WPFGraph;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for ResssistanceCalibration.xaml
    /// </summary>
    public partial class ResssistanceCalibration : Window
    {

        private const string RessistanceCalibrationFilePath = "Ref\\RessistanceCalibration.xml";

        DNBSoft.WPF.WPFGraph.WPFGraphSeries series1;
        private EntityLayer.RessistanceCalibration r;
        public ResssistanceCalibration()
        {
            InitializeComponent();

            r = new EntityLayer.RessistanceCalibration();
            r.readXml(RessistanceCalibrationFilePath);
            r.RessistanceCalMeasurenments.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(tempCalibrationService_PropertyChanged);

            MeasurenmentListView.ItemsSource = r.RessistanceCalMeasurenments;
            this.initGraph();
            this.refreshGraph();
        }

        private void tempCalibrationService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MeasurenmentListView.ItemsSource = null;
            MeasurenmentListView.ItemsSource = r.RessistanceCalMeasurenments;
        }

        private void dodadiButton_Click(object sender, RoutedEventArgs e)
        {
            RessistanceCalibrationDialog1 d = new RessistanceCalibrationDialog1();
            if (d.ShowDialog() == true)
            {
                r.RessistanceCalMeasurenments.Add(d.RessistanceCalMeasurenment);
                refreshGraph();

                r.writeToXml(RessistanceCalibrationFilePath);
            }
        }

        private void izbrisiButton_Click(object sender, RoutedEventArgs e)
        {
            if (MeasurenmentListView.SelectedItem != null)
            {
                r.RessistanceCalMeasurenments.Remove((EntityLayer.RessistanceCalMeasurenment)MeasurenmentListView.SelectedItem);
                r.writeToXml(RessistanceCalibrationFilePath);
            }
        }

        private void izlezButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void initGraph()
        {
            series1 = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            series1.Name = "Res";
 
            graph.Series.Add(series1);

            WPFGraphPointRenderers.RoundPoint series1PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series1PointRenderer.PointBrush = Brushes.Red;
            series1PointRenderer.PointSize = 5;
            series1.PointRenderer = series1PointRenderer;
            WPFGraphLineRenderers.DashedLine series1LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series1LineRenderer.LineBrush = Brushes.Pink;
            series1.LineRenderer = series1LineRenderer;
        }

        private void refreshGraph()
        {
            double maxY = graph.MaxYRange;
            double minY = graph.MinYRange;
            double maxX = 0;
            double minX = 1000;

            series1.Points.Clear();
            foreach (EntityLayer.RessistanceCalMeasurenment meas in r.RessistanceCalMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = meas.RMeas;
                point.Y = meas.RErr;
                if (point.Y > maxY)
                    maxY = point.Y + 0.1;
                if (point.Y < minY)
                    minY = point.Y - 0.1;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                series1.Points.Add(point);
            }
                graph.IntervalYRange = (maxY - minY) / 20;
                graph.IntervalXRange = (maxX - minX) / 10;

                graph.MaxYRange = maxY;
                graph.MinYRange = minY;
                graph.MaxXRange = maxX;
                graph.MinXRange = minX;
               
                graph.Refresh();
            
        }

    }
}
