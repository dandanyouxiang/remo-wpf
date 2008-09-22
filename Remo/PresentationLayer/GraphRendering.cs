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
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        private void graphsInit()
        {
            acGraphInit();
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

            AcGraph.Series.Add(seriesOilTemp);
            AcGraph.Series.Add(seriesAmbTemp);
            AcGraph.Series.Add(seriesTempRise);

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
            double maxY = -1000;
            double minY = 1000;
            double maxX = -1000;
            double minX = (long)DateTime.Now.Ticks;

            seriesOilTemp.Points.Clear();
            foreach (EntityLayer.TempMeasurenment t in datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments)
            {
                EntityLayer.TempMeasurenementConfiguration tempConfig = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
                int numberOfOilTemps = (tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? 1 : 0) + (tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? 1 : 0) + (tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? 1 : 0) + (tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? 1 : 0);
                double oilTemps = (tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? t.T1 : 0) + (tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? t.T2 : 0) + (tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? t.T3 : 0) + (tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? t.T4 : 0);
                double meanOilTemp = oilTemps / numberOfOilTemps;
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = (double)t.Time.Ticks;
                point.Y = meanOilTemp;
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
            foreach (EntityLayer.TempMeasurenment t in datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments)
            {
                EntityLayer.TempMeasurenementConfiguration tempConfig = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
                int numberOfOilTemps = (!tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? 1 : 0) + (!tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? 1 : 0) + (!tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? 1 : 0) + (!tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? 1 : 0);
                double oilTemps = (!tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? t.T1 : 0) + (!tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? t.T2 : 0) + (!tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? t.T3 : 0) + (!tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? t.T4 : 0);
                double meanOilTemp = oilTemps / numberOfOilTemps;
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = (double)t.Time.Ticks;
                point.Y = meanOilTemp;
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
            for (int i = 0; i < seriesAmbTemp.Points.Count; i++)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = seriesAmbTemp.Points[i].X;
                point.Y = seriesOilTemp.Points[i].Y - seriesAmbTemp.Points[i].Y;
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

            AcGraph.MaxYRange = maxY;
            AcGraph.MinYRange = minY;
            AcGraph.MaxXRange = maxX;
            AcGraph.MinXRange = minX;
            AcGraph.IntervalYRange = (maxY - minY) / 20;
            AcGraph.IntervalXRange = (AcGraph.MaxXRange - AcGraph.MinXRange) / 10;

            AcGraph.Refresh();
            this.InvalidateVisual();
        }
    }
}
