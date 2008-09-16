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
using System.IO;
using System.Reflection;

namespace DNBSoft.WPF.WPFGraph
{
    /// <summary>
    /// Interaction logic for WPFSeriesConfigurationWindow.xaml
    /// </summary>
    public partial class WPFSeriesConfigurationWindow : Window
    {
        public delegate void UpdatePreviewDelegate();
        private List<PointAssemblyPair> pointRenderers = new List<PointAssemblyPair>();
        private List<LineAssemblyPair> lineRenderers = new List<LineAssemblyPair>();
        private IWPFGraph graph = null;
        public delegate void RefreshDelegate();

        public WPFSeriesConfigurationWindow(IWPFGraph graph)
        {
            InitializeComponent();
            this.graph = graph;
            if (graph == null)
            {
                throw new WPFGraphExceptions.ParameterNullException();
            }

            #region load all point/line interfaces
            String[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.dll");

            foreach (String filename in files)
            {
                Console.WriteLine("Loading " + filename);
                try
                {
                    Assembly assembly = Assembly.LoadFrom(filename);

                    foreach (Type t in assembly.GetTypes())
                    {
                        if (t.IsClass && !t.IsAbstract && t.GetInterface("DNBSoft.WPF.WPFGraph.IWPFGraphPointRenderer") != null)
                        {
                            Console.WriteLine("Found class : " + t.FullName.ToString());
                            PointAssemblyPair pap = new PointAssemblyPair(t);
                            pointRenderers.Add(pap);
                            ComboBoxItem bi = new ComboBoxItem();
                            bi.Content = pap.getDefaultCanvas();
                            bi.Tag = pap;
                            pointComboBox.Items.Add(bi);
                        }


                        if (t.IsClass && !t.IsAbstract && t.GetInterface("DNBSoft.WPF.WPFGraph.IWPFGraphLineRenderer") != null)
                        {
                            Console.WriteLine("Found class : " + t.FullName.ToString());
                            LineAssemblyPair pap = new LineAssemblyPair(t);
                            lineRenderers.Add(pap);
                            ComboBoxItem bi = new ComboBoxItem();
                            bi.Content = pap.getDefaultCanvas();
                            bi.Tag = pap;
                            lineComboBox.Items.Add(bi);
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Loading fail");
                }
                finally
                {
                    Console.WriteLine("Loading complete");
                }
            }
            #endregion
        }

        public new void ShowDialog()
        {
            #region reload series names
            seriesComboBox.Items.Clear();

            for (int i = 0; i < graph.Series.Count; i++)
            {
                String name = null;
                if (graph.Series[i].Name == null)
                {
                    name = "Series " + (i + 1).ToString();                    
                }
                else
                {
                    name = graph.Series[i].Name;
                }

                ComboBoxItem bi = new ComboBoxItem();
                bi.Tag = graph.Series[i];
                bi.Content = name;
                seriesComboBox.Items.Add(bi);
            }
            #endregion

            base.ShowDialog();

            if (seriesComboBox.Items.Count > 0)
            {
                seriesComboBox.SelectedIndex = 1;
            }
        }

        private class PointAssemblyPair
        {
            private Type assemblyType = null;

            public PointAssemblyPair(Type assemblyType)
            {
                if (!(assemblyType.IsClass && 
                    !assemblyType.IsAbstract && 
                    assemblyType.GetInterface("DNBSoft.WPF.WPFGraph.IWPFGraphPointRenderer") != null)
                    )
                {
                    throw new Exception("Not a IWPFGraphPointRenderer");
                }
                this.assemblyType = assemblyType;
            }

            public IWPFGraphPointRenderer getInstance()
            {
                return (IWPFGraphPointRenderer)Activator.CreateInstance(assemblyType);
            }

            public Canvas getDefaultCanvas()
            {
                Canvas c = new Canvas();
                c.Width = 30;
                c.Height = 30;

                IWPFGraphPointRenderer pr = getInstance();
                WPFRenderParameters rp = new WPFRenderParameters(c, 0, 0, 1.0, 1.0, 30);
                pr.render(rp, new WPFGraphDataPoint() { X = 15, Y = 15 });

                return c;
            }
        }

        private class LineAssemblyPair
        {
            private Type assemblyType = null;

            public LineAssemblyPair(Type assemblyType)
            {
                if (!(assemblyType.IsClass &&
                    !assemblyType.IsAbstract &&
                    assemblyType.GetInterface("DNBSoft.WPF.WPFGraph.IWPFGraphLineRenderer") != null)
                    )
                {
                    throw new Exception("Not a IWPFGraphLineRenderer");
                }
                this.assemblyType = assemblyType;
            }

            public IWPFGraphLineRenderer getInstance()
            {
                return (IWPFGraphLineRenderer)Activator.CreateInstance(assemblyType);
            }

            public Canvas getDefaultCanvas()
            {
                Canvas c = new Canvas();
                c.Width = 30;
                c.Height = 30;

                IWPFGraphLineRenderer pr = getInstance();
                WPFRenderParameters rp = new WPFRenderParameters(c, 0, 0, 1.0, 1.0, 30);
                pr.render(rp, new WPFGraphDataPoint() { X = 2, Y = 2 }, new WPFGraphDataPoint() { X = 28, Y = 28 });

                return c;
            }
        }

        private void seriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (seriesComboBox.SelectedItem != null)
            {
                ComboBoxItem bi = (ComboBoxItem)seriesComboBox.SelectedItem;
                WPFGraphSeries s = (WPFGraphSeries)bi.Tag;

                //Set new preview
                pointPreviewCanvas.Children.Clear();
                WPFRenderParameters rp = new WPFRenderParameters(pointPreviewCanvas, 0, 0, 1.0, 1.0, pointPreviewCanvas.ActualHeight);
                s.PointRenderer.render(rp, new WPFGraphDataPoint() { X = pointPreviewCanvas.ActualWidth / 2.0, Y = pointPreviewCanvas.ActualHeight / 2.0 });

                linePreviewCanvas.Children.Clear();
                rp = new WPFRenderParameters(linePreviewCanvas, 0, 0, 1.0, 1.0, pointPreviewCanvas.ActualHeight);
                s.LineRenderer.render(rp, new WPFGraphDataPoint() { X = 2, Y = 2 }, new WPFGraphDataPoint() { X = 48, Y = 48 });

                updateRendererParameters();
            }
        }

        private void pointComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pointComboBox.SelectedItem != null && seriesComboBox.SelectedItem != null)
            {
                ComboBoxItem pointBoxItem = (ComboBoxItem)pointComboBox.SelectedItem;
                PointAssemblyPair pap = (PointAssemblyPair)pointBoxItem.Tag;

                ComboBoxItem seriesBoxItem = (ComboBoxItem)seriesComboBox.SelectedItem;
                WPFGraphSeries series = (WPFGraphSeries)seriesBoxItem.Tag;
                
                //set new renderer
                series.PointRenderer = pap.getInstance();

                //Set new preview
                pointPreviewCanvas.Children.Clear();
                WPFRenderParameters rp = new WPFRenderParameters(pointPreviewCanvas, 0, 0, 1.0, 1.0, pointPreviewCanvas.ActualHeight);
                series.PointRenderer.render(rp, new WPFGraphDataPoint() { X = pointPreviewCanvas.ActualWidth / 2.0, Y = pointPreviewCanvas.ActualHeight / 2.0 });

                updateRendererParameters();
            }
            else
            {
                //try to undo
                if (e.RemovedItems.Count > 0)
                {
                    pointComboBox.SelectedItem = e.RemovedItems[0];
                }
            }
        }

