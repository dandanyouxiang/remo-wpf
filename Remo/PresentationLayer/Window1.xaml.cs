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

namespace PresentationLayer
{
    
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Поле кое чува кој е работниот директориум.
        /// </summary>
        public string WorkPlacePath {get;set; }
        public string FileName { get; set; }

        /// <summary>
        /// Состојбите во кои апликацијата се наоѓа, во однос на мерењата.
        /// </summary>
        public enum ProcessStatesEnum { DcColdRes = 0, DcCoolRes, DcColdTemp, ACHotTemp, Idle };
        private ProcessStatesEnum currentProcessState = ProcessStatesEnum.Idle;
        public ProcessStatesEnum CurrentProcessState
        {
            get { return currentProcessState; }
            set
            {
                if (value != currentProcessState)
                {
                    currentProcessState = value;
                    int index = (int)currentProcessState;
                    StatusString = statusStrings[index / 2];
                    this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                }
            }
        }
        string[] statusStrings = new String[] { "Measuring Ressistance", "Measuring Temperature", "Idle"};
        [DefaultValue("Idle")]
        public string StatusString { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        DataSourceLayer.DataSourceServices ds;
        
        /// <summary>
        /// Изворот на податоци.
        /// </summary>
        DataSource datasource;

        /// <summary>
        /// РАбота со фајлови.
        /// </summary>
        FileStoring fileStoring;

        public Window1()
        {
            InitializeComponent();
            //Todo da se izbrishe posle - DA SE PAMTI nekade

            fileStoring = XmlFileServices.readXml(@"E:\file.info");
            
            //WorkPlacePath=@"E:\";
            WorkPlacePath = fileStoring.WorkplacePath;
            FileName = fileStoring.FileName;


            this.Title = "Remo - " + FileName;
            
            //Todo Da se vidi dali e vo red na ovoj nacin da se cita prethodno zacuvanata programa.
            datasource = new DataSource(FileName, FileCommand.Open);
            MainGrid.DataContext = datasource;
            StatusString = statusStrings[2];
            this.graphsInit();
            try
            {
                datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DCColdTemperatureTable_PropertyChanged);
                datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DCColdRessistanceTable_PropertyChanged);
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.PropertyChanged += new PropertyChangedEventHandler(ACTempMeasurenments_PropertyChanged);
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments.PropertyChanged += new PropertyChangedEventHandler(DcCoolingRessistanceMeasurenments_PropertyChanged);
                datasource.PropertyChanged+=new PropertyChangedEventHandler(datasource_PropertyChanged);

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
                TestCurrentTextBox.DataContext = datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel;
                NoOfSamplesTextBox.DataContext = datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel;

                //postavuvanje na datakontest na kanalite vo vtoriot tab
                /*
                thermometerChannelAC1.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC2.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC3.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                thermometerChannelAC4.DataContext = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();
                */
                statusTextBlock.DataContext = this;

                if (datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count>0)
                {
                    thermometerChannelAC1.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1;
                    thermometerChannelAC2.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2;
                    thermometerChannelAC3.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3;
                    thermometerChannelAC4.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;
                }



                //Todo termometrite vo vtoriot tab
                if (datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count > 0)
                {
                    thermometerChannelAC1.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1;
                    thermometerChannelAC2.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2;
                    thermometerChannelAC3.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3;
                    thermometerChannelAC4.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;
                }

                //Todo Rabota so fajlovi
                #region File Manipulation

                //XmlFileServices.writeToXml(@"E:\file.info",new FileStoring(".remo","Remo file","Data.remo",@"E:\"));

                //fileStoring = new FileStoring(WorkPlacePath);
                //fileStoring.FileExtension = "remo";
                //fileStoring.FileDescription = "Remo documents";
                

                #endregion File Manipulation


            }
            catch (Exception ex) 
            {
                ex.ToString();
                throw ex;
            }
        }
        public void  datasource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StdTemp")
                datasource.calcDcResValues();
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
            if(this.CurrentProcessState == ProcessStatesEnum.Idle)
                datasource.calcDcResValues();
        }

        /// <summary>
        /// OnPropertyChanged Handler за температурните мерења при греење - AcHot
        /// </summary>
        public void ACTempMeasurenments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Повторно се наведува ItemsSource-от на оваа табела.
            ACTable.ItemsSource = datasource.ACHeatingTable();

            thermometerChannelAC1.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1;
            thermometerChannelAC2.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2;
            thermometerChannelAC3.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3;
            thermometerChannelAC4.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;
        }
        /// <summary>
        /// OnPropertyChanged Handler за мерењата на отпор при ладење - DcCooling
        /// </summary>
        public void DcCoolingRessistanceMeasurenments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Повторно се наведува ItemsSource-от на оваа табела.
            DCCoolTable.ItemsSource = datasource.DCCoolingTable();
        }

        /// <summary>
        /// Старт на температурно мерењена ладно DcCold.
        /// </summary>
        private void startTempMeasDcColdButton_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentProcessState == ProcessStatesEnum.Idle)
            {
                if (((ToggleButton)e.Source).IsChecked == true)
                {
                    CurrentProcessState = ProcessStatesEnum.DcColdTemp;
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
            }
            else
            {
                startTempMeasDcColdButton.IsChecked = false;
            }
        }

        /// <summary>
        /// Handler за крај на температурнто мерење.
        /// </summary>
        private void ds_TempMeasurenmentFinished()
        {
            startTempMeasDcColdButton.IsChecked = false;
            startAcButton.IsChecked = false;
            CurrentProcessState = ProcessStatesEnum.Idle;
            this.acGraphRefresh();
        }

        /// <summary>
        /// Старт на мерење на отпор на ладно - DcCold.
        /// </summary>
        private void startResMeasDcColdButton_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentProcessState == ProcessStatesEnum.Idle)
            {
                if (((ToggleButton)e.Source).IsChecked == true)
                {
                    CurrentProcessState = ProcessStatesEnum.DcColdRes;
                    datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel].IsChannel1On = true;
                    datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel].IsChannel2On = true;
                    //Стартувај го мерењето на отпор
                    DataSourceLayer.DataSourceServices ds = new DataSourceLayer.DataSourceServices();
                    ds.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEventHandler(ds_RessistanceMeasurenmentFinished);
                    ds.start_RessistanceMeasurenment(datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel], false);
                }
            }
            else
            {
                startResMeasDcColdButton.IsChecked = false;
            }
        }
        /// <summary>
        /// Handler за крај на мерењето на отпор.
        /// </summary>
        private void ds_RessistanceMeasurenmentFinished()
        {
            startResMeasDcColdButton.IsChecked = false;
            StartDcCoolResButton.IsChecked = false;
            TNullButton.IsChecked = false;
            TNullButton.IsEnabled = true;
            CurrentProcessState = ProcessStatesEnum.Idle;
        }

        private void ChannelsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datasource.setValuesForSelectedChannel();

            GridViewColumn colum = new GridViewColumn();
            if (datasource.SelectedChannel < datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels.Count)
            {
                DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
                NoOfSamplesRessTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                TestCurrentTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                datasource.calcDcResValues();
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

        private void startAcButton_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentProcessState == ProcessStatesEnum.Idle)
            {
                reduceButton.IsEnabled = true;
                reduceButton.IsChecked = false;
                CurrentProcessState = ProcessStatesEnum.ACHotTemp;
                //Кои канали се On или Off
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel1On = thermometerChannelAC1.IsChannelOn;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel2On = thermometerChannelAC2.IsChannelOn;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel3On = thermometerChannelAC3.IsChannelOn;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel4On = thermometerChannelAC4.IsChannelOn;
                //Кои канали се Oil или Amb
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil = thermometerChannelAC1.IsOil;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil = thermometerChannelAC2.IsOil;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil = thermometerChannelAC3.IsOil;
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil = thermometerChannelAC4.IsOil;

                //Стартувај го мерењето на температура
                ds = new DataSourceLayer.DataSourceServices();
                ds.TempMeasurenmentFinished += new DataSourceLayer.DataSourceServices.TempMeasurenmentFinishedEventHandler(ds_TempMeasurenmentFinished);
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempNoOfSamplesCurrentState = 5;
                ds.start_TempMeasurenment(datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration);
                //
            }
        }
        /// <summary>
        /// Стопирање на температурно мерење
        /// </summary>
        private void startAcButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)e.Source).IsChecked == false && CurrentProcessState != ProcessStatesEnum.Idle)
            {

                ds.IsTempMeasStopped = true;
                startAcButton.IsChecked = false;
            }
        }
        /// <summary>
        /// Старт на мерење на отпор на ладење DcCooling
        /// </summary>
        private void StartDcCoolResButton_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentProcessState == ProcessStatesEnum.Idle)
            {
                CurrentProcessState = ProcessStatesEnum.DcCoolRes;
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.IsChannel1On = true;
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.IsChannel2On = true;
                //Стартувај го мерењето на отпор
                ds = new DataSourceLayer.DataSourceServices();
                ds.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEventHandler(ds_RessistanceMeasurenmentFinished);
                ds.start_RessistanceMeasurenment(datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel, true);
            }
            else
            {
                StartDcCoolResButton.IsChecked = false;
            }
        }

        private void reduceButton_Checked(object sender, RoutedEventArgs e)
        {
            ds.IsSampleReduced = true;
            ds.IsTempMeasInterupted = true;
            reduceButton.IsEnabled = false;
        }

        private void TNullButton_Checked(object sender, RoutedEventArgs e)
        {
            datasource.Root.DcCoolingMeasurenments.TNullTime = DateTime.Now;
            TNullButton.IsEnabled = false;
        }

        private void EndAcTempTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
           
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// При Validation Error на FrameworkElement компонента,
        /// да се врати последната валидна вредност (вредноста во binding source-от)
        /// </summary>
        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            ((FrameworkElement)e.OriginalSource).GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void MeanDCColdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox =(TextBox)sender;
            switch (textBox.Name) 
            {
                case "T1MeanDCColdTextBox": thermometerChannel1.Value = datasource.T1MeanDCColdTempTable; break;
                case "T2MeanDCColdTextBox": thermometerChannel2.Value = datasource.T2MeanDCColdTempTable; break;
                case "T3MeanDCColdTextBox": thermometerChannel3.Value = datasource.T3MeanDCColdTempTable; break;
                case "T4MeanDCColdTextBox": thermometerChannel4.Value = datasource.T4MeanDCColdTempTable; break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        
    }
    //Todo zema mnogu procesorsko vreme
    /// <summary>
    /// Поставуванје на стил за табелатата во ACHeating табот
    /// </summary>
    public class ListViewItemStyleSelector : StyleSelector
    {
        public int reducedIndex{get;set;}

        //Todo zema mnogu procesorsko vreme
        public override Style SelectStyle(object item, DependencyObject container)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("start SelectStyle");
            Style st = new Style();
            try
            {
                st.TargetType = typeof(ListViewItem);
                Setter backGroundSetter = new Setter();
                backGroundSetter.Property = ListViewItem.BackgroundProperty;
                ListView listView =
                    ItemsControl.ItemsControlFromItemContainer(container)
                      as ListView;
               
                int index =
                    listView.ItemContainerGenerator.IndexFromContainer(container);
                //Todo ??? zasto se cita pak
                
                DataSource datasource = new DataSource(@"D:\root.xml",FileCommand.New);
                reducedIndex = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.FindIndex(Utils.isReduced);

                if (index == reducedIndex)
                {
                    backGroundSetter.Value = Brushes.LightBlue;
                }
                else
                {
                    backGroundSetter.Value = Brushes.White;
                }
                st.Setters.Add(backGroundSetter);
            }
            catch (Exception ex) 
            {
                DateTime end = DateTime.Now;
                Console.WriteLine("end SelectStyle - Time:" + (end - start).TotalMilliseconds + " ms");
                return null;
            }
            DateTime end1 = DateTime.Now;
            Console.WriteLine("end SelectStyle - Time:"+(end1-start).TotalMilliseconds+" ms");
            return st;
        }
    } 
}
