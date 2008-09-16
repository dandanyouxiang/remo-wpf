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

namespace DNBSoft.WPF.WPFGraph
{
    /// <summary>
    /// Interaction logic for WPFGraphConfigurationDoubleProperty.xaml
    /// </summary>
    public partial class WPFGraphConfigurationDoubleProperty : UserControl
    {
        private IWPFGraphParameterSet target;
        private String name;
        private double value;
        private String lastValue = null;
        private IWPFGraph graph = null;
        private WPFSeriesConfigurationWindow.UpdatePreviewDelegate updatePreview;

        public WPFGraphConfigurationDoubleProperty(IWPFGraph graph, IWPFGraphParameterSet target, 
            String name, WPFSeriesConfigurationWindow.UpdatePreviewDelegate updatePreview)
        {
            InitializeComponent();

            this.graph = graph;
            this.target = target;
            this.name = name;
            this.value = double.Parse(target.getValue(name).ToString());
            lastValue = value.ToString();
            this.updatePreview = updatePreview;

            nameLabel.Content = name;
            valueTextBox.Text = value.ToString();
        }

        private void valueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lastValue = double.Parse(valueTextBox.Text).ToString();
            }
            catch (Exception)
            {
                valueTextBox.Text = lastValue;
            }
            finally
            {
                target.setValue(name, (object)double.Parse(valueTextBox.Text));
                value = double.Parse(valueTextBox.Text);
                if (graph != null)
                {
                    graph.Refresh();
                }
                if (updatePreview != null)
                {
                    updatePreview();
                }
            }
        }
    }
}
