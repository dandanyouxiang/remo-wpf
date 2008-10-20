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
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for WorkPlacePath.xaml
    /// </summary>
    public partial class WorkPlacePath : Window
    {
        public string Path{get;set;}

        public WorkPlacePath()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();

            fd.ShowDialog();

            // Process open file dialog box results
            if (true)
            {
                //Todo da se napravi da se odeluva samo patekata.
                WorkPlaceTextBox.Text = fd.SelectedPath; 
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Path=WorkPlaceTextBox.Text;
            this.DialogResult = true;
        }
    }
}
