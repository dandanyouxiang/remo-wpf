using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNBSoft.WPF.WPFGraph
{
    public abstract class WPFGraphEventClasses
    {
        public class RendererChangedEventArgs : EventArgs
        {
            private String property = null;
            private object oldValue = null;
            private object newValue = null;

            public RendererChangedEventArgs(String property, object oldValue, object newValue)
            {
                this.property = property;
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public String Property
            {
                get
                {
                    return property;
                }
            }

            public object OldValue
            {
                get
                {
                    return oldValue;
                }
            }

            public object NewValue
            {
                get
                {
                    return newValue;
                }
            }
        }

        public class DataPointChangedEventArgs : EventArgs
        {
            private WPFGraphEnumerations.Axis axis;
            private double oldValue;
            private double newValue;

            public DataPointChangedEventArgs(WPFGraphEnumerations.Axis axis, double oldValue, double newValue)
            {
                this.axis = axis;
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public WPFGraphEnumerations.Axis Axis
            {
                get
                {
                    return axis;
                }
            }

            public double OldValue
            {
                get
                {
                    return oldValue;
                }
            }

            public double NewValue
            {
                get
                {
                    return newValue;
                }
            }
        }

        public class PointRendererChangedEventArgs : EventArgs
        {
            private IWPFGraphPointRenderer oldValue;
            private IWPFGraphPointRenderer newValue;

            public PointRendererChangedEventArgs(IWPFGraphPointRenderer oldValue, IWPFGraphPointRenderer newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public IWPFGraphPointRenderer OldValue
            {
                get
                {
                    return oldValue;
                }
            }

            public IWPFGraphPointRenderer NewValue
            {
                get
                {
                    return newValue;
                }
            }
        }

        public class LineRendererChangedEventArgs : EventArgs
        {
            private IWPFGraphLineRenderer oldValue;
            private IWPFGraphLineRenderer newValue;

            public LineRendererChangedEventArgs(IWPFGraphLineRenderer oldValue, IWPFGraphLineRenderer newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            public IWPFGraphLineRenderer OldValue
            {
                get
                {
                    return oldValue;
                }
            }

            public IWPFGraphLineRenderer NewValue
            {
                get
                {
                    return newValue;
                }
            }
        }

        public class DataSeriesChangedEventArgs : EventArgs
        {
            private WPFGraphEnumerations.ListAction listAction;
            private int index;
            private WPFGraphDataPoint data;

            public DataSeriesChangedEventArgs(WPFGraphEnumerations.ListAction axis, int index, WPFGraphDataPoint data)
            {
                this.listAction = axis;
                this.index = index;
                this.data = data;
            }

            public WPFGraphEnumerations.ListAction ListAction
            {
                get
                {
                    return listAction;
                }
            }

            public int Index
            {
                get
                {
                    return index;
                }
            }

            public WPFGraphDataPoint Data
            {
                get
                {
                    return data;
                }
            }
        }
    }
}
