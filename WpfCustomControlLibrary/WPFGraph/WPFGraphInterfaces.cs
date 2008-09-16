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
    public abstract class WPFGraphInterfaces
    {
    }

    public interface IWPFGraphPointRenderer : IWPFGraphParameterSet
    {
        void render(WPFRenderParameters parameters, WPFGraphDataPoint p);
        event WPFGraphDelegates.RendererChangedEventDelegate RendererChanged;
        List<WPFGraphConfigurationParameter> getConfigurationParameters();
    }

    public interface IWPFGraphLineRenderer : IWPFGraphParameterSet
    {
        void render(WPFRenderParameters parameters, WPFGraphDataPoint p1, WPFGraphDataPoint p2);
        event WPFGraphDelegates.RendererChangedEventDelegate RendererChanged;
        List<WPFGraphConfigurationParameter> getConfigurationParameters();
    }

    public interface IWPFGraphParameterSet
    {
        void setValue(String parameter, object value);
        object getValue(String parameter);
    }

    public interface IWPFGraph
    {
        void Refresh();
        ListenableList<WPFGraphSeries> Series
        {
            get;
        }
    }
}
