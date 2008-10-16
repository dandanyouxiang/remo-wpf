using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FlukeClient
{
    public class RessistanceDataSource
    {
        private int _sampleRate;
        private int _numberOfSamples;
        private double _testCurrent;

        private string _ipAddress1;
        private string _ipAddress2;
        private int _port1;
        private int _port2;
        private Thread meas1Thread;
        private Thread meas2Thread;
        private int measNumber;
        private List<double> voltageMeas;
        private List<double> currentMeas;

        public delegate void MeasurenmentDoneEvent(double voltage, double current);

        /// <summary>
        /// Завршено е едно мерење на отпор.
        /// </summary>
        public event MeasurenmentDoneEvent MeasurenmentDone;

        public RessistanceDataSource(int sampleRate, int numberOfSamples, double current, string ipAddress1, string ipAddress2, int port1, int port2)
        {
            _sampleRate = sampleRate;
            _numberOfSamples = numberOfSamples;
            _testCurrent = current;
            _ipAddress1 = ipAddress1;
            _ipAddress2 = ipAddress2;
            _port1 = port1;
            _port2 = port2;
            measNumber = 0;

        }

        public void start_RessistanceMeasurenments()
        {
            voltageMeas = new List<double>();
            currentMeas = new List<double>();

           
            meas2Thread = new Thread(meas2);
            meas2Thread.Start();
            meas1Thread = new Thread(meas1);
            meas1Thread.Start();
        }
        public void stopRessistanceMeasurenments()
        {
            meas1Thread.Abort();
            meas2Thread.Abort();
        }

        private void meas1()
        {
            FlukeMeasurenmentClient meas1 = new FlukeMeasurenmentClient(_ipAddress1, _port1, 2 * _numberOfSamples);
            meas1.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas1_MeasFinished);
            meas1.startMeasurenment();
        }
        private void meas2()
        {
            FlukeMeasurenmentClient meas2 = new FlukeMeasurenmentClient(_ipAddress2, _port2, 2 * _numberOfSamples);
            meas2.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas2_MeasFinished);
            meas2.startMeasurenment();
        }
        public void meas1_MeasFinished(double result)
        {
            Console.WriteLine("result1: " + result);
            voltageMeas.Add(result);
            if (currentMeas.Count > measNumber && voltageMeas.Count > measNumber)
                OnMeasurenmentDone();
        }
        public void meas2_MeasFinished(double result)
        {
            Console.WriteLine("result2: " + result);
            currentMeas.Add(result);
            if (currentMeas.Count > measNumber && voltageMeas.Count > measNumber)
                OnMeasurenmentDone();
        }
        /// <summary>
        /// Завршено е едно мерење на отпор
        /// </summary>
        private void OnMeasurenmentDone()
        {
            if (MeasurenmentDone != null)
                MeasurenmentDone(voltageMeas[measNumber], currentMeas[measNumber]);
            measNumber++;
        }
    }
}
