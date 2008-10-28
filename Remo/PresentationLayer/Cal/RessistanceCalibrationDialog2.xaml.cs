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
        EntityLayer.RessistanceTransformerChannel ressistanceTransformerChannel;
        private List<double> ressistances;

        public RessistanceCalibrationDialog2(EntityLayer.RessistanceCalMeasurenment ressistanceCalMeasurenment)
        {
            InitializeComponent();

            RessistanceCalMeasurenment = ressistanceCalMeasurenment;

            s = new DataSourceLayer.DataSourceServices();
            ressistanceTransformerChannel = new EntityLayer.RessistanceTransformerChannel();
            ressistances = new List<double>();
            ressistanceTransformerChannel.TestCurrent = ressistanceCalMeasurenment.Current;
            ressistanceTransformerChannel.SampleRate = 5;
            ressistanceTransformerChannel.NoOfSamples = 4;
            s.RessistanceMeasurenmentDone += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentDoneEvent(s_RessistanceMeasurenmentDone);
            s.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEvent(s_RessistanceMeasurenmentFinished);
            //Стартувај го мерењето на отпор
            s.start_RessistanceMeasurenment(ressistanceTransformerChannel, IS_TEST, false);
        }
        private void s_RessistanceMeasurenmentFinished()
        {
            double rMeas = 0;
            foreach (double meas in ressistances)
                rMeas += meas;
            rMeas /= ressistances.Count;
            RessistanceCalMeasurenment.RMeas = rMeas;
            this.DialogResult = true;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            s.stopRessistanceMeasurenments();
            this.DialogResult = false;
        }
        public void s_RessistanceMeasurenmentDone(int channelNo, double correctedRess, double realRess)
        {
            if (channelNo == 1)
                ressistances.Add(realRess);
        }

    }
}
