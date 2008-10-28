using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityLayer;

namespace DataAccessLayer
{
    class InterpolationEntity
    {
        public double X { get; set; }
        public double Delta { get; set; }
    }
    public class InterpolationCal
    {
        private string TemperatureCalibrationFilePath;
        private string RessistanceCalibrationFilePath;
        private string RessistanceBaseCalibrationFilePath;
        private List<InterpolationEntity> t1List;
        private List<InterpolationEntity> t2List;
        private List<InterpolationEntity> t3List;
        private List<InterpolationEntity> t4List;
        private List<InterpolationEntity> ressistanceList;
        private List<InterpolationEntity> ressistanceBaseList;

        public InterpolationCal()
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            TemperatureCalibrationFilePath = baseDir + Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["TemperatureCalibrationFile"]);
            RessistanceBaseCalibrationFilePath = baseDir + Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["RessistanceBaseCalibrationFile"]);
            RessistanceCalibrationFilePath = baseDir + Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["RessistanceCalibrationFile"]);
            
            //Читај фајл со температурните калибрирања
            readTemperatureFile();
            //Читај фајл со отпорничките калибрирања
            readRessistanceFile();
            //Читај фајл со основните отпорнички калибрирања
            readBaseRessistanceFile();
        }

        public void readBaseRessistanceFile()
        {
            RessistanceCalibration r = new EntityLayer.RessistanceCalibration();
            r.readXml(RessistanceBaseCalibrationFilePath);
            ressistanceBaseList = new List<InterpolationEntity>();
            foreach (EntityLayer.RessistanceCalMeasurenment meas in r.RessistanceCalMeasurenments)
            {
                ressistanceBaseList.Add(new InterpolationEntity() { X = meas.RMeas, Delta = meas.RRef - meas.RMeas });
            }
        }
        public void readRessistanceFile()
        {
            RessistanceCalibration r = new EntityLayer.RessistanceCalibration();
            r.readXml(RessistanceCalibrationFilePath);
            ressistanceList = new List<InterpolationEntity>();
            foreach (EntityLayer.RessistanceCalMeasurenment meas in r.RessistanceCalMeasurenments)
            {
                ressistanceList.Add(new InterpolationEntity() { X = meas.RMeas, Delta = meas.RRef - meas.RMeas });
            }
        }
        public void readTemperatureFile()
        {
            //Читај фајл со температурните калибрирања
            TempCalibrationService tempCalibrationService = new TempCalibrationService();
            tempCalibrationService.TempCalibrationEntity = new EntityLayer.TempCalibration();
            tempCalibrationService.TempCalibrationEntity.readXml(TemperatureCalibrationFilePath);
            t1List = new List<InterpolationEntity>();
            t2List = new List<InterpolationEntity>();
            t3List = new List<InterpolationEntity>();
            t4List = new List<InterpolationEntity>();
            foreach (EntityLayer.TempCalMeasurenment meas in tempCalibrationService.TempCalibrationEntity.TempCalMeasurenments)
            {
                t1List.Add(new InterpolationEntity() { X = meas.T1, Delta = meas.T1Ref - meas.T1 });
                t2List.Add(new InterpolationEntity() { X = meas.T2, Delta = meas.T2Ref - meas.T2 });
                t3List.Add(new InterpolationEntity() { X = meas.T3, Delta = meas.T3Ref - meas.T3 });
                t4List.Add(new InterpolationEntity() { X = meas.T4, Delta = meas.T4Ref - meas.T4 });
            }
        }

        public double interpolateT1(double temperature)
        {
            return interpolateY(temperature, temperature, t1List);
        }
        public double interpolateT2(double temperature)
        {
            return interpolateY(temperature, temperature, t2List);
        }
        public double interpolateT3(double temperature)
        {
            return interpolateY(temperature, temperature, t3List);
        }
        public double interpolateT4(double temperature)
        {
            return interpolateY(temperature, temperature, t4List);
        }

        public double interpolateBaseRessistance(double ressistance)
        {
            return interpolateY(ressistance, ressistance, ressistanceBaseList);
        }
        public double interpolateRessistance(double ressistance)
        {
            return interpolateY(ressistance, ressistance, ressistanceList);
        }

        private double interpolateY(double x, double y, List<InterpolationEntity> list)
        {
            if (list.Count > 0)
            {
                int index = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].X >= x)
                    {
                        index = i;
                        break;
                    }
                }
                //x <= најмалиот елемент
                if (index == 0)
                {
                    y += y * list[0].Delta / list[0].X;
                }
                //x > најголемиот елемент
                else if (index == -1)
                {
                    y += y * list[list.Count - 1].Delta / list[list.Count - 1].X;
                }
                //најмалиот елемент <= x <= најголемиот елемент
                else
                {
                    y += list[index - 1].Delta + (list[index].Delta - list[index - 1].Delta) * (x - list[index - 1].X) / (list[index].X - list[index - 1].X);
                }
            }
            return y;
        }
        
        public void test()
        {
            List<InterpolationEntity> list = new List<InterpolationEntity>();
            list.Add(new InterpolationEntity() { X = 1, Delta = 2 });
            //list.Add(new InterpolationEntity() { X = 2, Delta = -1 });
            //list.Add(new InterpolationEntity() { X = 6, Delta = 3 });
            double x = 0;
            double y = 1;
            Console.WriteLine(interpolateY(x, y, list));
        }
    }
}
