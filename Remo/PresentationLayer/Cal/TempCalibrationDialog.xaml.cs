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
using DataSourceLayer;
using System.ComponentModel;
using System.Windows.Threading;

namespace PresentationLayer
{
    
    /// <summary>
    /// Interaction logic for TempCalibrationDialog.xaml
    /// </summary>
    public partial class TempCalibrationDialog : Window
    {
        private EntityLayer.TempCalMeasurenment _tempCalMeasurenment;
        public EntityLayer.TempCalMeasurenment TempCalMeasurenment { get { return _tempCalMeasurenment; } }

        private bool IS_TEST = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["IS_TEST"]);
        private EntityLayer.TempMeasurenementConfiguration _tempMeasurenementConfiguration;

        private DataSourceServices dataSourceServices;
        public TempCalibrationDialog()
        {
            InitializeComponent();
            _tempMeasurenementConfiguration = new EntityLayer.TempMeasurenementConfiguration { IsChannel1On = true, IsChannel2On = true, IsChannel3On = true, IsChannel4On = true };
            _tempMeasurenementConfiguration.TempMeasurenments = new EntityLayer.ListWithChangeEvents<EntityLayer.TempMeasurenment>();
            _tempMeasurenementConfiguration.TempNoOfSamplesCurrentState = 1;
            _tempMeasurenementConfiguration.TempSampleRateCurrentState = 1;
            dataSourceServices = new DataSourceServices();
            dataSourceServices.start_TempMeasurenment(_tempMeasurenementConfiguration, IS_TEST, false);
            dataSourceServices.TempMeasurenmentDone +=new DataSourceServices.TempMeasurenmentDoneEvent(dataSourceServices_TempMeasurenmentDone);
            
            T1Meas.DataContext = this;
            T2Meas.DataContext = this;
            T3Meas.DataContext = this;
            T4Meas.DataContext = this;
        }
        class Temps
        {
            public double T1 { get; set; }
            public double T2 { get; set; }
            public double T3 { get; set; }
            public double T4 { get; set; }
        }
        Temps t;
        private void dataSourceServices_TempMeasurenmentDone(List<double> correctedTemps, List<double> realTemps)
        {
            t = new Temps {
                T1 = realTemps[0],
                T2 = realTemps[1],
                T3 = realTemps[2],
                T4 = realTemps[3]
            };
            T1Meas.DataContext = t;
            T2Meas.DataContext = t;
            T3Meas.DataContext = t;
            T4Meas.DataContext = t;
            _tempMeasurenementConfiguration.TempSampleRateCurrentState = 1;
            _tempMeasurenementConfiguration.TempNoOfSamplesCurrentState = 1;
            dataSourceServices.start_TempMeasurenment(_tempMeasurenementConfiguration, IS_TEST, false);    
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Контруирај entity објект со мерените и внесените вредности
                _tempCalMeasurenment = new EntityLayer.TempCalMeasurenment(
                DateTime.Now,
                t.T1,
                Convert.ToDouble(T1Ref.Text.Replace(".", ",")),
                t.T2,
                Convert.ToDouble(T2Ref.Text.Replace(".", ",")),
                t.T3,
                Convert.ToDouble(T3Ref.Text.Replace(".", ",")),
                t.T4,
                Convert.ToDouble(T4Ref.Text.Replace(".", ","))
                );
                this.DialogResult = true;
            }
            catch (FormatException ex) 
            {
                this.DialogResult = false;
            }            
        }
    }
}
