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
using System.Windows.Controls.Primitives;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Interaction logic for ThermometerChannel.xaml
    /// </summary>
    public partial class ThermometerChannel : UserControl
    {

        public string Header { get { return header.Text; } set { header.Text = value; } }

        public bool IsOil
        {
            get
            {
                return comboBox.SelectedIndex == 1;
            }
            set
            {
                if (value)
                    comboBox.SelectedIndex = 1;
                else
                    comboBox.SelectedIndex = 2;
            }
        }

        public bool IsChannelOn
        {
            get
            {
                return (bool)OnOffButton.IsChecked;
            }
            set
            {
                OnOffButton.IsChecked = value;
            }
        }

        public Thermometer ThermometerControl
        {
            get { return thermometer; }
            set { thermometer = value; }
        }

        public double Value 
        {
            get { return thermometer.Value; }
            set { thermometer.Value = value; }  
        }

        public double Maximum
        {
            get { return thermometer.Maximum; }
            set { thermometer.Maximum = value; }
        }

        public double Minimum
        {
            get { return thermometer.Minimum; }
            set { thermometer.Minimum = value; }
        }

        public int DecimalsOnTicks
        {
            get { return thermometer.Decimals; }
            set { thermometer.Decimals = value; }
        }

        public int TickCount
        {
            get { return thermometer.Ticks.Count; }
            set
            {
                double tickCount = value;
                double dt = (Maximum - Minimum) / tickCount;
                DoubleCollection ticks = new DoubleCollection();
                for (int i = 0; i <= tickCount; i++)
                {
                    double tick = Math.Round(Minimum + i * dt, DecimalsOnTicks);
                    ticks.Add(tick);
                }
                thermometer.Ticks = ticks;
            }
        }

        public ThermometerChannel()
        {
            InitializeComponent(); 
        }
    }
}
