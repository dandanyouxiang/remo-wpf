using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityLayer;
using System.Threading;
using FlukeClient;
using PlcProcess;

namespace DataSourceLayer
{

    public class DataSourceServices
    {
        #region fields
        private Thread measurenmentThread;
        private int _sampleRate;
        private int _numberOfSamples;
        private double _testCurrent;
        private EntityLayer.TempMeasurenementConfiguration _tempMeasurenementConfiguration;
        private EntityLayer.RessistanceTransformerChannel _ressistanceTransformerChannel;
        private bool isTest;
        private bool isColdMeas;
        private double CURRENT_ODNOS1 = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CURRENT_ODNOS1"]);
        private double CURRENT_ODNOS2 = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CURRENT_ODNOS2"]);
        private double LIMIT_CURRENT1 = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["LIMIT_CURRENT1"]);
        /// <summary>
        /// Односот кај ќе се примени на тековното мерење. Се бира според избраната струја на мерење.
        /// </summary>
        private double _currentOdnos;

        //Instruments conf
        private RessistanceDataSource rds;
        private TemperatureDataSource tds;
        private string IP_ADDRESS_VOLTAGE;
        private string IP_ADDRESS_CURRENT;
        private int PORT_VOLTAGE;
        private int PORT_CURRENT;
        //PLC conf
        private int PLC_PORT_NUMBER;
        private short SAMPLE_RATE_ADDRESS = 3;
        private short NO_OF_TRIGGERS_ADDRESS = 1;
        private short START_MEAS_ADDRESS = 2;
        private short TEST_CURRENT_ADDRESS = 9;

        private ProcessManager pm;

        private int channelNo = 1;
        private DataAccessLayer.InterpolationCal intCal;
        #endregion

        #region Properties
        public bool IsTempMeasInterupted { get; set; }
        public bool IsSampleReduced { get; set; }
        public bool IsTempMeasStopped { get; set; }
        #endregion

        #region events
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за завршено мерење на температури
        /// </summary>
        public delegate void TempMeasurenmentFinishedEvent();
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за завршено мерење на отпори
        /// </summary>
        public delegate void RessistanceMeasurenmentFinishedEvent();
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за напревено едно мерење на температурите
        /// </summary>
        public delegate void TempMeasurenmentDoneEvent(List<double> correctedTemps, List<double> realTemps);
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за едно завршено мерење на отпор
        /// </summary>
        public delegate void RessistanceMeasurenmentDoneEvent(int channelNo, double correctedRessistance, double realRessistance);
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за појава на грешка при мерење на отпор.
        /// </summary>
        public delegate void RessistanceMeasurenmentsErrorEvent();

        public event TempMeasurenmentFinishedEvent TempMeasurenmentFinished;
        public event RessistanceMeasurenmentFinishedEvent RessistanceMeasurenmentFinished;
        public event TempMeasurenmentDoneEvent TempMeasurenmentDone;
        public event RessistanceMeasurenmentDoneEvent RessistanceMeasurenmentDone;
        public event RessistanceMeasurenmentsErrorEvent RessistanceMeasurenmentsError;

        private void OnTempMeasurenmentFinished()
        {
            //Call this method on the Right Thread
            if (TempMeasurenmentFinished != null)
                ((System.Windows.Threading.DispatcherObject)TempMeasurenmentFinished.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, TempMeasurenmentFinished);
        }
        private void OnRessistanceMeasurenmentFinished()
        {
            //Call this method on the Right Thread
            if (RessistanceMeasurenmentFinished != null)
                ((System.Windows.Threading.DispatcherObject)RessistanceMeasurenmentFinished.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, RessistanceMeasurenmentFinished);
        }
        private void OnTempMeasurenmentDone(List<double> correctedTemps, List<double> realTemps)
        {
            //Call this method on the Right Thread
            if (TempMeasurenmentDone != null)
                ((System.Windows.Threading.DispatcherObject)TempMeasurenmentDone.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, TempMeasurenmentDone, correctedTemps, realTemps);
        }
        private void OnRessistanceMeasurenmentDone(int channelNo, double correctedRessistance, double realRessistance)
        {
            //Call this method on the Right Thread
            if (RessistanceMeasurenmentDone != null)
                ((System.Windows.Threading.DispatcherObject)RessistanceMeasurenmentDone.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, RessistanceMeasurenmentDone, channelNo, correctedRessistance, realRessistance);
        }
        #endregion

