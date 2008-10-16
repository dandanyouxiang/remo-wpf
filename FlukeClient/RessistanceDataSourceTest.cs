using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlukeClient
{
    public class RessistanceDataSourceTest
    {
        private string IP_ADDRESS_VOLTAGE = "192.168.1.1";
        private string IP_ADDRESS_CURRENT = "192.168.1.2";
        private int PORT_VOLTAGE = 3490;
        private int PORT_CURRENT = 3490;
        private int _sampleRate = 2;
        private int _numberOfSamples = 10;
        private double _current = 1;

        public RessistanceDataSourceTest()
        {
            RessistanceDataSource rds = 
                new RessistanceDataSource(_sampleRate,_numberOfSamples,_current, 
                    IP_ADDRESS_VOLTAGE, IP_ADDRESS_CURRENT, PORT_VOLTAGE, PORT_CURRENT);

            rds.MeasurenmentDone += new RessistanceDataSource.MeasurenmentDoneEvent(rsd_MeasurenmentDone);
            rds.start_RessistanceMeasurenments();
        }
        public void rsd_MeasurenmentDone(double voltage, double current, int measNumber)
        {
            Console.WriteLine("DateTime.Now:" + DateTime.Now + "no:" + measNumber + " Voltage:" + voltage + " Current:" + current);
        }
    }
}
