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
using System.ComponentModel;
using DataAccessLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DataSource datasource;
        public Window1()
        {
            InitializeComponent();
            datasource = new DataSource(@"E:\root.xml");
            MainGrid.DataContext = datasource;
            try
            {
                datasource.PropertyChanged += new PropertyChangedEventHandler(DCColdTemperatureTable_PropertyChanged);
                datasource.PropertyChanged += new PropertyChangedEventHandler(DCColdRessistanceTable_PropertyChanged);

                DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
                DCColdRessistanceTable.DataContext = datasource;

                NoOfSamplesRessTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                SampleRateTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;

                TestCurrentTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                NoOFSamplesTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;
                //SampleRateRessTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];

                DCColdTemperatureTable.ItemsSource = datasource.DCColdTemperatureTable();
                DCColdTemperatureTable.DataContext = datasource;

                DCCoolTable.ItemsSource = datasource.DCCoolingTable();

                ACTable.ItemsSource = datasource.ACHeatingTable();

                //thermometerChannel1.DataContext = datasource;
                //cooling
                TestCurrentTextBox.DataContext = datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannels;
                NoOfSamplesTextBox.DataContext = datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannels;

                //postavuvanje na datakontest na kanalite vo vtoriot tab

                thermometerChannelAC1.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC2.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC3.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC4.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();

            }
            catch (Exception ex) 
            {
                ex.ToString();
            }
            //RessistanceTable.bi
        }

        public void DCColdTemperatureTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DCColdTemperatureTable.ItemsSource = datasource.DCColdTemperatureTable();

            //Promena na vrednostite na termometrite
            //thermometerChannel1.Value = datasource.T1MeanDCColdTempTable;
            //thermometerChannel2.Value = datasource.T2MeanDCColdTempTable;
            //thermometerChannel3.Value = datasource.T3MeanDCColdTempTable;
            //thermometerChannel4.Value = datasource.T4MeanDCColdTempTable;
        }

        public void DCColdRessistanceTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(ChannelsListBox.SelectedIndex);
        }

        /// <summary>
        /// Старт на температурно мерење
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startTempMeasDcColdButton_Click(object sender, RoutedEventArgs e)
        {
            //Кои канали се On или Off
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1On = thermometerChannel1.IsChannelOn;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2On = thermometerChannel2.IsChannelOn;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3On = thermometerChannel3.IsChannelOn;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4On = thermometerChannel4.IsChannelOn;
            //Кои канали се Oil или Amb
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil = thermometerChannel1.IsOil;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil = thermometerChannel2.IsOil;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil = thermometerChannel3.IsOil;
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil = thermometerChannel4.IsOil;

            //Стартувај го мерењето на температура
            DataSourceLayer.DataSourceServices ds = new DataSourceLayer.DataSourceServices();
            ds.TempMeasurenmentFinished += new DataSourceLayer.DataSourceServices.TempMeasurenmentFinishedEventHandler(ds_TempMeasurenmentFinished);
            ds.start_TempMeasurenment(datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration);
            //
        }

        /// <summary>
        /// Handler за крај на температурнто мерење
        /// </summary>
        private void ds_TempMeasurenmentFinished()
        {
        }

        private void startResMeasDcColdButton_Click(object sender, RoutedEventArgs e)
        {
            datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel].IsChannel1On = true;
            datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel].IsChannel2On = true;
            //Стартувај го мерењето на отпор
            DataSourceLayer.DataSourceServices ds = new DataSourceLayer.DataSourceServices();
            ds.TempMeasurenmentFinished += new DataSourceLayer.DataSourceServices.TempMeasurenmentFinishedEventHandler(ds_TempMeasurenmentFinished);
            ds.start_RessistanceMeasurenment(datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel]);
            //
        }

        private void ChannelsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datasource.setValuesForSelectedChannel();

            GridViewColumn colum = new GridViewColumn();
            if (datasource.SelectedChannel < datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels.Count)
            {
                DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
                
               // NoOfSamplesRessTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
               // SampleRateRessTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];

               // TestCurrentTempTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
               // NoOFSamplesTempTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
            }
            else 
            {
                DCColdRessistanceTable.ItemsSource = null;
                //NoOfSamplesRessTextBox.DataContext = null;
                //SampleRateRessTextBox.DataContext = null;

                //TestCurrentTempTextBox.DataContext = null;
                //NoOFSamplesTempTextBox.DataContext = null;
            }
        }



        private void DCColdTemperatureTable_SourceUpdated(object sender, DataTransferEventArgs e)
        {
          //  thermometerChannelAC1.Value = datasource.root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>().T1;
        }

        
  
        
    }
}
