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
        private bool IS_TEST = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["IS_TEST"]);
        DataSourceLayer.DataSourceServices s;
        EntityLayer.RessistanceTransformerChannel ch;
        public RessistanceCalibrationDialog2(EntityLayer.RessistanceCalMeasurenment ressistanceCalMeasurenment)
        {
            InitializeComponent();

            RessistanceCalMeasurenment = ressistanceCalMeasurenment;

            s = new DataSourceLayer.DataSourceServices();
            ch = new EntityLayer.RessistanceTransformerChannel();
            ch.TestCurrent = ressistanceCalMeasurenment.Current;
            ch.RessistanceNoOfSamplesCurrentState = 4;
            ch.RessistanceMeasurenments = new EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment>();
            s.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEvent(s_RessistanceMeasurenmentFinished);
        }
        
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            s.stopRessistanceMeasurenments();
            this.DialogResult = false;
        }
        public void s_RessistanceMeasurenmentFinished()
        {
            double rMeas = 0;
            double n = 0;
            foreach (EntityLayer.RessistanceMeasurenment meas in ch.RessistanceMeasurenments)
            {
                if (meas.ChannelNo == 1)
                {
                    rMeas += meas.Voltage / meas.Current;
                    n++;
                }
            }
            rMeas /= n;
            RessistanceCalMeasurenment.RMeas = rMeas;
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Започни го мерењто на отпор
            s.start_RessistanceMeasurenment(ch, IS_TEST, false);
        }

    }
}
