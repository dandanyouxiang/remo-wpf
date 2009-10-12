using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using DataAccessLayer;
using System.IO;

namespace PresentationLayer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Commands

        /// <summary>
        /// Креирање на нов документ при ново мерење.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = fileStoring.sugestFileName(DateTime.Now); // Default file name
            dlg.DefaultExt = ".remo"; // Default file extension
            dlg.Filter = "Remo documents (.remo)|*.remo"; // Filter files by extension
            dlg.InitialDirectory = WorkPlacePath;
            dlg.ValidateNames = true;
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FileName = dlg.FileName;
                this.Title = "Remo - " + FileName;
                //polneje na fajlot
                datasource = new DataSource(FileName, FileCommand.New);
                
                MainGrid.ClearValue(Grid.DataContextProperty);

                setDataContext();
                updateGraphsAndFields();

                MainGrid.UpdateLayout();
            }

        }
        /// <summary>
        /// Команда за отворање на некој фајл со зачувани податоци од некое мерење.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = fileStoring.getFirstFile(); // Default file name
            dlg.DefaultExt = ".remo"; // Default file extension
            dlg.Filter = "Remo documents (.remo)|*.remo"; // Filter files by extension
            dlg.InitialDirectory = WorkPlacePath;
            dlg.ValidateNames = true;
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FileName = dlg.FileName;
                this.Title = "Remo - " + FileName;
                //polneje na fajlot
                datasource = new DataSource(FileName, FileCommand.Open);

                MainGrid.ClearValue(Grid.DataContextProperty);

                setDataContext();
                updateGraphsAndFields();
                MainGrid.UpdateLayout();
            }

        }
        /// <summary>
        /// Команда за зачувување на мерењето.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           datasource.saveData(FileName);
        }
        /// <summary>
        /// Команда за затворање.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Команда за подесување на работниот простор.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_WorkPlacePath_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            WorkPlacePath workPlacePath = new WorkPlacePath(fileStoring);

            if ((bool)workPlacePath.ShowDialog()) 
            {
                //System.Reflection.Assembly.GetExecutingAssembly().Location
                fileStoring.WorkplacePath = workPlacePath.Path;
                string baseDir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                XmlFileServices.writeToXml(baseDir+@"Ref\file.info", fileStoring);
                WorkPlacePath = fileStoring.WorkplacePath;
            }
        }
        /// <summary>
        /// Команди за принтање.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ReportsLayer.GenerateReports gr = new ReportsLayer.GenerateReports(datasource, AcGraph, T1Graph, T2Graph);


            if ((bool)gr.ShowDialog())
            {
                
            }
                
        }
        /// <summary>
        /// Команда за подесување на податоците за трансформаторот.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_TransformatorProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            TransformatorProperties tp = new TransformatorProperties(datasource);

            if ((bool)tp.ShowDialog())
            {
            }
            save();

        }

        private void Command_SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = fileStoring.sugestFileName(DateTime.Now); // Default file name
            dlg.DefaultExt = ".remo"; // Default file extension
            dlg.Filter = "Remo documents (.remo)|*.remo"; // Filter files by extension
            dlg.InitialDirectory = WorkPlacePath;
            dlg.ValidateNames = true;
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FileName = dlg.FileName;
                this.Title = "Remo - " + FileName;
                datasource.saveData(FileName);
            }

        }

        #endregion Commands

        #region Other Functions

        /// <summary>
        /// Поставување на датаконтекст на контролите.
        /// Се користи при open.
        /// </summary>
        private void setDataContext()
        {
            MainGrid.DataContext = datasource;
            
            datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DCColdTemperatureTable_PropertyChanged);
            datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DCColdRessistanceTable_PropertyChanged);
            datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.PropertyChanged += new PropertyChangedEventHandler(ACTempMeasurenments_PropertyChanged);
            datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments.PropertyChanged += new PropertyChangedEventHandler(DcCoolingRessistanceMeasurenments_PropertyChanged);
            datasource.PropertyChanged += new PropertyChangedEventHandler(datasource_PropertyChanged);

            datasource.SelectedChannel = 0;
            DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
            DCColdRessistanceTable.DataContext = datasource;

            NoOfSamplesRessTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
            SampleRateTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;

            TestCurrentTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
            NoOFSamplesTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;

            DCColdTemperatureTable.ItemsSource = datasource.DCColdTemperatureTable();
            DCColdTemperatureTable.DataContext = datasource;

            DCCoolTable.ItemsSource = datasource.DCCoolingTable();

            ACTable.ItemsSource = datasource.ACHeatingTable();
            
            statusTextBlock.DataContext = this;
        }
        /// <summary>
        /// Се користи при open 
        /// </summary>
        private void updateGraphsAndFields()
        {
            this.acGraphRefresh();

            datasource.calculateResults();
            this.dcCoolingGraphsRefresh();

        }
        #endregion
    }
}
