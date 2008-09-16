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
using DNBSoft.Generics;

namespace DNBSoft.WPF.WPFGraph
{
    public partial class WPFScatterGraph : UserControl, IWPFGraph
    {
        private double minX = 0;
        private double maxX = 100;
        private double intervalX = 10;
        private double minY = 0;
        private double maxY = 100;
        private double intervalY = 10;

        private Brush axisTitleTickBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private double axisTitleTickThickness = 1;

        private ListenableList<WPFGraphSeries> series = new ListenableList<WPFGraphSeries>();

        public WPFScatterGraph()
        {
            InitializeComponent();

            masterGrid.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            ContextMenu contextMenu = new ContextMenu();
            MenuItem refreshContextButton = new MenuItem();
            refreshContextButton.Header = "Refresh";
            refreshContextButton.Click += new RoutedEventHandler(refreshContextButton_Click);
            MenuItem configureContextButton = new MenuItem();
            configureContextButton.Header = "Format Data Series...";
            configureContextButton.Click += new RoutedEventHandler(configureContextButton_Click);

            contextMenu.Items.Add(configureContextButton);
            contextMenu.Items.Add(refreshContextButton);
            this.ContextMenu = contextMenu;

            updateXKey();
            updateYKey();
        }

        private void configureContextButton_Click(object sender, RoutedEventArgs e)
        {
            new WPFSeriesConfigurationWindow(this).ShowDialog();
        }

        private void refreshContextButton_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.XAxisTitle = "X Axis";
            this.YAxisTitle = "Y Axis";

            series.ElementAdded += new ListenableList<WPFGraphSeries>.ElementAddedDelegate(series_ElementAdded);
            series.ElementRemoved += new ListenableList<WPFGraphSeries>.ElementRemovedDelegate(series_ElementRemoved);

            this.SizeChanged += new SizeChangedEventHandler(WPFScatterGraph_SizeChanged);
            WPFScatterGraph_SizeChanged(null, null);

            //this.Refresh();
        }

        private void WPFScatterGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry g = new RectangleGeometry(new Rect(0,0,graphCanvas.ActualWidth, graphCanvas.ActualHeight));
            graphCanvas.Clip = g;

            this.Refresh();

