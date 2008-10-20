using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace FlukeClient
{
    public class FlukeMeasurenmentClientTest
    {
        private bool m1Finished = false;
        private bool m2Finished = false;

        public FlukeMeasurenmentClientTest()
        {
            Thread meas1Thread = new Thread(meas1);
            meas1Thread.Start();
            Thread meas2Thread = new Thread(meas2);
            meas2Thread.Start();

        }
        private void meas1()
        {
            FlukeMeasurenmentClient meas1 = new FlukeMeasurenmentClient("192.168.1.1", 3490, 1);
            meas1.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas1_MeasFinished);
            meas1.startMeasurenment();

        }
        private void meas2()
        {
            FlukeMeasurenmentClient meas2 = new FlukeMeasurenmentClient("192.168.1.2", 3490, 1);
            meas2.MeasFinished += new FlukeMeasurenmentClient.MeasFinishedEvent(meas2_MeasFinished);
            meas2.startMeasurenment();
        }
        private void meas1_MeasFinished(double result)
        {
            Console.WriteLine("result1: " + result);
            m1Finished = true;
            if (m2Finished)
                Console.ReadKey();
        }
        private void meas2_MeasFinished(double result)
        {
            Console.WriteLine("result2: " + result);
            m2Finished = true;
            if (m1Finished)
                Console.ReadKey();
        }

        

    }
}
