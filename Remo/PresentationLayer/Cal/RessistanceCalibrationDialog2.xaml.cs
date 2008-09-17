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
    /// Interaction logic for RessistanceCalibrationDialog2.xaml
    /// </summary>
    public partial class RessistanceCalibrationDialog2 : Window
    {

        /// <summary>
        /// Извршеното мерење
        /// </summary>
        public EntityLayer.RessistanceCalMeasurenment RessistanceCalMeasurenment { get; set; }

        public RessistanceCalibrationDialog2()
        {
            InitializeComponent();
        }
       
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            RessistanceCalMeasurenment = new EntityLayer.RessistanceCalMeasurenment();
            RessistanceCalMeasurenment.Time = DateTime.Now;
            RessistanceCalMeasurenment.RRef = 100;
            RessistanceCalMeasurenment.RMeas = 100.01;
            this.DialogResult = true;
        }

    }
}