            updateXKey();
            updateYKey();
        }

        public void Refresh()
        {
            try
            {
                graphCanvas.Children.Clear();

                WPFRenderParameters parameters = new WPFRenderParameters(graphCanvas,
                    minX, minY, 
                    1.0 / (maxX - minX) * graphCanvas.ActualWidth, 1.0 / (maxY - minY) * graphCanvas.ActualHeight,
                    graphCanvas.ActualHeight);


                for (int i = 0; i < series.Count; i++)
                {
                    series[i].render(parameters);
                }

                updateXKey();
            }
            catch (Exception err)
            {
                Console.WriteLine("Render ERROR: " + err.ToString());
            }
        }


        #region axis accessors
        public Brush AxisBrush
        {
            get
            {
                return verticalAxisBorder.BorderBrush.Clone();
            }
            set
            {
                verticalAxisBorder.BorderBrush = value;
                cornerAxisBorder.BorderBrush = value;
                horizontalAxisBorder.BorderBrush = value;
            }
        }

        public double AxisThickness
        {
            get
            {
                return verticalAxisBorder.BorderThickness.Right;
            }
            set
            {
                verticalAxisBorder.BorderThickness = new Thickness(0, 0, value, 0);
                cornerAxisBorder.BorderThickness = new Thickness(0, value, value, 0);
                horizontalAxisBorder.BorderThickness = new Thickness(0, value, 0, 0);
            }
        }
        public Brush AxisVerticalBackground
        {
            get
            {
                return verticalAxisBorder.Background;
            }
            set
            {
                verticalAxisBorder.Background = value;
            }
        }

        public Brush GraphBackground
        {
            get
            {
                return horizontalAxisBorder.Background;
            }
            set
            {
                horizontalAxisBorder.Background = value;
                verticalAxisBorder.Background = value;
                cornerAxisBorder.Background = value;
                topBorder.Background = value;
                sideBorder.Background = value;
            }
        }

        public GridLength AxisVerticalWidth
        {
            get
            {
                return masterGrid.ColumnDefinitions[0].Width;
            }
            set
            {
                masterGrid.ColumnDefinitions[0].Width = value;
            }
        }

        public GridLength AxisHorizontalHeight
        {
            get
            {
                return masterGrid.RowDefinitions[1].Height;
            }
            set
            {
                masterGrid.RowDefinitions[1].Height = value;
            }
        }
        #endregion

        #region axis Key accessors
        public double AxisFontSize
        {
            get
            {
                return horizontalTitle.FontSize;
            }
            set
            {
                horizontalTitle.FontSize = value;
                verticalTitle.FontSize = value;
            }
        }

        public Brush AxisTitleTickBrush
        {
            get
            {
                return axisTitleTickBrush;
            }
            set
            {
                if (axisTitleTickBrush != value)
                {
                    Brush oldValue = axisTitleTickBrush;
                    axisTitleTickBrush = value;
                }
            }
        }

        public double AxisTitleTickThickness
        {
            get
            {
                return axisTitleTickThickness;
            }
            set
            {
                if (axisTitleTickThickness != value)
                {
                    double oldValue = axisTitleTickThickness;
                    axisTitleTickThickness = value;
                }
            }
        }

        #region x axis
        public double MinXRange
        {
            get
            {
                return minX;
            }
            set
            {
                if (minX != value)
                {
                    minX = value;
                    updateXKey();
                }
            }
        }

        public double MaxXRange
        {
            get
            {
                return maxX;
            }
            set
            {
                if (maxX != value)
                {
                    maxX = value;
                    updateXKey();
                }
            }
        }

        public double IntervalXRange
        {
            get
            {
                return intervalX;
            }
            set
            {
                if (intervalX != value)
                {
                    intervalX = value;
                    updateXKey();
                }
            }
        }

        public String XAxisTitle
        {
            get
            {
                return horizontalTitle.Content.ToString();
            }
            set
            {
                horizontalTitle.Content = value;
            }
        }

        private void updateXKey()
        {
            horizontalAxis.Children.Clear();

            WPFRenderParameters parameters = new WPFRenderParameters(graphCanvas,
                    minX, minY,
                    1.0 / (maxX - minX) * graphCanvas.ActualWidth, 1.0 / (maxY - minY) * graphCanvas.ActualHeight,
                    graphCanvas.ActualHeight);

            for (double d = minX + intervalX; d < maxX; d += intervalX)
            {
                Label label = new Label();
                label.Content = d.ToString();
                label.SetValue(Canvas.TopProperty, 8.0);
                horizontalAxis.Children.Add(label);
                label.UpdateLayout();
                if (label.ActualWidth > 0)
                {
                    label.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, d) - (label.ActualWidth / 2.0));
                }
                else
                {
                    label.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, d));
                }
            }

            for (double d = minX + intervalX; d <= maxX; d += intervalX)
            {
                Line line = new Line();
                line.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, d);
                line.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, d);
                line.Y1 = 0;
                line.Y2 = 10;
                line.Stroke = this.AxisTitleTickBrush;
                line.StrokeThickness = this.AxisTitleTickThickness;
                horizontalAxis.Children.Add(line);
            }
        }
        #endregion

        #region y axis
        public double MinYRange
        {
            get
            {
                return minY;
            }
            set
            {
                if (minY != value)
                {
                    minY = value;
                    updateYKey();
                }
            }
        }

        public double MaxYRange
        {
            get
            {
                return maxY;
            }
            set
            {
                if (maxY != value)
                {
                    maxY = value;
                    updateYKey();
                }
            }
        }

        public double IntervalYRange
        {
            get
            {
                return intervalY;
            }
            set
            {
                if (intervalY != value)
                {
                    intervalY = value;
                    updateYKey();
                }
            }
        }

        public String YAxisTitle
        {
            get
            {
                return verticalTitle.Content.ToString();
            }
            set
            {
                verticalTitle.Content = value;
            }
        }

        private void updateYKey()
        {
            verticalAxis.Children.Clear();

            WPFRenderParameters parameters = new WPFRenderParameters(graphCanvas,
                    minX, minY,
                    1.0 / (maxX - minX) * graphCanvas.ActualWidth, 1.0 / (maxY - minY) * graphCanvas.ActualHeight,
                    graphCanvas.ActualHeight);

            for (double d = minY + intervalY; d < maxY; d += intervalY)
            {
                Label label = new Label();
                label.Content = Math.Round(d,1).ToString();
                verticalAxis.Children.Add(label);
                label.UpdateLayout();
                label.SetValue(Canvas.LeftProperty, verticalAxis.ActualWidth - (10 + label.ActualWidth));
                if (label.ActualWidth > 0)
                {
                    label.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, d) - (label.ActualHeight / 2.0));
                }
                else
                {
                    label.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, d));
                }
            }

            for (double d = minY + intervalY; d <= maxY; d += intervalY)
            {
                Line line = new Line();
                line.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, d);
                line.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, d);
                line.X1 = verticalAxis.ActualWidth;
                line.X2 = verticalAxis.ActualWidth - 10;
                line.Stroke = this.AxisTitleTickBrush;
                line.StrokeThickness = this.AxisTitleTickThickness;
                verticalAxis.Children.Add(line);
            }
        }
        #endregion
        #endregion

        #region series
        public ListenableList<WPFGraphSeries> Series
        {
            get
            {
                return series;
            }
        }

        private void series_ElementRemoved(ListenableList<WPFGraphSeries> sender, ListenableList<WPFGraphSeries>.ElementRemovedEventArgs<WPFGraphSeries> args)
        {
            throw new NotImplementedException();
        }

        private void series_ElementAdded(ListenableList<WPFGraphSeries> sender, ListenableList<WPFGraphSeries>.ElementAddedEventArgs<WPFGraphSeries> args)
        {
            WPFRenderParameters parameters = new WPFRenderParameters(graphCanvas,
                    minX, minY,
                    1.0 / (maxX - minX) * graphCanvas.ActualWidth, 1.0 / (maxY - minY) * graphCanvas.ActualHeight,
                    graphCanvas.ActualHeight);

            series[args.Index].render(parameters);

            series[args.Index].DataSeriesChanged += new WPFGraphDelegates.DataSeriesChangedEventDelegate(WPFScatterGraph_DataSeriesChanged);
            series[args.Index].DataSeriesRendererChanged += new WPFGraphDelegates.RendererChangedEventDelegate(WPFScatterGraph_DataSeriesRendererChanged);
        }

        private void WPFScatterGraph_DataSeriesRendererChanged(object sender, WPFGraphEventClasses.RendererChangedEventArgs args)
        {
            this.Refresh();
        }

        private void WPFScatterGraph_DataSeriesChanged(object sender, WPFGraphEventClasses.DataSeriesChangedEventArgs args)
        {
            this.Refresh();
        }
        #endregion
    }
}
