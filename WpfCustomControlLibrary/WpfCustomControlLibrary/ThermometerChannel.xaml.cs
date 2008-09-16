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
        public bool IsOil
        {
            get
            {
                return comboBox.SelectedIndex == 1;
            }
        }
        public bool IsChannelOn { get { return (bool)OnOffButton.IsChecked; } }

        public double Value 
        {
            get { return thermometer.Value; }
            set { thermometer.Value = value; }  
        }

        public string Header { get { return header.Text; } set { header.Text = value; } }

        public ThermometerChannel()
        {
            InitializeComponent(); 
        }
    }
}
