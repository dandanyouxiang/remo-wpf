using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace FlukeClient
{
    public class FlukeMeasurenmentClient
    {
        public delegate void MeasErrorEvent();
        public event MeasErrorEvent MeasError;

        public delegate void MeasFinishedEvent(double measurenment);
        public delegate void MeasEndEvent();
        /// <summary>
        /// event за крај на едно мерење
        /// </summary>
        public event MeasFinishedEvent MeasFinished;
       
        /// <summary>
        /// event за крај на сите мерења
        /// </summary>
        public event MeasEndEvent MeasEnd;

        private enum TriggerSource { IMMEDIATE, EXTERNAL };
        private enum NPlc { _1 = 0, _10, _100 };

        private const int TRIGGER_COUNT = 1;
        private const int SAMPLE_COUNT = 2;

        private TcpClient client;
        private string _ipAddress;
        private int _port;
        private int _numberOfMeasurenments;
        private NetworkStream stream;
        public bool IsStop { get; set; }
        private void OnMeasFinished(double measurenment)
        {
            if (MeasFinished != null)
                MeasFinished(measurenment);
        }
        private void OnMeasEnd()
        {
            if (MeasEnd != null)
                MeasEnd();
        }

        public FlukeMeasurenmentClient(string ipAddress, int port, int numberOfMeasurenments)
        {
            _ipAddress = ipAddress;
            _port = port;
            _numberOfMeasurenments = numberOfMeasurenments;
        }

        public void startMeasurenment()
        {
            try
            {
                client = new TcpClient(_ipAddress, _port);
                //client.Connect(_ipAddress, _port);
                stream = client.GetStream();
                //Отварање на конекција кон инструментот
                //stream = openConection(ref client, _ipAddress, _port);
                //Конфигурирање на мерењата
                init();
                //Изведување на мерења
                for (int i = 0; i < _numberOfMeasurenments && !IsStop; i++)
                {
                    //Испраќање на команда за читање на податоци од инструментот
                    //После ова команда се чека на Trigger-и
                    sendReadCommand(stream);
                    //Читање на податоци од input stream штом ќе станат достапни
                    //Оваа метода ќе врати по читањето на сите податоци
                    string measStr = readData(stream, TRIGGER_COUNT * SAMPLE_COUNT);

                    //Пресметување на средна вредност од сите читања
                    double mean = parseStringAndCalcMean(measStr);

                    Console.WriteLine(_ipAddress + " i:" + i + " Read string\n" + measStr);
                    //Throw event
                    OnMeasFinished(mean);
                }
                reset();
                clearStatusReg();
                stream.Flush();
                stream.Dispose();
                OnMeasEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                if (MeasError != null)
                    MeasError();
                //throw ex;
            }
            finally
            {
                closeSocket();
            }          
        }
        public void init()
        {
            enterRemote(stream);
            reset();
            clearStatusReg();
            reset();
            clearStatusReg();
            System.Threading.Thread.Sleep(1000);
            setNplc(stream, NPlc._10);
            setVoltageAutoRange(stream);
            setTriggerCount(stream, TRIGGER_COUNT);
            setSampleCount(stream, SAMPLE_COUNT);
            setTriggerDelay(stream);
            setTriggerSource(stream, TriggerSource.EXTERNAL);
            IsStop = false;
            //System.Threading.Thread.Sleep(100);
        }
        private double parseStringAndCalcMean(string measStr)
        {
            string[] split = measStr.Split(',');
            List<double> measurenments = new List<double>();
            foreach (string str in split)
            {
                double result;
                if (Double.TryParse(str.Replace('.',','), out result))
                    measurenments.Add(result);
                Console.Write("parsed result:" + result);
            }
            double mean = 0;
            foreach (double meas in measurenments)
                mean += meas;
            if (measurenments.Count != 0)
                mean = mean / measurenments.Count;
            return mean;
        }

        #region read наредби
        private string readData(NetworkStream clientStream, int numberOfMeasPoints)
        {
            string str = "";
            int measPointsRead = 0;
            ASCIIEncoding encoder = new ASCIIEncoding();

            while (measPointsRead < numberOfMeasPoints)
            {
                //Чекај податоците да станат достапни
                while (!clientStream.DataAvailable)
                    System.Threading.Thread.Sleep(100);
                byte[] readBuffer = new byte[4096];
                int bytesRead = clientStream.Read(readBuffer, 0, 4096);
                string readString = encoder.GetString(readBuffer, 0, bytesRead);
                str += readString;

                string[] split = readString.Split(',');
                int k = 0;
                for (int i = 0; i < split.Length; i++)
                    if (split[i] != "")
                        k++;
                measPointsRead += k;
                //Console.WriteLine("Client Read\n" + readString + " Meas Points: " + k + "\n");
            }
            return str;
        }
        #endregion

        #region open/close TCP/IP connection
        private NetworkStream openConection(ref TcpClient client, string ipAddress, int port)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            client = new TcpClient();
            client.Connect(serverEndPoint);
            return client.GetStream();

        }
        public void closeSocket()
        {
            if (client != null)
                client.Client.Close();
        }
        #endregion

        #region Write наредби
        private void enterRemote(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("syst:rem\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        public void reset()
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("*rst\n");
            stream.Write(buffer, 0, buffer.Length);
        }
        //Clear status byte summary and all event registers
        public void clearStatusReg()
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("*CLS\n");
            stream.Write(buffer, 0, buffer.Length);
        }
        //NPLC cycles
        private void setNplc(NetworkStream clientStream, NPlc nplc)
        {
            string nplcStr = Math.Pow(10, (int)nplc).ToString();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("VOLT:NPLC " + nplcStr + "\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Voltage range
        private void setVoltageAutoRange(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("VOLT:RANGE:AUTO ON\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Trigger count
        private void setTriggerCount(NetworkStream clientStream, int noOfTriggers)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("TRIG:COUN +" + noOfTriggers + "\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Sample Count
        private void setSampleCount(NetworkStream clientStream, int noOfSamples)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("SAMP:COUN " + noOfSamples + "\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Trigger Source
        private void setTriggerSource(NetworkStream clientStream, TriggerSource triggerSource)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string trigSource = "";
            switch (triggerSource)
            {
                case TriggerSource.EXTERNAL: trigSource = "EXT"; break;
                case TriggerSource.IMMEDIATE: trigSource = "IMM"; break;
            }
            byte[] buffer = encoder.GetBytes("TRIG:SOUR " + trigSource + "\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Trigger delay
        private void setTriggerDelay(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("TRIG:DEL MIN\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Read command
        private void sendReadCommand(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("READ?\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        #endregion

    }

}