        public DataSourceServices()
        {
            PLC_PORT_NUMBER = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PLC_PORT_NUMBER"]);
            IP_ADDRESS_VOLTAGE = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IP_ADDRESS_VOLTAGE"]);
            IP_ADDRESS_CURRENT = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IP_ADDRESS_CURRENT"]);
            PORT_VOLTAGE = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PORT_VOLTAGE"]);
            PORT_CURRENT = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["PORT_CURRENT"]);
            intCal = new DataAccessLayer.InterpolationCal();
        }

        #region ressistance measurenments
        /// <summary>
        /// Стартување на мерење на отпори. Оваа метода веднаш враќа и ја врши стартува нов Thread, во кој се врши мерењето.
        /// </summary>
        public void start_RessistanceMeasurenment(EntityLayer.RessistanceTransformerChannel ressistanceTransformerChannel, bool isTest, bool isColdMeas)
        {
            _sampleRate = ressistanceTransformerChannel.SampleRate;
            _numberOfSamples =  2 * ressistanceTransformerChannel.NoOfSamples;
            _testCurrent = ressistanceTransformerChannel.TestCurrent;
            _ressistanceTransformerChannel = ressistanceTransformerChannel;
            this.isTest = isTest;

            //Избор на односот на шантот со кој се мери,
            //според избраната струја.
            if (_testCurrent <= LIMIT_CURRENT1)
                _currentOdnos = CURRENT_ODNOS1;
            else
                _currentOdnos = CURRENT_ODNOS2;
            this.isColdMeas = isColdMeas;
            if (!isTest)
            {
                pm = new ProcessManager((PLCP.enPortNumber)PLC_PORT_NUMBER);

                WriteMemIntTask writeSampleRateTask = new WriteMemIntTask("writeSampleRateTask", SAMPLE_RATE_ADDRESS, (short)(_sampleRate * 100 - 50));
                WriteMemIntTask writeNoOfTriggersTask = new WriteMemIntTask("writeNoOfTriggersTask", NO_OF_TRIGGERS_ADDRESS, (short)(_numberOfSamples + 1));
                WriteMemIntTask writeTestCurrentTask = new WriteMemIntTask("writeTestCurrentTask", TEST_CURRENT_ADDRESS, (short)(_testCurrent * 10));
                SetMemBitTask startMeasTask = new SetMemBitTask("start meas task", START_MEAS_ADDRESS);

                pm.addPlcTask(writeSampleRateTask);
                pm.addPlcTask(writeNoOfTriggersTask);
                pm.addPlcTask(writeTestCurrentTask); 
                pm.addPlcTask(startMeasTask);

                rds = new RessistanceDataSource(_sampleRate, _numberOfSamples, _testCurrent, IP_ADDRESS_VOLTAGE, IP_ADDRESS_CURRENT, PORT_VOLTAGE, PORT_CURRENT);
                rds.MeasurenmentDone += new RessistanceDataSource.MeasurenmentDoneEvent(rsd_MeasurenmentDone);
                rds.MeasurenmentsEnd+=new RessistanceDataSource.MeasurenmentsEndEvent(rds_MeasurenmentsEnd);
                _ressistanceTransformerChannel.RessistanceMeasurenments.Clear();
                rds.MeasurenmentError+=new RessistanceDataSource.MeasurenmentErrorEvent(rds_MeasurenmentError);
                rds.start_RessistanceMeasurenments();
                pm.start();
            }
            else
            {
                measurenmentThread = new Thread(ressistanceMeasurenmentDoWork);
                measurenmentThread.Start();
            }
        }
        public void rds_MeasurenmentError()
        {
            //Call this method on the Right Thread
            if (RessistanceMeasurenmentsError != null)
                ((System.Windows.Threading.DispatcherObject)RessistanceMeasurenmentFinished.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, RessistanceMeasurenmentsError);
        }
        public void rsd_MeasurenmentDone(double voltage, double current, int measNumber)
        {

            //За да инструментите имаат време да го стабилизираат мерениот напон
            //Потребно е да се исфрлат првите семплови
            double ressMeasured = voltage / (current * _currentOdnos);
            //Основна корекција
            double ressBaseCorrected = intCal.interpolateBaseRessistance(ressMeasured);
            //Корекција
            double ressCorrected = intCal.interpolateRessistance(ressBaseCorrected);
            lock (_ressistanceTransformerChannel.RessistanceMeasurenments)
            {
                Console.WriteLine("voltage:" + voltage);
                Console.WriteLine("current:" + current);
                Console.WriteLine("ressMeasured:" + ressMeasured);
                Console.WriteLine("ressBaseCorrected:" + ressBaseCorrected);
                Console.WriteLine("ressCorrected:" + ressCorrected);

                //Се прави корекција на струјата
                double currentCorrected = current * _currentOdnos * ressMeasured / ressCorrected;
                _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                       new RessistanceMeasurenment(DateTime.Now, channelNo, voltage, currentCorrected));
                OnRessistanceMeasurenmentDone(channelNo, ressCorrected, ressBaseCorrected);

            }
            if (channelNo == 1)
                channelNo = 2;
            else
                channelNo = 1;
        }
       
        public void stopRessistanceMeasurenments()
        {
            if (rds != null)
            {
                rds.stopRessistanceMeasurenments();
                ResetMemBitTask stopRessMeasTask = new ResetMemBitTask("Stop meas", START_MEAS_ADDRESS);
                stopRessMeasTask.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(stopRessMeasTask_TaskExecutedEvent);
                pm.addPlcTask(stopRessMeasTask);
            }
        }
        /// <summary>
        /// Крај на мерењата
        /// </summary>
        public void rds_MeasurenmentsEnd()
        {
            ResetMemBitTask stopRessMeasTask = new ResetMemBitTask("Stop meas", START_MEAS_ADDRESS);
            stopRessMeasTask.TaskExecutedEvent += new PlcRWTask.TaskExecutedEventHandler(stopRessMeasTask_TaskExecutedEvent);
            pm.addPlcTask(stopRessMeasTask);
            OnRessistanceMeasurenmentFinished();
        }
        public void stopRessMeasTask_TaskExecutedEvent(object sender, EventArgs e)
        {
            if (pm != null)
                pm.stop();
        }
        #endregion

        #region temperature measurenments
        /// <summary>
        /// Стартување на мерење на температури. Оваа метода веднаш враќа и ја врши стартува нов Thread, во кој се врши мерењето.
        /// </summary>
        public void start_TempMeasurenment(EntityLayer.TempMeasurenementConfiguration tempMeasurenmentConfiguration, bool isTest, bool isColdMeas)
        {
            this._sampleRate = tempMeasurenmentConfiguration.TempSampleRateCurrentState;
            this._numberOfSamples = tempMeasurenmentConfiguration.TempNoOfSamplesCurrentState;
            this._tempMeasurenementConfiguration = tempMeasurenmentConfiguration;
            this.isTest = isTest;
            IsTempMeasInterupted = false;
            IsSampleReduced = false;
            IsTempMeasStopped = false;
            this.isColdMeas = isColdMeas;
            if (!isTest)
            {
                pm = new ProcessManager((PLCP.enPortNumber)PLC_PORT_NUMBER);
                tds = new TemperatureDataSource(pm, _sampleRate, _numberOfSamples);
                _tempMeasurenementConfiguration.TempMeasurenments.Clear();
                tds.MeasurenmentDone += new TemperatureDataSource.MeasurenmentDoneEvent(tds_MeasurenmentDone);
                tds.MeasurenmentEnd += new TemperatureDataSource.MeasurenmentEndEvent(tds_MeasurenmentEnd);
                tds.startTempMeasurenments();
            }
            else
            {
                measurenmentThread = new Thread(tempMeasurenmentDoWork);
                measurenmentThread.Start();
            }
        }
        /// <summary>
        /// Едно температурни мерење е завршено
        /// </summary>
        public void tds_MeasurenmentDone(double t1, double t2, double t3, double t4)
        {
            double t1Corrected = _tempMeasurenementConfiguration.IsChannel1On ? intCal.interpolateT1(t1) : Double.NaN;
            double t2Corrected = _tempMeasurenementConfiguration.IsChannel2On ? intCal.interpolateT2(t2) : Double.NaN;
            double t3Corrected = _tempMeasurenementConfiguration.IsChannel3On ? intCal.interpolateT3(t3) : Double.NaN;
            double t4Corrected = _tempMeasurenementConfiguration.IsChannel4On ? intCal.interpolateT4(t4) : Double.NaN;
            DateTime time = DateTime.Now;
            _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(time, t1Corrected, t2Corrected, t3Corrected, t4Corrected) { IsSampleReduced = this.IsSampleReduced});
            IsSampleReduced = false;
            OnTempMeasurenmentDone(new List<double>() { t1Corrected, t2Corrected, t3Corrected, t4Corrected }, new List<double>() { t1, t2, t3, t4 });
        }
        /// <summary>
        /// Крај на температурните мерења
        /// </summary>
        public void tds_MeasurenmentEnd()
        {
            this.OnTempMeasurenmentFinished();
        }
        /// <summary>
        /// Стопирање на температурните мерења
        /// </summary>
        public void stopTempMeasurenments()
        {
            if(isTest)
                IsTempMeasStopped = true;
            else
                tds.stopTempMeasurenments();
        }

        #endregion

        #region test methods
        private void tempMeasurenmentDoWork()
        {
            lock (_tempMeasurenementConfiguration.TempMeasurenments)
            {
                _tempMeasurenementConfiguration.TempMeasurenments.Clear();
                Random rand = new Random();
                double t1, t2, t3, t4;
                for (int i = 0; i < _numberOfSamples && !IsTempMeasStopped; i++)
                {
                    if (isColdMeas)
                    {
                        t1 = _tempMeasurenementConfiguration.IsChannel1On ? 20.2 : double.NaN;
                        t2 = _tempMeasurenementConfiguration.IsChannel2On ? 20.2 : double.NaN;
                        t3 = _tempMeasurenementConfiguration.IsChannel3On ? 20.2 : double.NaN;
                        t4 = _tempMeasurenementConfiguration.IsChannel4On ? 20.2 : double.NaN;
                        _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(
                            DateTime.Now, t1, t2, t3, t4));
                    }
                    else
                    {
                        t1 = _tempMeasurenementConfiguration.IsChannel1On ? rand.NextDouble() * 10 + 20 : double.NaN;
                        t2 = _tempMeasurenementConfiguration.IsChannel2On ? rand.NextDouble() * 10 + 20 : double.NaN;
                        t3 = _tempMeasurenementConfiguration.IsChannel3On ? rand.NextDouble() * 10 + 20 : double.NaN;
                        t4 = _tempMeasurenementConfiguration.IsChannel4On ? rand.NextDouble() * 10 + 20 : double.NaN;
                        _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(
                                DateTime.Now, t1, t2, t3, t4));
                    }
                    _tempMeasurenementConfiguration.TempMeasurenments[i].IsSampleReduced = this.IsSampleReduced;
                    this.IsSampleReduced = false;
                    double t1Corrected = _tempMeasurenementConfiguration.IsChannel1On ? intCal.interpolateT1(t1) : Double.NaN;
                    double t2Corrected = _tempMeasurenementConfiguration.IsChannel2On ? intCal.interpolateT2(t2) : Double.NaN;
                    double t3Corrected = _tempMeasurenementConfiguration.IsChannel3On ? intCal.interpolateT3(t3) : Double.NaN;
                    double t4Corrected = _tempMeasurenementConfiguration.IsChannel4On ? intCal.interpolateT4(t4) : Double.NaN;
                    OnTempMeasurenmentDone(new List<double>() { t1Corrected, t2Corrected, t3Corrected, t4Corrected }, new List<double>() { t1, t2, t3, t4 });
                    for (int j = 0; j < _sampleRate && !IsTempMeasInterupted && !IsTempMeasStopped && i < _numberOfSamples - 1; j++)
                        Thread.Sleep(1000);
                    IsTempMeasInterupted = false;
                    if (IsTempMeasStopped)
                        break;
                }
                IsTempMeasStopped = false;
            }
            //throw event
            OnTempMeasurenmentFinished();
        }

        private void ressistanceMeasurenmentDoWork()
        {
            List<double> testValues = new List<double>()
            {0.99632, 0.9961, 0.99603, 0.99584, 0.99584, 0.99556, 0.99527, 0.99523, 0.99527, 0.99517, 0.99477, 0.99488, 0.99470, 0.99463, 0.99443, 0.99445, 0.99419, 0.99408, 0.99388, 0.99386, 0.99373, 0.99369};
            
            lock (_ressistanceTransformerChannel.RessistanceMeasurenments)
            {
                _ressistanceTransformerChannel.RessistanceMeasurenments.Clear();
                Random rand = new Random();
                int channel = 1;
                for (int i = 0; i < _numberOfSamples; i++)
                {
                    if (i % 2 == 0)
                        channel = 1;
                    else
                        channel = 2;

                    double ressReal;
                    if (isColdMeas)
                    {
                        ressReal = 0.82293996;
                    }
                    else
                    {
                        ressReal = testValues[(int)(i / 2)];
                    }
                    //Додади го мерењето во Entity објектите
                     _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                            new RessistanceMeasurenment(DateTime.Now, channel, ressReal, 1));
                    OnRessistanceMeasurenmentDone(1, ressReal, ressReal);
                    Thread.Sleep(_sampleRate * 1000);
                }
            }
            //throw event
            OnRessistanceMeasurenmentFinished();
        }
        #endregion

    }
}
