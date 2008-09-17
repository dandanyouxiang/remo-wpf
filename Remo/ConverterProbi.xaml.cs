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

namespace ConverterProbi
{
    class Entity
    {
        public double Double1 { get; set; }
        public int Int1 { get; set; }
        public double Double2 { get; set; }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Entity e;
        public Window1()
        {
            InitializeComponent();
            
            e = new Entity { Double1 = 10.123456789, Int1 = 39, Double2 = 0.12121212 };
            textBox1.DataContext = e;
            intTextBox.DataContext = e;
            textBox3.DataContext = e;
        }

        private void intTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show("Error");
        }
    }
}
