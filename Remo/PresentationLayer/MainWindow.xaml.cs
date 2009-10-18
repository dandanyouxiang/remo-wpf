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
using System.Windows.Threading;

namespace PresentationLayer
{
    
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Поле кое чува кој е работниот директориум.
        /// </summary>
        public string WorkPlacePath { get; set; }

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
                    if(currentProcessState == ProcessStatesEnum.Idle)
                        progressBar1.IsIndeterminate = false;
                    else
                        progressBar1.IsIndeterminate = true;
                    int index = (int)currentProcessState;
                    StatusString = statusStrings[index / 2];
                    this.OnPropertyChanged(new PropertyChangedEventArgs(null));
                }
            }
        }
        string[] statusStrings = new String[] { "Measuring Ressistance", "Measuring Temperature", "Idle"};
        private bool IS_TEST;
        private DispatcherTimer acHotTimer;
        private TimeSpan acTimeToNextSample;
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

        public MainWindow()
        {
            InitializeComponent();
            
            IS_TEST = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["IS_TEST"]);
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            fileStoring = XmlFileServices.readXml(baseDir + @"Ref\file.info");
            WorkPlacePath = fileStoring.WorkplacePath;
            FileName = fileStoring.FileName;

            this.Title = "Remo - " + FileName;

            //Todo Da se vidi dali e vo red na ovoj nacin da se cita prethodno zacuvanata programa.
            datasource = new DataSource(FileName, FileCommand.New);
            MainGrid.DataContext = datasource;
            StatusString = statusStrings[2];
            this.graphsInit();
            try
            {
                datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DCColdTemperatureTable_PropertyChanged);
                datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DCColdRessistanceTable_PropertyChanged);
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.PropertyChanged += new PropertyChangedEventHandler(ACTempMeasurenments_PropertyChanged);
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments.PropertyChanged += new PropertyChangedEventHandler(DcCoolingRessistanceMeasurenments_PropertyChanged);
                datasource.PropertyChanged += new PropertyChangedEventHandler(datasource_PropertyChanged);

                DCColdRessistanceTable.ItemsSource = datasource.DCColdRessistanceTable(datasource.SelectedChannel);
                DCColdRessistanceTable.DataContext = datasource;

                NoOfSamplesRessTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                SampleRateTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;

                TestCurrentTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel];
                NoOFSamplesTempTextBox.DataContext = datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration;

                DCColdTemperatureTable.ItemsSource = datasource.DCColdTemperatureTable();
                DCColdTemperatureTable.DataContext = datasource;

                DCCoolTable.ItemsSource = datasource.DCCoolingTable();

                //ACTable.ItemsSource = datasource.ACHeatingTable();

                statusTextBlock.DataContext = this;
                
                if (datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count > 0)
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
                acHotTimer = new DispatcherTimer();
                //1 s
                acHotTimer.Interval = new TimeSpan(0, 0, 0, 1);
                acHotTimer.Tick += new EventHandler(acHotTimer_Tick);
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
            if (datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count > 0)
            {
                thermometerChannelAC1.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1;
                thermometerChannelAC2.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2;
                thermometerChannelAC3.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3;
                thermometerChannelAC4.Value = datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;
            }
            this.acGraphRefresh();
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
                    ds = new DataSourceLayer.DataSourceServices();
                    ds.TempMeasurenmentDone += new DataSourceLayer.DataSourceServices.TempMeasurenmentDoneEvent(ds_TempMeasurenmentDone);
                    ds.TempMeasurenmentFinished += new DataSourceLayer.DataSourceServices.TempMeasurenmentFinishedEvent(ds_TempMeasurenmentFinished);
                    ds.start_TempMeasurenment(datasource.Root.DcColdMeasurenments.TempMeasurenementConfiguration, IS_TEST, true);
                    //
                }
            }
            else
            {
                startTempMeasDcColdButton.IsChecked = false;
            }
        }

        private void startTempMeasDcColdButton_Click(object sender, RoutedEventArgs e)
        {
            //Стопирање на температурните мерења
            if (startTempMeasDcColdButton.IsChecked == false)
            {
                ds.stopTempMeasurenments();
                save();
            }
        }

        /// <summary>
        /// Завршено е едно температурно мерење
        /// </summary>
        public void ds_TempMeasurenmentDone(List<double> correctedTemps, List<double> realTemps)
        {
            //Рестарт на тајмерот
            if (currentProcessState == ProcessStatesEnum.ACHotTemp)
            {
                acTimeToNextSample = new TimeSpan(0, 0, datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState);
                AcTimeToNextSample.Text = getAcTimeToNextSampleString(acTimeToNextSample);
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
            //Ac timer
            acHotTimer.Stop();
            acTimeToNextSample = new TimeSpan(0, 0, datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState);
            AcTimeToNextSample.Text = getAcTimeToNextSampleString(acTimeToNextSample);
            save();
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
                    datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel].SampleRate = 5;
                    //Стартувај го мерењето на отпор
                    ds = new DataSourceLayer.DataSourceServices();
                    ds.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEvent(ds_ColdRessistanceMeasurenmentFinished);
                    ds.RessistanceMeasurenmentsError+=new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentsErrorEvent(ds_RessistanceMeasurenmentsError);
                    ds.start_RessistanceMeasurenment(datasource.Root.DcColdMeasurenments.RessistanceTransformerChannels[datasource.SelectedChannel], IS_TEST, true);
                }
            }
            else
            {
                startResMeasDcColdButton.IsChecked = false;
            }
        }
        private void startResMeasDcColdButton_Click(object sender, RoutedEventArgs e)
        {
            if (!startResMeasDcColdButton.IsChecked == true && currentProcessState == ProcessStatesEnum.DcColdRes)
            {
                ds.stopRessistanceMeasurenments();
                CurrentProcessState = ProcessStatesEnum.Idle;
                save();
            }
        }

        public void ds_RessistanceMeasurenmentsError()
        {
            MessageBox.Show("Се појави грешки при поврзување со инструментите.\nПробајте пак. Ако проблемот не се реши рестартирајте ги инструментите.", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
            ds.stopRessistanceMeasurenments();
            CurrentProcessState = ProcessStatesEnum.Idle;
            startResMeasDcColdButton.IsChecked = false;
        }

        /// <summary>
        /// Handler за крај на мерењето на отпор.
        /// </summary>
        private void ds_ColdRessistanceMeasurenmentFinished()
        {
            startResMeasDcColdButton.IsChecked = false;
            StartDcCoolResButton.IsChecked = false;
            TNullButton.IsChecked = false;
            TNullButton.IsEnabled = true;
            CurrentProcessState = ProcessStatesEnum.Idle;
            save();
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
            }
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
                ds.TempMeasurenmentDone += new DataSourceLayer.DataSourceServices.TempMeasurenmentDoneEvent(ds_TempMeasurenmentDone);
                ds.TempMeasurenmentFinished += new DataSourceLayer.DataSourceServices.TempMeasurenmentFinishedEvent(ds_TempMeasurenmentFinished);
                datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempNoOfSamplesCurrentState = 500000;
                ds.start_TempMeasurenment(datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration, IS_TEST, false);
                //
                acTimeToNextSample = new TimeSpan(0, 0, datasource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState);
                acHotTimer.Start();
            }
        }
        /// <summary>
        /// Стопирање на температурно мерење
        /// </summary>
        private void startAcButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)e.Source).IsChecked == false && CurrentProcessState != ProcessStatesEnum.Idle)
            {
                ds.stopTempMeasurenments();
                acHotTimer.Stop();
                AcTimeToNextSample.Text = "00:00";
                //pri testiranje
                ds.IsTempMeasStopped = true;
                CurrentProcessState = ProcessStatesEnum.Idle;
                datasource.evaluateTemperatureSettingsDcCooling();
                save();
            }
        }

        public void acHotTimer_Tick(object sender, EventArgs e)
        {
            acTimeToNextSample -= acHotTimer.Interval;
            AcTimeToNextSample.Text = getAcTimeToNextSampleString(acTimeToNextSample);
        }
        private string getAcTimeToNextSampleString(TimeSpan t)
        {
            string m = "";
            if (t.Minutes.ToString().Length == 1)
                m += "0";
            string s = "";
            if (t.Seconds.ToString().Length == 1)
                s += "0";
            return m + t.Minutes + ":" + s + t.Seconds;
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

                //Update-увај ги оние properties за конфигурирање на мерењата
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.TestCurrent = datasource.TestCurrentDcCooling;
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.NoOfSamples = datasource.NoOfSamplesDcCooling;
                datasource.Root.DcCoolingMeasurenments.SelectedDcColdChannel.Value = datasource.SelectedChannel;
                datasource.Root.DcCoolingMeasurenments.R1Cold.Value = datasource.R1ColdAtDcCool;
                datasource.Root.DcCoolingMeasurenments.R2Cold.Value = datasource.R2ColdAtDcCool;
                datasource.evaluateTemperatureSettingsDcCooling();
                datasource.Root.DcCoolingMeasurenments.EndAcTemp.Value = datasource.EndAcTemp;
                datasource.Root.DcCoolingMeasurenments.KDropInOil.Value = datasource.KDropInOil;
                datasource.Root.DcCoolingMeasurenments.IsTempDataMeasured.Value = datasource.IsTempMeasured;
                datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.SampleRate = datasource.SampleRateDcCooling;
                //

                //Стартувај го мерењето на отпор
                ds = new DataSourceLayer.DataSourceServices();
                ds.RessistanceMeasurenmentFinished += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentFinishedEvent(ds_CoolRessistanceMeasurenmentFinished);
                ds.RessistanceMeasurenmentsError += new DataSourceLayer.DataSourceServices.RessistanceMeasurenmentsErrorEvent(ds_RessistanceMeasurenmentsError);
                ds.start_RessistanceMeasurenment(datasource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel, IS_TEST, false);
                save();
            }
            else
            {
                StartDcCoolResButton.IsChecked = false;
            }
        }
        private void StartDcCoolResButton_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDcCoolResButton.IsChecked == true && currentProcessState == ProcessStatesEnum.DcCoolRes)
            {
                ds.stopRessistanceMeasurenments();
                // Call the finished handler manualy
                ds_ColdRessistanceMeasurenmentFinished();
            }
        }
        public void ds_CoolRessistanceMeasurenmentFinished()
        {
            datasource.calculateResults();
            this.ds_ColdRessistanceMeasurenmentFinished();
            dcCoolingGraphsRefresh();
            save();
        }

        private void reduceButton_Checked(object sender, RoutedEventArgs e)
        {
            ds.IsSampleReduced = true;
            ds.IsTempMeasInterupted = true;
            reduceButton.IsEnabled = false;
        }

        private void TNullButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Double.IsNaN(datasource.R1ColdAtDcCool) || Double.IsNaN(datasource.R2ColdAtDcCool))
            {
                MessageBox.Show("Немате извршено мерење на отпори на ладно", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
                TNullButton.IsChecked = false;
                return;

            }
            if (Double.IsNaN(datasource.TCold))
            {
                MessageBox.Show("Немате извршено мерење на температури на ладно", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
                TNullButton.IsChecked = false;
                return;
            }
            if (Double.IsNaN(datasource.EndAcTemp) || Double.IsNaN(datasource.KDropInOil))
            {
                MessageBox.Show("Немате извршено мерење на температури при загревање", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
                TNullButton.IsChecked = false;
                return;
            }

            datasource.Root.DcCoolingMeasurenments.TNullTime = DateTime.Now;
            TNullButton.IsEnabled = false;
        }

        /// <summary>
        /// При Validation Error на FrameworkElement компонента,
        /// да се врати последната валидна вредност (вредноста во binding source-от)
        /// </summary>
        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            //Стави Null Validation.ErrorTEmplate за да не покажува црвена граница кај TextBox
            System.Windows.Controls.Validation.SetErrorTemplate(((FrameworkElement)e.OriginalSource), null);
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

        /// <summary>
        /// Стартување на StartUpWindow
        /// </summary>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            /*
             StartUpWindow stp = new StartUpWindow();
            if ((bool)stp.ShowDialog())
            {
                int i = 0;
                switch (stp.fileCommand)
                {
                    case FileCommand.New: this.CommandBindings[0].Command.Execute(null); break;
                    case FileCommand.Open: this.CommandBindings[1].Command.Execute(null); break;
                }
                //Show Transformator Properties
                this.CommandBindings[6].Command.Execute(null);
            }
            else
                this.Close();*/
        }

        private void CalibrateTemperatureMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TempCalibration t = new TempCalibration();
            t.ShowDialog();
        }

        private void CalibrateRessistanceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResssistanceCalibration r = new ResssistanceCalibration();
            r.ShowDialog();
        }
        
        /// <summary>
        /// Се користи за автоматско зачувување (при мерења и сл.) на тековниот фајл.
        /// </summary>
        private void save()
        {
            this.CommandBindings[2].Command.Execute(null);
        }

        /// <summary>
        /// Refresh на сите графици и полиња кои се пресметуваат.
        /// Се користи при open на remo документ.
        /// </summary>
        private void refreshAllDependentForms()
        {
            acGraphRefresh();
            dcCoolingGraphsRefresh();
        }

        private void MeasuredRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (datasource != null)
            {
                datasource.IsTempMeasured = true;
            }
        }

        private void ManuelRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (datasource != null)
            {
                datasource.IsTempMeasured = false;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ds != null)
            {
                ds.stopRessistanceMeasurenments();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
   
}
