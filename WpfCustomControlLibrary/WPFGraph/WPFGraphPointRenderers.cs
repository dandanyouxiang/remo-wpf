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
    public abstract class WPFGraphPointRenderers
    {
        public class RoundPoint : BasePoint, IWPFGraphPointRenderer
        {
            #region IWPFGraphPointRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p)
            {
                Ellipse e = new Ellipse();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.PointBrush;
                e.Fill = this.PointBrush;
                e.SetValue(Canvas.ZIndexProperty, 10);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0));
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0));
                parameters.Canvas.Children.Add(e);
            }

            public new void setValue(string parameter, object value)
            {
                base.setValue(parameter, value);
            }

            public new object getValue(string parameter)
            {
                return base.getValue(parameter);
            }

            public new List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                return base.getConfigurationParameters();
            }

            #endregion
        }
        public class SqaurePoint : BasePoint, IWPFGraphPointRenderer
        {
            #region IWPFGraphPointRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p)
            {
                Rectangle e = new Rectangle();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.PointBrush;
                e.Fill = this.PointBrush;
                e.SetValue(Canvas.ZIndexProperty, 10);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0));
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0));
                parameters.Canvas.Children.Add(e);
            }

            public new void setValue(string parameter, object value)
            {
                base.setValue(parameter, value);
            }

            public new object getValue(string parameter)
            {
                return base.getValue(parameter);
            }

            public new List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                return base.getConfigurationParameters();
            }

            #endregion
        }

        public class ShadowRoundPoint : BasePoint, IWPFGraphPointRenderer
        {
            private double offsetX = 3.0;
            private double offsetY = 3.0;
            private Brush shadowBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            public double ShadowOffsetX
            {
                get
                {
                    return offsetX;
                }
                set
                {
                    if (offsetX != value)
                    {
                        double oldValue = offsetX;
                        offsetX = value;
                        fireEvent("ShadowOffsetX", oldValue, value);
                    }
                }
            }

            public double ShadowOffsetY
            {
                get
                {
                    return offsetY;
                }
                set
                {
                    if (offsetY != value)
                    {
                        double oldValue = offsetY;
                        offsetY = value;
                        fireEvent("ShadowOffsetY", oldValue, value);
                    }
                }
            }

            public Brush ShadowBrush
            {
                get
                {
                    return shadowBrush;
                }
                set
                {
                    if (shadowBrush != value)
                    {
                        Brush oldValue = shadowBrush;
                        shadowBrush = value;
                        fireEvent("ShadowBrush", oldValue, value);
                    }
                }
            }

            #region IWPFGraphPointRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p)
            {
                #region point
                Ellipse e = new Ellipse();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.PointBrush;
                e.Fill = this.PointBrush;
                e.SetValue(Canvas.ZIndexProperty, 10);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0));
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0));
                parameters.Canvas.Children.Add(e);
                #endregion

                #region shadow
                e = new Ellipse();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.ShadowBrush;
                e.Fill = this.ShadowBrush;
                e.SetValue(Canvas.ZIndexProperty, 1);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0) + offsetX);
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0) + offsetY);
                parameters.Canvas.Children.Add(e);
                #endregion
            }

            public new void setValue(string parameter, object value)
            {
                if (parameter.Equals("ShadowOffsetX"))
                {
                    try
                    {
                        this.ShadowOffsetX = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("ShadowOffsetY"))
                {
                    try
                    {
                        this.ShadowOffsetY = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("ShadowBrush"))
                {
                    try
                    {
                        this.ShadowBrush = (Brush)value;
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else
                {
                    base.setValue(parameter, value);
                }
            }

            public object getValue(string parameter)
            {
                if (parameter.Equals("ShadowOffsetX"))
                {
                    return this.ShadowOffsetX;
                }
                else if (parameter.Equals("ShadowOffsetY"))
                {
                    return this.ShadowOffsetY;
                }
                else if (parameter.Equals("ShadowBrush"))
                {
                    return shadowBrush;
                }
                else
                {
                    return base.getValue(parameter);
                }
            }

            public new List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                List<WPFGraphConfigurationParameter> l = base.getConfigurationParameters();
                l.Add(new WPFGraphConfigurationParameter("ShadowOffsetX", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("ShadowOffsetY", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("ShadowBrush", shadowBrush.GetType()));
                return l;
            }

            #endregion
        }

        public class ShadowSquarePoint : BasePoint, IWPFGraphPointRenderer
        {
            private double offsetX = 3.0;
            private double offsetY = 3.0;
            private Brush shadowBrush = new SolidColorBrush(Color.FromRgb(128, 128, 128));

            public double ShadowOffsetX
            {
                get
                {
                    return offsetX;
                }
                set
                {
                    if (offsetX != value)
                    {
                        double oldValue = offsetX;
                        offsetX = value;
                        fireEvent("ShadowOffsetX", oldValue, value);
                    }
                }
            }

            public double ShadowOffsetY
            {
                get
                {
                    return offsetY;
                }
                set
                {
                    if (offsetY != value)
                    {
                        double oldValue = offsetY;
                        offsetY = value;
                        fireEvent("ShadowOffsetY", oldValue, value);
                    }
                }
            }

            public Brush ShadowBrush
            {
                get
                {
                    return shadowBrush;
                }
                set
                {
                    if (shadowBrush != value)
                    {
                        Brush oldValue = shadowBrush;
                        shadowBrush = value;
                        fireEvent("ShadowBrush", oldValue, value);
                    }
                }
            }

            #region IWPFGraphPointRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p)
            {
                #region point
                Rectangle e = new Rectangle();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.PointBrush;
                e.Fill = this.PointBrush;
                e.SetValue(Canvas.ZIndexProperty, 10);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0));
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0));
                parameters.Canvas.Children.Add(e);
                #endregion

                #region shadow
                e = new Rectangle();
                e.Width = this.PointSize;
                e.Height = this.PointSize;
                e.Stroke = this.ShadowBrush;
                e.Fill = this.ShadowBrush;
                e.SetValue(Canvas.ZIndexProperty, 1);
                e.SetValue(Canvas.LeftProperty, parameters.transpose(WPFGraphEnumerations.Axis.X, p.X) - (this.PointSize / 2.0) + offsetX);
                e.SetValue(Canvas.TopProperty, parameters.transpose(WPFGraphEnumerations.Axis.Y, p.Y) - (this.PointSize / 2.0) + offsetY);
                parameters.Canvas.Children.Add(e);
                #endregion
            }

            public new void setValue(string parameter, object value)
            {
                if (parameter.Equals("ShadowOffsetX"))
                {
                    try
                    {
                        this.ShadowOffsetX = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("ShadowOffsetY"))
                {
                    try
                    {
                        this.ShadowOffsetY = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("ShadowBrush"))
                {
                    try
                    {
                        this.ShadowBrush = (Brush)value;
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else
                {
                    base.setValue(parameter, value);
                }
            }

            public object getValue(string parameter)
            {
                if (parameter.Equals("ShadowOffsetX"))
                {
                    return this.ShadowOffsetX;
                }
                else if (parameter.Equals("ShadowOffsetY"))
                {
                    return this.ShadowOffsetY;
                }
                else if (parameter.Equals("ShadowBrush"))
                {
                    return shadowBrush;
                }
                else
                {
                    return base.getValue(parameter);
                }
            }

            public new List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                List<WPFGraphConfigurationParameter> l = base.getConfigurationParameters();
                l.Add(new WPFGraphConfigurationParameter("ShadowOffsetX", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("ShadowOffsetY", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("ShadowBrush", shadowBrush.GetType()));
                return l;
            }

            #endregion
        }

        public abstract class BasePoint
        {
            public double size = 3;
            public Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            public event WPFGraphDelegates.RendererChangedEventDelegate RendererChanged;

            public double PointSize
            {
                get
                {
                    return size;
                }
                set
                {
                    if (size != value)
                    {
                        double oldValue = size;
                        size = value;
                        fireEvent("LineThickness", oldValue, value);
                    }
                }
            }

            public Brush PointBrush
            {
                get
                {
                    return brush;
                }
                set
                {
                    if (brush != value)
                    {
                        Brush oldValue = brush;
                        brush = value;
                        fireEvent("PointBrush", oldValue, value);
                    }
                }
            }

            protected void fireEvent(String property, object oldValue, object newValue)
            {
                if (RendererChanged != null)
                {
                    RendererChanged(this,
                        new WPFGraphEventClasses.RendererChangedEventArgs(property, oldValue, newValue));
                }
            }

            public void setValue(string parameter, object value)
            {
                if (parameter.Equals("PointSize"))
                {
                    try
                    {
                        this.PointSize = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("PointBrush"))
                {
                    try
                    {
                        this.PointBrush = (Brush)value;
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else
                {
                    throw new WPFGraphExceptions.PropertyNotFoundException();
                }
            }

            public object getValue(String parameter)
            {
                if (parameter.Equals("PointSize"))
                {
                    return this.PointSize;
                }
                else if (parameter.Equals("PointBrush"))
                {
                    return this.PointBrush;
                }
                else
                {
                    throw new WPFGraphExceptions.PropertyNotFoundException();
                }
            }

            public List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                List<WPFGraphConfigurationParameter> l = new List<WPFGraphConfigurationParameter>();
                l.Add(new WPFGraphConfigurationParameter("PointSize", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("PointBrush", PointBrush.GetType()));
                return l;
            }
        }
    }
}
