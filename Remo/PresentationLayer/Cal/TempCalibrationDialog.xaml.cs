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

        private EntityLayer.TempMeasurenementConfiguration _tempMeasurenementConfiguration;

        private DataSourceServices dataSourceServices;
        public TempCalibrationDialog()
        {
            InitializeComponent();
            _tempMeasurenementConfiguration = new EntityLayer.TempMeasurenementConfiguration { IsChannel1On = true, IsChannel2On = true, IsChannel3On = true, IsChannel4On = true };
            _tempMeasurenementConfiguration.TempMeasurenments = new EntityLayer.ListWithChangeEvents<EntityLayer.TempMeasurenment>();

            dataSourceServices = new DataSourceServices();
            dataSourceServices.start_TempMeasurenment(_tempMeasurenementConfiguration, true, false);
            dataSourceServices.TempMeasurenmentFinished+=new DataSourceServices.TempMeasurenmentFinishedEvent(dataSourceServices_TempMeasurenmentFinished);
            
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
        private void dataSourceServices_TempMeasurenmentFinished()
        {
            t = new Temps { 
                T1 = _tempMeasurenementConfiguration.TempMeasurenments[0].T1, 
                T2 = _tempMeasurenementConfiguration.TempMeasurenments[0].T2,
                T3 = _tempMeasurenementConfiguration.TempMeasurenments[0].T3,
                T4 = _tempMeasurenementConfiguration.TempMeasurenments[0].T4
            };
            T1Meas.DataContext = t;
            T2Meas.DataContext = t;
            T3Meas.DataContext = t;
            T4Meas.DataContext = t;
            _tempMeasurenementConfiguration.TempSampleRateCurrentState = 1;
            _tempMeasurenementConfiguration.TempNoOfSamplesCurrentState = 2;
            dataSourceServices.start_TempMeasurenment(_tempMeasurenementConfiguration, true, false);    
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
