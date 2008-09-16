using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNBSoft.WPF.WPFGraph
{
    public abstract class WPFGraphDelegates
    {
        public delegate void RendererChangedEventDelegate(object sender, WPFGraphEventClasses.RendererChangedEventArgs args);

        public delegate void DataPointChangedEventDelegate(object sender, WPFGraphEventClasses.DataPointChangedEventArgs args);

        public delegate void PointRendererChangedEventDelegate(object sender, WPFGraphEventClasses.PointRendererChangedEventArgs args);

        public delegate void LineRendererChangedEventDelegate(object sender, WPFGraphEventClasses.PointRendererChangedEventArgs args);

        public delegate void DataSeriesChangedEventDelegate(object sender, WPFGraphEventClasses.DataSeriesChangedEventArgs args);
    }
}
