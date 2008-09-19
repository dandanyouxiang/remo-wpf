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

        DataSourceLayer.DataSourceServices s;
        EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment> m;
        public RessistanceCalibrationDialog2(EntityLayer.RessistanceCalMeasurenment ressistanceCalMeasurenment)
        {
            InitializeComponent();

            RessistanceCalMeasurenment = ressistanceCalMeasurenment;

            s = new DataSourceLayer.DataSourceServices();
            m = new EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment>();
            s.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEventHandler(s_RessistanceMeasurenmentFinished);
        }
        
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void s_RessistanceMeasurenmentFinished()
        {
            RessistanceCalMeasurenment.RMeas = m[0].Voltage / m[0].Current;
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Започни го мерењто на отпор
            s.start_RessistanceMeasurenment(1, 1, m);
        }

    }
}
