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

namespace WpfCustomControlsClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            double _max;
            if (double.TryParse(Max.Text, out _max))
                Thermometer.Maximum = _max;
            double _min;
            if (double.TryParse(Min.Text, out _min))
                Thermometer.Minimum = _min;
            double _value;
            if (double.TryParse(Value.Text, out _value))
            {
                Thermometer.Value = _value;
                ThChannel.Value = _value;
            }
        
        }

    }
}
