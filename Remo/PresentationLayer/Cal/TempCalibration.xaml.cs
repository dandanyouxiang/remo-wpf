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
using DataAccessLayer;
using System.Collections;
using System.ComponentModel;
using DNBSoft.WPF.WPFGraph;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for TempCalibration.xaml
    /// </summary>
    public partial class TempCalibration : Window
    {
        private TempCalibrationService tempCalibrationService;
        private const string TemperatureCalibrationFilePath = "Ref\\TemperatureCalibration.xml";

        DNBSoft.WPF.WPFGraph.WPFGraphSeries series1;
        DNBSoft.WPF.WPFGraph.WPFGraphSeries series2;
        DNBSoft.WPF.WPFGraph.WPFGraphSeries series3;
        DNBSoft.WPF.WPFGraph.WPFGraphSeries series4;
        public TempCalibration()
        {
            InitializeComponent();

            this.initGraph();

            tempCalibrationService = new TempCalibrationService();
            tempCalibrationService.TempCalibrationEntity = new EntityLayer.TempCalibration();
            tempCalibrationService.TempCalibrationEntity.readXml(TemperatureCalibrationFilePath);
            
            tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments.PropertyChanged += tempCalibrationService.TempCalMeasurenments_PropertyChanged;
            tempCalibrationService.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(tempCalibrationService_PropertyChanged);

            MeasurenmentListView.ItemsSource = tempCalibrationService.TempCalMeasurenments;

            this.refreshGraph();
        }

        private void tempCalibrationService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MeasurenmentListView.ItemsSource = tempCalibrationService.TempCalMeasurenments;
        }


        private void izlezButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dodadiButton_Click(object sender, RoutedEventArgs e)
        {
            TempCalibrationDialog tempCalDialog = new TempCalibrationDialog();
            if (tempCalDialog.ShowDialog() == true)
            {
                tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments.Add(tempCalDialog.TempCalMeasurenment);
                this.refreshGraph();
                tempCalibrationService.TempCalibrationEntity.writeToXml(TemperatureCalibrationFilePath);
            }
        }

        private void izbrisiButton_Click(object sender, RoutedEventArgs e)
        {
            if (MeasurenmentListView.SelectedItem != null)
            {
                DateTime time = ((TempCalibrationService.TempCalMeasurenmentsView)MeasurenmentListView.SelectedItem).Time;
                EntityLayer.TempCalMeasurenment measToRemove = 
                    tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments.Where(t => t.Time.Equals(time)).Single();
                tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments.Remove(measToRemove);
            }
        }

        private void initGraph()
        {
            series1 = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            series2 = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            series3 = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            series4 = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            series1.Name = "T1";
            series2.Name = "T2";
            series3.Name = "T3";
            series4.Name = "T4";
            graph.Series.Add(series1);
            graph.Series.Add(series2);
            graph.Series.Add(series3);
            graph.Series.Add(series4);

            WPFGraphPointRenderers.RoundPoint series1PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series1PointRenderer.PointBrush = Brushes.Red;
            series1PointRenderer.PointSize = 5;
            series1.PointRenderer = series1PointRenderer;
            WPFGraphLineRenderers.DashedLine series1LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series1LineRenderer.LineBrush = Brushes.Pink;
            series1.LineRenderer = series1LineRenderer;

            WPFGraphPointRenderers.RoundPoint series2PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series2PointRenderer.PointBrush = Brushes.Blue;
            series2PointRenderer.PointSize = 5;
            series2.PointRenderer = series2PointRenderer;
            WPFGraphLineRenderers.DashedLine series2LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series2LineRenderer.LineBrush = Brushes.LightBlue;
            series2.LineRenderer = series2LineRenderer;

            WPFGraphPointRenderers.RoundPoint series3PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series3PointRenderer.PointBrush = Brushes.Green;
            series3PointRenderer.PointSize = 5;
            series3.PointRenderer = series3PointRenderer;
            WPFGraphLineRenderers.DashedLine series3LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series3LineRenderer.LineBrush = Brushes.LightGreen;
            series3.LineRenderer = series3LineRenderer;

            WPFGraphPointRenderers.RoundPoint series4PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series4PointRenderer.PointBrush = Brushes.Purple;
            series4PointRenderer.PointSize = 5;
            series4.PointRenderer = series4PointRenderer;
            WPFGraphLineRenderers.DashedLine series4LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series4LineRenderer.LineBrush = Brushes.LightSkyBlue;
            series4.LineRenderer = series4LineRenderer;
        }

        private void refreshGraph()
        {
            double maxY = graph.MaxYRange;
            double minY = graph.MinYRange;
            double maxX = 0;
            double minX = 1000;

            series1.Points.Clear();
            foreach (EntityLayer.TempCalMeasurenment t in tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = t.T1;
                point.Y = t.T1 - t.T1Ref;
                if (point.Y > maxY)
                    maxY = point.Y;
                if(point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                series1.Points.Add(point);
            }

            series2.Points.Clear();
            foreach (EntityLayer.TempCalMeasurenment t in tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = t.T2;
                point.Y = t.T2 - t.T2Ref;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                series2.Points.Add(point);
            }

            series3.Points.Clear();
            foreach (EntityLayer.TempCalMeasurenment t in tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = t.T3;
                point.Y = t.T3 - t.T3Ref;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                series3.Points.Add(point);
            }

            series4.Points.Clear();
            foreach (EntityLayer.TempCalMeasurenment t in tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = t.T4;
                point.Y = t.T4 - t.T4Ref;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                series4.Points.Add(point);
            }
            graph.MaxYRange = maxY;
            graph.MinYRange = minY;
            graph.MaxXRange = maxX;
            graph.MinXRange = minX;
            graph.IntervalYRange = (maxY - minY) / 20;
            graph.IntervalXRange = (graph.MaxXRange - graph.MinXRange) / 20;

            graph.Refresh();
        }
    }
}
