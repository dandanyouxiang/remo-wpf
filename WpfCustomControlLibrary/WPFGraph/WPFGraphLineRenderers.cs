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
    public abstract class WPFGraphLineRenderers
    {
        public class NormalLine : BaseLine, IWPFGraphLineRenderer
        {
            #region IWPFGraphLineRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p1, WPFGraphDataPoint p2)
            {
                Line l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X);
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y);
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X);
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y);
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.LineBrush;
                l.SetValue(Canvas.ZIndexProperty, 2);

                parameters.Canvas.Children.Add(l);
            }

            public new void setValue(string parameter, object value)
            {
                base.setValue(parameter, value);
            }

            public new object getValue(string parameter)
            {
                return base.getValue(parameter);
            }

            #endregion
        }
        public class DashedLine : BaseLine, IWPFGraphLineRenderer
        {
            #region IWPFGraphLineRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p1, WPFGraphDataPoint p2)
            {
                Line l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X);
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y);
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X);
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y);
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.LineBrush;
                DoubleCollection dc = new DoubleCollection();
                dc.Add(5);
                l.StrokeDashArray = dc;
                
                l.SetValue(Canvas.ZIndexProperty, 2);

                parameters.Canvas.Children.Add(l);
            }

            public new void setValue(string parameter, object value)
            {
                base.setValue(parameter, value);
            }

            public new object getValue(string parameter)
            {
                return base.getValue(parameter);
            }

            #endregion
        }

        public class ShadowLine : BaseLine, IWPFGraphLineRenderer
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

            #region IWPFGraphLineRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p1, WPFGraphDataPoint p2)
            {
                #region line
                Line l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X);
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y);
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X);
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y);
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.LineBrush;
                l.SetValue(Canvas.ZIndexProperty, 2);

                parameters.Canvas.Children.Add(l);
                #endregion

                #region shadow
                l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X) + offsetX;
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y) + offsetY;
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X) + offsetX;
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y) + offsetY;
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.ShadowBrush;
                l.SetValue(Canvas.ZIndexProperty, 1);

                parameters.Canvas.Children.Add(l);
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

            public new object getValue(string parameter)
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

        public class ShadowDashedLine : BaseLine, IWPFGraphLineRenderer
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

            #region IWPFGraphLineRenderer Members

            public void render(WPFRenderParameters parameters, WPFGraphDataPoint p1, WPFGraphDataPoint p2)
            {
                #region line
                Line l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X);
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y);
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X);
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y);
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.LineBrush;
                l.SetValue(Canvas.ZIndexProperty, 2);
                DoubleCollection dc = new DoubleCollection();
                dc.Add(5);
                l.StrokeDashArray = dc;

                parameters.Canvas.Children.Add(l);
                #endregion

                #region shadow
                l = new Line();
                l.X1 = parameters.transpose(WPFGraphEnumerations.Axis.X, p1.X) + offsetX;
                l.Y1 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p1.Y) + offsetY;
                l.X2 = parameters.transpose(WPFGraphEnumerations.Axis.X, p2.X) + offsetX;
                l.Y2 = parameters.transpose(WPFGraphEnumerations.Axis.Y, p2.Y) + offsetY;
                l.StrokeThickness = this.LineThickness;
                l.Stroke = this.ShadowBrush;
                l.SetValue(Canvas.ZIndexProperty, 1);
                dc = new DoubleCollection();
                dc.Add(5);
                l.StrokeDashArray = dc;

                parameters.Canvas.Children.Add(l);
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

            public new object getValue(string parameter)
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

        public abstract class BaseLine
        {
            private Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            private double lineThickness = 1;
            public event WPFGraphDelegates.RendererChangedEventDelegate RendererChanged;

            public Brush LineBrush
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
                        fireEvent("LineBrush", oldValue, value);
                    }
                }
            }

            public double LineThickness
            {
                get
                {
                    return lineThickness;
                }
                set
                {
                    if (lineThickness != value)
                    {
                        double oldValue = lineThickness;
                        lineThickness = value;
                        fireEvent("LineThickness", oldValue, value);
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
                if (parameter.Equals("LineThickness"))
                {
                    try
                    {
                        this.LineThickness = double.Parse(value.ToString());
                    }
                    catch (Exception)
                    {
                        throw new WPFGraphExceptions.ParameterFormatException();
                    }
                }
                else if (parameter.Equals("LineBrush"))
                {
                    try
                    {
                        this.LineBrush = (Brush)value;
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
                if (parameter.Equals("LineThickness"))
                {
                    return this.LineThickness;
                }
                else if (parameter.Equals("LineBrush"))
                {
                    return this.LineBrush;
                }
                else
                {
                    throw new WPFGraphExceptions.PropertyNotFoundException();
                }
            }

            public List<WPFGraphConfigurationParameter> getConfigurationParameters()
            {
                List<WPFGraphConfigurationParameter> l = new List<WPFGraphConfigurationParameter>();
                l.Add(new WPFGraphConfigurationParameter("LineThickness", (0.0).GetType()));
                l.Add(new WPFGraphConfigurationParameter("LineBrush", brush.GetType()));
                return l;
            }
        }
    }
}
