using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using DataAccessLayer;
using DNBSoft.WPF.WPFGraph;

namespace PresentationLayer
{
    /*
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        private void graphsInit()
        {
            
        }

        WPFGraphSeries seriesOilTemp;
        WPFGraphSeries seriesAmbTemp;
        WPFGraphSeries seriesTempRise;
        private void acGraphInit()
        {
            seriesOilTemp = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            seriesAmbTemp = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
            seriesTempRise = new DNBSoft.WPF.WPFGraph.WPFGraphSeries();
           
            seriesOilTemp.Name = "TOil";
            seriesAmbTemp.Name = "TAmb";
            seriesTempRise.Name = "TRise";

            graph.Series.Add(seriesOilTemp);
            graph.Series.Add(seriesAmbTemp);
            graph.Series.Add(seriesTempRise);

            WPFGraphPointRenderers.RoundPoint series1PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series1PointRenderer.PointBrush = Brushes.Red;
            series1PointRenderer.PointSize = 5;
            seriesOilTemp.PointRenderer = series1PointRenderer;
            WPFGraphLineRenderers.DashedLine series1LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series1LineRenderer.LineBrush = Brushes.Pink;
            seriesOilTemp.LineRenderer = series1LineRenderer;

            WPFGraphPointRenderers.RoundPoint series2PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series2PointRenderer.PointBrush = Brushes.Blue;
            series2PointRenderer.PointSize = 5;
            seriesAmbTemp.PointRenderer = series2PointRenderer;
            WPFGraphLineRenderers.DashedLine series2LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series2LineRenderer.LineBrush = Brushes.LightBlue;
            seriesAmbTemp.LineRenderer = series2LineRenderer;

            WPFGraphPointRenderers.RoundPoint series3PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series3PointRenderer.PointBrush = Brushes.Black;
            series3PointRenderer.PointSize = 5;
            seriesTempRise.PointRenderer = series3PointRenderer;
            WPFGraphLineRenderers.DashedLine series3LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series3LineRenderer.LineBrush = Brushes.LightGray;
            seriesTempRise.LineRenderer = series3LineRenderer;
        }

        private void dcCoolingGraphsInit()
        {
        }

        private void acGraphRefresh()
        {
            double maxY = graph.MaxYRange;
            double minY = graph.MinYRange;
            double maxX = 0;
            double minX = 1000;

            seriesOilTemp.Points.Clear();
            foreach (EntityLayer.TempMeasurenment t in datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = t.T1;
                point.Y = t.T1 - t.T1Ref;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                seriesOilTemp.Points.Add(point);
            }

            seriesAmbTemp.Points.Clear();
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
                seriesAmbTemp.Points.Add(point);
            }

            seriesTempRise.Points.Clear();
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
                seriesTempRise.Points.Add(point);
            }

            graph.MaxYRange = maxY;
            graph.MinYRange = minY;
            graph.MaxXRange = maxX;
            graph.MinXRange = minX;
            graph.IntervalYRange = (maxY - minY) / 20;
            graph.IntervalXRange = (graph.MaxXRange - graph.MinXRange) / 20;

            graph.Refresh();
        }
    }*/
}