        private void updateRendererParameters()
        {
            if (seriesComboBox.SelectedItem != null)
            {
                pointProperties.Children.Clear();
                lineProperties.Children.Clear();

                ComboBoxItem bi = (ComboBoxItem)seriesComboBox.SelectedItem;
                WPFGraphSeries s = (WPFGraphSeries)bi.Tag;

                List<WPFGraphConfigurationParameter> parameters = s.PointRenderer.getConfigurationParameters();
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i].Type == (0.0).GetType())
                    {
                        WPFGraphConfigurationDoubleProperty dp =
                            new WPFGraphConfigurationDoubleProperty(graph, s.PointRenderer, parameters[i].Parameter, new UpdatePreviewDelegate(updatePreview));

                        pointProperties.Children.Add(dp);
                    }
                    else if (parameters[i].Type == new SolidColorBrush().GetType())
                    {
                        WPFGraphConfigurationBrushProperty bp =
                            new WPFGraphConfigurationBrushProperty(graph, s.PointRenderer, parameters[i].Parameter, new UpdatePreviewDelegate(updatePreview));

                        pointProperties.Children.Add(bp);
                    }
                }

                parameters = s.LineRenderer.getConfigurationParameters();
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i].Type == (0.0).GetType())
                    {
                        WPFGraphConfigurationDoubleProperty dp =
                            new WPFGraphConfigurationDoubleProperty(graph, s.LineRenderer, parameters[i].Parameter, new UpdatePreviewDelegate(updatePreview));

                        lineProperties.Children.Add(dp);
                    }
                    else if (parameters[i].Type == new SolidColorBrush().GetType())
                    {
                        WPFGraphConfigurationBrushProperty bp =
                            new WPFGraphConfigurationBrushProperty(graph, s.LineRenderer, parameters[i].Parameter, new UpdatePreviewDelegate(updatePreview));

                        lineProperties.Children.Add(bp);
                    }
                }
            }
        }

        private void updatePreview()
        {
            if (pointComboBox.SelectedItem != null && seriesComboBox.SelectedItem != null)
            {
                ComboBoxItem pointBoxItem = (ComboBoxItem)pointComboBox.SelectedItem;
                PointAssemblyPair pap = (PointAssemblyPair)pointBoxItem.Tag;

                ComboBoxItem seriesBoxItem = (ComboBoxItem)seriesComboBox.SelectedItem;
                WPFGraphSeries series = (WPFGraphSeries)seriesBoxItem.Tag;

                pointPreviewCanvas.Children.Clear();
                WPFRenderParameters rp = new WPFRenderParameters(pointPreviewCanvas, 0, 0, 1.0, 1.0, pointPreviewCanvas.ActualHeight);
                series.PointRenderer.render(rp, new WPFGraphDataPoint() { X = pointPreviewCanvas.ActualWidth / 2.0, Y = pointPreviewCanvas.ActualHeight / 2.0 });
            }
        }

        private void lineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lineComboBox.SelectedItem != null && seriesComboBox.SelectedItem != null)
            {
                ComboBoxItem lineBoxItem = (ComboBoxItem)lineComboBox.SelectedItem;
                LineAssemblyPair pap = (LineAssemblyPair)lineBoxItem.Tag;

                ComboBoxItem seriesBoxItem = (ComboBoxItem)seriesComboBox.SelectedItem;
                WPFGraphSeries series = (WPFGraphSeries)seriesBoxItem.Tag;

                //set new renderer
                series.LineRenderer = pap.getInstance();

                //Set new preview
                linePreviewCanvas.Children.Clear();
                WPFRenderParameters rp = new WPFRenderParameters(linePreviewCanvas, 0, 0, 1.0, 1.0, pointPreviewCanvas.ActualHeight);
                series.LineRenderer.render(rp, new WPFGraphDataPoint() { X = 2, Y = 2 }, new WPFGraphDataPoint() { X = 48, Y = 48 });

                updateRendererParameters();
            }
            else
            {
                //try to undo
                if (e.RemovedItems.Count > 0)
                {
                    lineComboBox.SelectedItem = e.RemovedItems[0];
                }
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
