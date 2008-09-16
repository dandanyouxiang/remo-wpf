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
using Microsoft.Samples.CustomControls;

namespace DNBSoft.WPF.WPFGraph
{
    /// <summary>
    /// Interaction logic for WPFGraphConfigurationBrushProperty.xaml
    /// </summary>
    public partial class WPFGraphConfigurationBrushProperty : UserControl
    {
        private IWPFGraphParameterSet target;
        private String name;
        private SolidColorBrush value;
        private IWPFGraph graph = null;
        private WPFSeriesConfigurationWindow.UpdatePreviewDelegate updatePreview;

        public WPFGraphConfigurationBrushProperty(IWPFGraph graph, IWPFGraphParameterSet target,
            String name, WPFSeriesConfigurationWindow.UpdatePreviewDelegate updatePreview)
        {
            InitializeComponent();

            this.graph = graph;
            this.target = target;
            this.name = name;
            this.value = (SolidColorBrush)target.getValue(name);
            this.updatePreview = updatePreview;

            nameLabel.Content = name;
            previewCanvas.Background = value;
        }

        private void configureButton_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker p = new ColorPicker();
            p.R = value.Color.R;
            p.G = value.Color.G;
            p.B = value.Color.B;
            p.A = value.Color.A;

            ColorPickerDialog cpd = new ColorPickerDialog();
            //cpd.SelectedColor = value.Color;

            cpd.ShowDialog();

            if ((bool)cpd.DialogResult)
            {
                value.Color = cpd.SelectedColor;
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
