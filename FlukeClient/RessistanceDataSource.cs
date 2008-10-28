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
        private bool meas1End;
        private bool meas2End;
        private List<double> voltageMeas;
        private List<double> currentMeas;
        FlukeMeasurenmentClient meas1;
        FlukeMeasurenmentClient meas2;

        public delegate void MeasurenmentDoneEvent(double voltage, double current, int measNumber);
        public delegate void MeasurenmentsEndEvent();
        public delegate void MeasurenmentErrorEvent();
        /// <summary>
        /// Завршено е едно мерење на отпор.
        /// </summary>
        public event MeasurenmentDoneEvent MeasurenmentDone;
        /// <summary>
        /// Сите мерења се завршени
        /// </summary>
        public event MeasurenmentsEndEvent MeasurenmentsEnd;
        public event MeasurenmentErrorEvent MeasurenmentError;

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

            meas1Thread = new Thread(measure1);
            meas2Thread = new Thread(measure2);
            meas1Thread.IsBackground = true;
            meas2Thread.IsBackground = true;
            meas1Thread.Start();
            meas2Thread.Start();
        }
        public void stopRessistanceMeasurenments()
        {
            meas1.IsStop = true;
            meas2.IsStop = true;
        }

        private void measure1()
        {
            meas1End = false;
            meas1 = new FlukeMeasurenmentClient(_ipAddress1, _port1,  _numberOfSamples);
            meas1.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas1_MeasFinished);
            meas1.MeasEnd += new FlukeMeasurenmentClient.MeasEndEvent(meas1_MeasEnd);
            meas1.MeasError+=new FlukeMeasurenmentClient.MeasErrorEvent(meas_MeasError);
            meas1.startMeasurenment();
        }
        private void measure2()
        {
            meas2End = false;
            meas2 = new FlukeMeasurenmentClient(_ipAddress2, _port2,  _numberOfSamples);
            meas2.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas2_MeasFinished);
            meas2.MeasEnd += new FlukeMeasurenmentClient.MeasEndEvent(meas2_MeasEnd);
            meas2.MeasError += new FlukeMeasurenmentClient.MeasErrorEvent(meas_MeasError);
            meas2.startMeasurenment();
        }
        public void meas_MeasError()
        {
            if (MeasurenmentError != null)
                MeasurenmentError();
        }
        public void meas1_MeasFinished(double result)
        {
            Console.WriteLine("voltage: " + result);
            voltageMeas.Add(result);
            if (currentMeas.Count > measNumber && voltageMeas.Count > measNumber)
                OnMeasurenmentDone();
        }
        public void meas2_MeasFinished(double result)
        {
            Console.WriteLine("current: " + result);
            currentMeas.Add(result);
            if (currentMeas.Count > measNumber && voltageMeas.Count > measNumber)
                OnMeasurenmentDone();
        }
        public void meas1_MeasEnd()
        {
            meas1End = true;
            if (meas2End)
                OnMeasurenmentsEnd();
        }
        public void meas2_MeasEnd()
        {
            meas2End = true;
            if (meas1End)
                OnMeasurenmentsEnd();
        }
        /// <summary>
        /// Завршено е едно мерење на отпор
        /// </summary>
        private void OnMeasurenmentDone()
        {
            if (MeasurenmentDone != null)
            {
                MeasurenmentDone.BeginInvoke(voltageMeas[measNumber], currentMeas[measNumber], measNumber,null,null);
                Console.WriteLine("voltage meas:" + voltageMeas[measNumber] + " current meas:" + currentMeas[measNumber]);
            }
            measNumber++;
        }
        /// <summary>
        /// Крај на мерењата на двата канали
        /// </summary>
        private void OnMeasurenmentsEnd()
        {
            if (MeasurenmentsEnd != null)
            {
                MeasurenmentsEnd.BeginInvoke(null, null);
            }
        }
    }
}
