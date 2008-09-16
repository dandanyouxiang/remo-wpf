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
    public class WPFGraphSeries
    {
        #region class variables
        private ListenableList<WPFGraphDataPoint> points = new ListenableList<WPFGraphDataPoint>();
        private IWPFGraphPointRenderer pointRenderer = new WPFGraphPointRenderers.RoundPoint();
        private IWPFGraphLineRenderer lineRenderer = new WPFGraphLineRenderers.NormalLine();
        private String name = null;
        #endregion

        #region events
        public event WPFGraphDelegates.RendererChangedEventDelegate DataSeriesRendererChanged;
        public event WPFGraphDelegates.DataSeriesChangedEventDelegate DataSeriesChanged;
        #endregion

        public WPFGraphSeries()
        {
            points.ElementAdded += new ListenableList<WPFGraphDataPoint>.ElementAddedDelegate(points_ElementAdded);
            points.ElementRemoved += new ListenableList<WPFGraphDataPoint>.ElementRemovedDelegate(points_ElementRemoved);
        }

        internal void render(WPFRenderParameters parameters)
        {
            if (points.Count == 0)
            {

            }
            else if (points.Count == 1)
            {
                #region render point
                if (points[0].PointRenderer == null)
                {
                    pointRenderer.render(parameters, points[0]);
                }
                else
                {
                    points[0].PointRenderer.render(parameters, points[0]);
                }
                #endregion
            }
            else
            {
                #region render line
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (points[i].LineRenderer == null)
                    {
                        lineRenderer.render(parameters, points[i], points[i + 1]);
                    }
                    else
                    {
                        points[i].LineRenderer.render(parameters, points[i], points[i + 1]);
                    }
                }
                #endregion

                #region render point
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].PointRenderer == null)
                    {
                        pointRenderer.render(parameters, points[i]);
                    }
                    else
                    {
                        points[i].PointRenderer.render(parameters, points[i]);
                    }
                }
                #endregion
            }
        }

        #region assesors
        public IWPFGraphPointRenderer PointRenderer
        {
            get
            {
                return pointRenderer;
            }
            set
            {
                if (value == null)
                {
                    throw new Exception("Null value exception");
                }
                else
                {
                    if (pointRenderer != null)
                    {
                        pointRenderer.RendererChanged -= new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                    }
                    IWPFGraphPointRenderer oldValue = pointRenderer;
                    pointRenderer = value;
                    if (pointRenderer != null)
                    {
                        pointRenderer.RendererChanged +=new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                    }
                    fireEvent(new WPFGraphEventClasses.RendererChangedEventArgs("NEW", oldValue, value));
                }
            }
        }

        public IWPFGraphLineRenderer LineRenderer
        {
            get
            {
                return lineRenderer;
            }
            set
            {
                if (value == null)
                {
                    throw new Exception("Null value exception");
                }
                else
                {
                    if (lineRenderer != null)
                    {
                        lineRenderer.RendererChanged -= new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                    }
                    IWPFGraphLineRenderer oldValue = lineRenderer;
                    lineRenderer = value;
                    if (lineRenderer != null)
                    {
                        lineRenderer.RendererChanged -= new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                    }
                    fireEvent(new WPFGraphEventClasses.RendererChangedEventArgs("NEW", oldValue, value));
                }
            }
        }

        public ListenableList<WPFGraphDataPoint> Points
        {
            get
            {
                return points;
            }
        }

        public Range<double> getRange(WPFGraphEnumerations.Axis axis)
        {
            if (points.Count == 0)
            {
                throw new Exception("No points in series");
            }
            else
            {
                Range<double> range = new Range<double>(double.MaxValue, double.MinValue);

                for (int i = 0; i < points.Count; i++)
                {
                    if (axis == WPFGraphEnumerations.Axis.X)
                    {
                        range.From = Math.Min(range.From, points[i].X);
                        range.To = Math.Max(range.To, points[i].X);
                    }
                    else
                    {
                        range.From = Math.Min(range.From, points[i].Y);
                        range.To = Math.Max(range.To, points[i].Y);
                    }
                }

                return range;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion

        #region event handlers
        private void fireEvent(WPFGraphEventClasses.RendererChangedEventArgs args)
        {
            if (DataSeriesRendererChanged != null)
            {
                DataSeriesRendererChanged(this, args);
            }
        }

        private void fireEvent(WPFGraphEventClasses.DataSeriesChangedEventArgs args)
        {
            if (DataSeriesChanged != null)
            {
                DataSeriesChanged(this, args);
            }
        }

        private void pointRenderer_RendererChanged(object sender, WPFGraphEventClasses.RendererChangedEventArgs args)
        {
            fireEvent(args);
        }

        private void points_ElementRemoved(ListenableList<WPFGraphDataPoint> sender, ListenableList<WPFGraphDataPoint>.ElementRemovedEventArgs<WPFGraphDataPoint> args)
        {
            fireEvent(new WPFGraphEventClasses.DataSeriesChangedEventArgs(WPFGraphEnumerations.ListAction.Removed, args.Index, args.Item));
        }

        private void points_ElementAdded(ListenableList<WPFGraphDataPoint> sender, ListenableList<WPFGraphDataPoint>.ElementAddedEventArgs<WPFGraphDataPoint> args)
        {
            fireEvent(new WPFGraphEventClasses.DataSeriesChangedEventArgs(WPFGraphEnumerations.ListAction.Added, args.Index, args.Item));
        }
        #endregion
    }
}
