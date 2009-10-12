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
    public partial class ThermometerChannelTest : Window
    {
        public ThermometerChannelTest()
        {
            InitializeComponent();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            double _max;
            if (double.TryParse(Max.Text, out _max))
                ThChannel.Maximum = _max;

            double _min;
            if (double.TryParse(Min.Text, out _min))
                ThChannel.Minimum = _min;

            int _decimals;
            if (int.TryParse(Decimals.Text, out _decimals))
                ThChannel.DecimalsOnTicks = _decimals;

            double _value;
            if (double.TryParse(Value.Text, out _value))
                ThChannel.Value = _value;

            int _tickCount;
            if (int.TryParse(TickCount.Text, out _tickCount))
                ThChannel.TickCount = _tickCount;
        }
    }
}
