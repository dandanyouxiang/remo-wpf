using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityLayer;
using System.Threading;

namespace DataSourceLayer
{

    public class DataSourceServices
    {
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за завршено мерење на температури
        /// </summary>
        public delegate void TempMeasurenmentFinishedEventHandler();
        /// <summary>
        /// Delegate за EventHandler кој дава Notification за завршено мерење на отпори
        /// </summary>
        public delegate void RessistanceMeasurenmentFinishedEventHandler();

        /// <summary>
        /// EventHandler кој дава Notification за завршено мерење на температури
        /// </summary>
        public event TempMeasurenmentFinishedEventHandler TempMeasurenmentFinished;
        /// <summary>
        /// EventHandler кој дава Notification за завршено мерење на отпори
        /// </summary>
        public event RessistanceMeasurenmentFinishedEventHandler RessistanceMeasurenmentFinished;

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

        /// <summary>
        /// Стартување на мерење на температури. Оваа метода веднаш враќа и ја врши стартува нов Thread, во кој се врши мерењето.
        /// </summary>
        /// <param name="sampleRate">sampleRate во секунди</param>
        /// <param name="numberOfSamples"></param>
        /// <param name="tempMeasurenments"></param>
        public void start_TempMeasurenment(EntityLayer.TempMeasurenementConfiguration tempMeasurenmentConfiguration)
        {
            this._sampleRate = tempMeasurenmentConfiguration.TempSampleRateCurrentState;
            this._numberOfSamples = tempMeasurenmentConfiguration.TempNoOfSamplesCurrentState;
            this._tempMeasurenementConfiguration = tempMeasurenmentConfiguration;

            IsTempMeasInterupted = false;
            IsSampleReduced = false;
            IsTempMeasStopped = false;
            measurenmentThread = new Thread(tempMeasurenmentDoWork);
            measurenmentThread.Start();
        }
        /// <summary>
        /// Стартување на мерење на отпори. Оваа метода веднаш враќа и ја врши стартува нов Thread, во кој се врши мерењето.
        /// </summary>
        /// <param name="sampleRate">sampleRate во секунди</param>
        /// <param name="numberOfSamples"></param>
        /// <param name="tempMeasurenments"></param>
        public void start_RessistanceMeasurenment(EntityLayer.RessistanceTransformerChannel ressistanceTransformerChannel)
        {
            _sampleRate = ressistanceTransformerChannel.RessistanceSampleRateCurrentState;
            _numberOfSamples = ressistanceTransformerChannel.RessistanceNoOfSamplesCurrentState;
            _current = ressistanceTransformerChannel.TestCurrent;

            _ressistanceTransformerChannel = ressistanceTransformerChannel;

            measurenmentThread = new Thread(ressistanceMeasurenmentDoWork);
            measurenmentThread.Start();
        }

        private Thread measurenmentThread;

        private int _sampleRate;
        private int _numberOfSamples;
        private double _current;
        private EntityLayer.TempMeasurenementConfiguration _tempMeasurenementConfiguration;
        private EntityLayer.RessistanceTransformerChannel _ressistanceTransformerChannel;

        public bool IsTempMeasInterupted { get; set; }
        public bool IsSampleReduced { get; set; }
        public bool IsTempMeasStopped { get; set; }

        private void tempMeasurenmentDoWork()
        {
            lock (_tempMeasurenementConfiguration.TempMeasurenments)
            {
                _tempMeasurenementConfiguration.TempMeasurenments.Clear();
                Random rand = new Random();
                for (int i = 0; i < _numberOfSamples && !IsTempMeasStopped; i++)
                {
                    _tempMeasurenementConfiguration.TempMeasurenments.Add(new TempMeasurenment(
                        DateTime.Now,
                        _tempMeasurenementConfiguration.IsChannel1On ? rand.NextDouble() * 10 + 20 : double.NaN,
                        _tempMeasurenementConfiguration.IsChannel2On ? rand.NextDouble() * 10 + 20 : double.NaN,
                        _tempMeasurenementConfiguration.IsChannel3On ? rand.NextDouble() * 10 + 20 : double.NaN,
                        _tempMeasurenementConfiguration.IsChannel4On ? rand.NextDouble() * 10 + 20 : double.NaN
                        ));
                    _tempMeasurenementConfiguration.TempMeasurenments[i].IsSampleReduced = this.IsSampleReduced;
                    this.IsSampleReduced = false;
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
                    _ressistanceTransformerChannel.RessistanceMeasurenments.Add(
                        new RessistanceMeasurenment(DateTime.Now, channel, rand.NextDouble() * 10, rand.NextDouble() * 10));
                    Thread.Sleep(_sampleRate * 1000);
                }
            }
            //throw event
            OnRessistanceMeasurenmentFinished();
        }

        
    }
}
