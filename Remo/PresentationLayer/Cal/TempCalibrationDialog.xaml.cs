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

        private EntityLayer.ListWithChangeEvents<EntityLayer.TempMeasurenment> _tempMeasurenments;

        private DataSourceServices dataSourceServices;
        public TempCalibrationDialog()
        {
            InitializeComponent();
            _tempMeasurenments = new EntityLayer.ListWithChangeEvents<EntityLayer.TempMeasurenment>();
            dataSourceServices = new DataSourceServices();
            dataSourceServices.start_TempMeasurenment(1, 1, _tempMeasurenments);
            dataSourceServices.TempMeasurenmentFinished+=new DataSourceServices.TempMeasurenmentFinishedEventHandler(dataSourceServices_TempMeasurenmentFinished);
            
        }

        private void dataSourceServices_TempMeasurenmentFinished()
        {
            // Get the dispatcher from the current window, and use it to invoke
            // the update code.
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (DataSourceServices.TempMeasurenmentFinishedEventHandler)delegate()
            {
                T1.DataContext = T2.DataContext = T3.DataContext = T4.DataContext = _tempMeasurenments[0];
                dataSourceServices.start_TempMeasurenment(1, 1, _tempMeasurenments);
            }
            );       
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            //Контруирај entity објект со мерените и внесените вредности
            //Todo потребни се конвертори и валидатори
            _tempCalMeasurenment = new EntityLayer.TempCalMeasurenment(
                DateTime.Now,
                Convert.ToDouble(T1.Text.Replace(".",",")),
                Convert.ToDouble(T1Ref.Text.Replace(".", ",")),
                Convert.ToDouble(T2.Text.Replace(".", ",")),
                Convert.ToDouble(T2Ref.Text.Replace(".", ",")),
                Convert.ToDouble(T3.Text.Replace(".", ",")),
                Convert.ToDouble(T3Ref.Text.Replace(".", ",")),
                Convert.ToDouble(T4.Text.Replace(".", ",")),
                Convert.ToDouble(T4Ref.Text.Replace(".", ","))
                );
            this.DialogResult = true;
        }
    }
}
