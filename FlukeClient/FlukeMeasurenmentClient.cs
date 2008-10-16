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
        public delegate void MeasFinishedEvent(double measurenment);
        public event MeasFinishedEvent MeasFinished;

        private enum TriggerSource { IMMEDIATE, EXTERNAL };
        private enum NPlc { _1 = 0, _10, _100 };

        private const int TRIGGER_COUNT = 1;
        private const int SAMPLE_COUNT = 4;

        private TcpClient client;
        private string _ipAddress;
        private int _port;
        private int _numberOfMeasurenments;

        private void OnMeasFinished(double measurenment)
        {
            if (MeasFinished != null)
                MeasFinished(measurenment);
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
                //Отварање на конекција кон инструментот
                NetworkStream stream = openConection(ref client, _ipAddress, _port);

                //Конфигурирање на мерењата
                enterRemote(stream);
                reset(stream);
                clearStatusReg(stream);
                setNplc(stream, NPlc._10);
                setVoltageAutoRange(stream);
                setTriggerCount(stream, TRIGGER_COUNT);
                setSampleCount(stream, SAMPLE_COUNT);
                setTriggerDelay(stream);
                setTriggerSource(stream, TriggerSource.EXTERNAL);
                System.Threading.Thread.Sleep(500);
                //Изведување на мерења
                for (int i = 0; i < _numberOfMeasurenments; i++)
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
                clearStatusReg(stream);
                reset(stream);
                
                stream.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                throw ex;
            }
            finally
            {
                closeSocket(client);
            }          
        }

        private double parseStringAndCalcMean(string measStr)
        {
            string[] split = measStr.Split(',');
            List<double> measurenments = new List<double>();
            foreach (string str in split)
            {
                double result;
                if (Double.TryParse(str, out result))
                    measurenments.Add(result);
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
        public void closeSocket(TcpClient client)
        {
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
        private void reset(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("*rst\n");
            clientStream.Write(buffer, 0, buffer.Length);
        }
        //Clear status byte summary and all event registers
        private void clearStatusReg(NetworkStream clientStream)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("*CLS\n");
            clientStream.Write(buffer, 0, buffer.Length);
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
