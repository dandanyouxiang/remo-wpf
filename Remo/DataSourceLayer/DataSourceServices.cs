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
        private double _current;
        private EntityLayer.TempMeasurenementConfiguration _tempMeasurenementConfiguration;
        private EntityLayer.RessistanceTransformerChannel _ressistanceTransformerChannel;
        private bool isTest;
        private bool isColdMeas;
        private double CURRENT_ODNOS = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CURRENT_ODNOS"]);
        //Instruments conf
        private RessistanceDataSource rds;
        private TemperatureDataSource tds;
        private string IP_ADDRESS_VOLTAGE;
        private string IP_ADDRESS_CURRENT;
        private int PORT_VOLTAGE;
        private int PORT_CURRENT;
        //PLC conf
        private int PLC_PORT_NUMBER;
        private short NO_OF_TRIGGERS_ADDRESS = 1;
        private short START_MEAS_ADDRESS = 2;
        private short TEST_CURRENT_ADDRESS = 9;

        private ProcessManager pm;

        private int channelNo = 1;
        private bool isFirstMeasurenment = true;
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
        public delegate void TempMeasurenmentDoneEvent();
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за едно завршено мерење на отпор
        /// </summary>
        public delegate void RessistanceMeasurenmentDoneEvent();
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
        private void OnTempMeasurenmentDone()
        {
            //Call this method on the Right Thread
            if (TempMeasurenmentDone != null)
                ((System.Windows.Threading.DispatcherObject)TempMeasurenmentDone.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, TempMeasurenmentDone);
        }
        private void OnRessistanceMeasurenmentDone()
        {
            //Call this method on the Right Thread
            if (RessistanceMeasurenmentDone != null)
                ((System.Windows.Threading.DispatcherObject)RessistanceMeasurenmentDone.Target).Dispatcher.
                    BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, RessistanceMeasurenmentDone);
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
            _sampleRate = ressistanceTransformerChannel.RessistanceSampleRateCurrentState;
            _numberOfSamples = 2 * ressistanceTransformerChannel.RessistanceNoOfSamplesCurrentState;
            _current = ressistanceTransformerChannel.TestCurrent;
            _ressistanceTransformerChannel = ressistanceTransformerChannel;
            this.isTest = isTest;
            this.isColdMeas = isColdMeas;
            if (!isTest)
            {
                pm = new ProcessManager((PLCP.enPortNumber)PLC_PORT_NUMBER);
                
                SetMemBitTask startMeasTask = new SetMemBitTask("start meas task", START_MEAS_ADDRESS);
                WriteMemIntTask writeNoOfTriggersTask = new WriteMemIntTask("writeNoOfTriggersTask", NO_OF_TRIGGERS_ADDRESS, (short)(_numberOfSamples + 1));
                WriteMemIntTask writeTestCurrentTask = new WriteMemIntTask("writeTestCurrentTask", TEST_CURRENT_ADDRESS, (short)(_current * 10));
                pm.addPlcTask(startMeasTask);
                pm.addPlcTask(writeNoOfTriggersTask);
                pm.addPlcTask(writeTestCurrentTask);
                
                rds = new RessistanceDataSource(_sampleRate, _numberOfSamples, _current, IP_ADDRESS_VOLTAGE, IP_ADDRESS_CURRENT, PORT_VOLTAGE, PORT_CURRENT);
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
            if (RessistanceMeasurenmentsError != null)
                RessistanceMeasurenmentsError();
        }
        public void rsd_MeasurenmentDone(double voltage, double current, int measNumber)
        {
            if (!isFirstMeasurenment)
            {
                double ressMeasured = voltage / (current * CURRENT_ODNOS);
                double ressCorrected = intCal.interpolateRessistance(ressMeasured);
                lock (_ressistanceTransformerChannel.RessistanceMeasurenments)
                {
                    Console.WriteLine("voltage:" + voltage);
                    Console.WriteLine("current:" + current);
                    Console.WriteLine("ressMeasured:" + ressMeasured);
                    Console.WriteLine("ressCorrected:" + ressCorrected);
                    _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                           new RessistanceMeasurenment(DateTime.Now, channelNo, voltage * ressCorrected / ressMeasured, current * CURRENT_ODNOS));
                    OnRessistanceMeasurenmentDone();
                }
                if (channelNo == 1)
                    channelNo = 2;
                else
                    channelNo = 1;
            }
            isFirstMeasurenment = !isFirstMeasurenment;
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
                tds.MeasurenmentDone+=new TemperatureDataSource.MeasurenmentDoneEvent(tds_MeasurenmentDone);
                tds.MeasurenmentEnd+=new TemperatureDataSource.MeasurenmentEndEvent(tds_MeasurenmentEnd);
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
            _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(time, t1Corrected, t2Corrected, t3Corrected, t4Corrected));
            OnTempMeasurenmentDone();
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
                for (int i = 0; i < _numberOfSamples && !IsTempMeasStopped; i++)
                {
                    if (isColdMeas)
                    {
                        _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(
                            DateTime.Now,
                            _tempMeasurenementConfiguration.IsChannel1On ? 20.2 : double.NaN,
                            _tempMeasurenementConfiguration.IsChannel2On ? 20.2 : double.NaN,
                            _tempMeasurenementConfiguration.IsChannel3On ? 20.2 : double.NaN,
                            _tempMeasurenementConfiguration.IsChannel4On ? 20.2 : double.NaN
                        ));
                    }
                    else
                    {
                        _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(
                                DateTime.Now,
                                _tempMeasurenementConfiguration.IsChannel1On ? rand.NextDouble() * 10 + 20 : double.NaN,
                                _tempMeasurenementConfiguration.IsChannel2On ? rand.NextDouble() * 10 + 20 : double.NaN,
                                _tempMeasurenementConfiguration.IsChannel3On ? rand.NextDouble() * 10 + 20 : double.NaN,
                                _tempMeasurenementConfiguration.IsChannel4On ? rand.NextDouble() * 10 + 20 : double.NaN
                                ));
                    }
                    _tempMeasurenementConfiguration.TempMeasurenments[i].IsSampleReduced = this.IsSampleReduced;
                    this.IsSampleReduced = false;
                    OnTempMeasurenmentDone();
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
                    if (isColdMeas)
                        _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                            new RessistanceMeasurenment(DateTime.Now, channel, 0.82293996, 1));
                    else
                        _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                            new RessistanceMeasurenment(DateTime.Now, channel, testValues[(int)(i / 2)], 1));
                    OnRessistanceMeasurenmentDone();
                    Thread.Sleep(_sampleRate * 1000);
                }
            }
            //throw event
            OnRessistanceMeasurenmentFinished();
        }
        #endregion

    }
}
