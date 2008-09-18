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
        public void start_TempMeasurenment(int sampleRate, int numberOfSamples, List<TempMeasurenment> tempMeasurenments)
        {
            this._sampleRate = sampleRate;
            this._numberOfSamples = numberOfSamples;
            this._tempMeasurenments = tempMeasurenments;

            measurenmentThread = new Thread(tempMeasurenmentDoWork);
            measurenmentThread.Start();
        }
        /// <summary>
        /// Стартување на мерење на отпори. Оваа метода веднаш враќа и ја врши стартува нов Thread, во кој се врши мерењето.
        /// </summary>
        /// <param name="sampleRate">sampleRate во секунди</param>
        /// <param name="numberOfSamples"></param>
        /// <param name="tempMeasurenments"></param>
        public void start_RessistanceMeasurenment(int sampleRate, int numberOfSamples, List<RessistanceMeasurenment> ressistanceMeasurenments)
        {
            _sampleRate = sampleRate;
            _numberOfSamples = numberOfSamples;
            _ressistanceMeasurenments = ressistanceMeasurenments;

            measurenmentThread = new Thread(ressistanceMeasurenmentDoWork);
            measurenmentThread.Start();
        }

        private Thread measurenmentThread;

        private int _sampleRate;
        private int _numberOfSamples;
        private List<TempMeasurenment> _tempMeasurenments;
        private List<RessistanceMeasurenment> _ressistanceMeasurenments;

        private void tempMeasurenmentDoWork()
        {
            if (_tempMeasurenments == null)
                _tempMeasurenments = new List<TempMeasurenment>();
            lock (_tempMeasurenments)
            {
                _tempMeasurenments.Clear();
            }
            Random rand = new Random();
            for (int i = 0; i < _numberOfSamples; i++)
            {
                Thread.Sleep(_sampleRate * 1000);
                _tempMeasurenments.Add(new TempMeasurenment(
                    DateTime.Now, 
                    rand.NextDouble() * 10 + 20,
                    rand.NextDouble() * 10 + 20,
                    rand.NextDouble() * 10 + 20,
                    rand.NextDouble() * 10 + 20)
                    );
            }
            //throw event
            OnTempMeasurenmentFinished();
        }

        private void ressistanceMeasurenmentDoWork()
        {
            if (_ressistanceMeasurenments == null)
                _ressistanceMeasurenments = new List<RessistanceMeasurenment>();
            lock (_ressistanceMeasurenments)
            {
                _ressistanceMeasurenments.Clear();
            }
            Random rand = new Random();
            int channel = 1;
            for (int i = 0; i < _numberOfSamples; i++)
            {
                if (i % 2 == 0)
                    channel = 1;
                else
                    channel = 2;
                Thread.Sleep(_sampleRate * 1000);
                _ressistanceMeasurenments.Add(
                    new RessistanceMeasurenment(DateTime.Now, channel, rand.NextDouble() * 10, rand.NextDouble() * 10) );
            }
            //throw event
            OnRessistanceMeasurenmentFinished();
        }

        
    }
}
