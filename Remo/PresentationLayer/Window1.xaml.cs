﻿using System;
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
            datasource = new DataSource(@"C:\Documents and Settings\Gjore\Desktop\Remo\root.xml");
            MainGrid.DataContext = datasource;
            try
            {
                datasource.PropertyChanged += new PropertyChangedEventHandler(DCColdTemperatureTable_PropertyChanged);
                datasource.PropertyChanged += new PropertyChangedEventHandler(DCColdRessistanceTable_PropertyChanged);

                DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
                NoOfSamplesRessTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                SampleRateRessTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];

                TestCurrentTempTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                NoOFSamplesTempTextBox.DataContext = datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                
                DCColdTemperatureTable.ItemsSource = datasource.DCColdTemperatureTable();
                DCColdTemperatureTable.DataContext = datasource;

                DCCoolTable.ItemsSource = datasource.DCCoolingTable();

                ACTable.ItemsSource = datasource.ACHeatingTable();


                //cooling
                TestCurrentTextBox.DataContext = datasource.root.DcCoolingMeasurenments.RessistanceTransformerChannels;
                NoOfSamplesTextBox.DataContext = datasource.root.DcCoolingMeasurenments.RessistanceTransformerChannels;

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
        }

        public void DCColdRessistanceTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(ChannelsListBox.SelectedIndex);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            //datasource.root.DcColdMeasurenments.RessistanceTransformerChannels[0].TestCurrent = 23;
            datasource.root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil = true;
            datasource.root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[0].Time = DateTime.Now;
        }

        private void ChannelsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridViewColumn colum = new GridViewColumn();
            if (datasource.SelectedChannel < datasource.root.DcColdMeasurenments.RessistanceTransformerChannels.Count)
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
        
    }
}
