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
            if (TempMeasurenmentFinished != null)
                TempMeasurenmentFinished();
        }
        private void OnRessistanceMeasurenmentFinished()
        {
            if (RessistanceMeasurenmentFinished != null)
                RessistanceMeasurenmentFinished();
        }

        /// <summary>
        /// Стартување на мерење на температури. Оваа метода веднаш враќа и ја врши стартува нов Thread ,ов кој се врши мерењето.
        /// </summary>
        /// <param name="sampleRate"></param>
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
        /// Стартување на мерење на отпори. Оваа метода веднаш враќа и ја врши стартува нов Thread ,ов кој се врши мерењето.
        /// </summary>
        /// <param name="sampleRate"></param>
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
            for (int i = 0; i < _numberOfSamples; i++)
            {
                Thread.Sleep(_sampleRate * 1000);
                _tempMeasurenments.Add(new TempMeasurenment(
                    DateTime.Now,
                    new Random().NextDouble() * 10 + 20,
                    new Random().NextDouble() * 10 + 20,
                    new Random().NextDouble() * 10 + 20,
                    new Random().NextDouble() * 10 + 20)
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
            for (int i = 0; i < _numberOfSamples; i++)
            {
                Thread.Sleep(_sampleRate * 1000);
                _ressistanceMeasurenments.Add(new RessistanceMeasurenment(
                    DateTime.Now,
                    1,
                    new Random().Next(1, 10), 
                    new Random().Next(1, 5))
                    );
            }
            //throw event
            OnRessistanceMeasurenmentFinished();
        }

        
    }
}
