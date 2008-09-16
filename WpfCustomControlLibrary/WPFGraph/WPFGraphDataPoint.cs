using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNBSoft.WPF.WPFGraph
{
    public class WPFGraphDataPoint
    {
        #region class variables
        private double x = 0;
        private double y = 0;
        private IWPFGraphPointRenderer pointRenderer = null;
        private IWPFGraphLineRenderer lineRenderer = null;
        #endregion

        #region events
        public event WPFGraphDelegates.DataPointChangedEventDelegate DataPointChanged;
        public event WPFGraphDelegates.RendererChangedEventDelegate DataPointRendererChanged;
        #endregion

        #region assesors
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x != value)
                {
                    double oldValue = x;
                    x = value;
                    fireEvent(WPFGraphEnumerations.Axis.X, oldValue, value);
                }
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y != value)
                {
                    double oldValue = y;
                    y = value;
                    fireEvent(WPFGraphEnumerations.Axis.Y, oldValue, value);
                }
            }
        }

        public IWPFGraphPointRenderer PointRenderer
        {
            get
            {
                return pointRenderer;
            }
            set
            {
                if (pointRenderer != null)
                {
                    pointRenderer.RendererChanged -= new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                }
                IWPFGraphPointRenderer oldValue = pointRenderer;
                pointRenderer = value;
                if (pointRenderer != null)
                {
                    pointRenderer.RendererChanged += new WPFGraphDelegates.RendererChangedEventDelegate(pointRenderer_RendererChanged);
                }
                fireEvent(new WPFGraphEventClasses.RendererChangedEventArgs("NEW", oldValue, value));
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
        #endregion

        #region event handlers
        private void pointRenderer_RendererChanged(object sender, WPFGraphEventClasses.RendererChangedEventArgs args)
        {
            fireEvent(args);
        }

        private void fireEvent(WPFGraphEnumerations.Axis axis, double oldValue, double newValue)
        {
            if (DataPointChanged != null)
            {
                DataPointChanged(this, new WPFGraphEventClasses.DataPointChangedEventArgs(axis, oldValue, newValue));
            }
        }

        private void fireEvent(WPFGraphEventClasses.RendererChangedEventArgs args)
        {
            if (DataPointRendererChanged != null)
            {
                DataPointRendererChanged(this, args);
            }
        }
        #endregion

        public override string ToString()
        {
            return x + " : " + y;
        }
    }
}
