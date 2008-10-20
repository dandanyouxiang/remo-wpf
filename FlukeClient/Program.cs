using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlukeClient
{
    class Program
    {
        static void Main(String[] args)
        {
            RessistanceDataSourceTest rdsTest = new RessistanceDataSourceTest();
            //new FlukeMeasurenmentClientTest();
            Console.ReadKey();
            rdsTest.stopMeas();
            Console.ReadKey();
        }
    }
}
