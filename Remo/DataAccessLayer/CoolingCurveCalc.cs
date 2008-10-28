using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public class MeasurenmentEntity
    {
        public double TimeSeconds { get; set; }
        public double Value { get; set; }
        public MeasurenmentEntity(double timeSeconds, double rHot)
        {
            TimeSeconds = timeSeconds;
            Value = rHot;
        }
    }
    public class CoolingCurveCalc
    {
        //Inputs
        public List<MeasurenmentEntity> RessMeasurenments { get; set; }
        public double TCold { get; set; }
        public double RCold { get; set; }
        public double TempCoeff { get; set; }

        //MeguRezultati
        public List<MeasurenmentEntity> TempMeasurenments { get; private set; }
        public SortedList<double, double> aotList { get; private set; }

        //Outputs
        public double RAtT0 { get; private set; }
        public double TAtT0 { get; private set; }
        public double AOT { get; private set; }
        public double RAOT { get; private set; }
        public double a { get; private set; }
        public double b { get; private set; }
        public double rSqr { get; private set; }
        //f(t) = RAOT + a*exp(b*t)
        public string Func { get; private set; }
        
        public CoolingCurveCalc()
        {
            RAOT = double.NaN;
            TAtT0 = double.NaN;
            AOT = double.NaN;
            a = double.NaN;
            b = double.NaN;
            rSqr = double.NaN;
        }


        public void calculate()
        {
            TempMeasurenments = new List<MeasurenmentEntity>();
            foreach (MeasurenmentEntity m in RessMeasurenments)
            {
                double thot = calcTHot(TCold, RCold, m.Value, TempCoeff);
                TempMeasurenments.Add(
                    new MeasurenmentEntity(m.TimeSeconds, thot));
            }
            if (!double.IsNaN(TCold))
            {
                //<rSqr,aot> вредности сортирани по rSqr
                SortedList<double, double> aotList = new SortedList<double, double>();
                //Барање на AOT со една децимала
                for (int i = (int)(TCold * 10); i <= 1500; i++)
                {
                    if (i == 700)
                        rSqr = rSqr;
                    double aot = i;
                    aot /= 10;
                    double rsqr = calcEq(aot);
                    if (!aotList.ContainsKey(rsqr))
                        aotList.Add(rsqr, aot);
                }
                rSqr = aotList.Keys[aotList.Count - 1];
                AOT = aotList.Values[aotList.Count - 1];
                calcEq(AOT);
                TAtT0 = a + AOT;
                RAtT0 = calcRHot(TCold, TAtT0, RCold, TempCoeff);
                RAOT = calcRHot(TCold, AOT, RCold, TempCoeff);
                Func = Math.Round(a, 6) + " * Exp(" + Math.Round(b, 6) + "*t)";
            }
        }
        private double calcEq(double aot)
        {
            double sumXi = 0;
            double sumYi = 0;
            double sumLnYi = 0;
            double sumXiSqr = 0;
            double sumXiLnYi = 0;
            double sumLnYiSqr = 0;
            double n = TempMeasurenments.Count;
            foreach (MeasurenmentEntity tempMeas in TempMeasurenments)
            {
                double temp = tempMeas.Value - aot;

                sumXi += tempMeas.TimeSeconds;
                sumYi += temp;
                sumLnYi += Math.Log(temp, Math.E);
                sumXiSqr += Math.Pow(tempMeas.TimeSeconds, 2);
                sumXiLnYi += tempMeas.TimeSeconds * Math.Log(temp, Math.E);
                sumLnYiSqr += Math.Pow(Math.Log(temp, Math.E), 2);
            }

            b = (sumXiLnYi - sumXi * sumLnYi / n) / (sumXiSqr - Math.Pow(sumXi, 2) / n);
            a = Math.Exp(sumLnYi / n - b * sumXi / n);

            double rsqr = Math.Pow(sumXiLnYi - sumXi * sumLnYi / n, 2);
            rsqr /= (sumXiSqr - Math.Pow(sumXi, 2) / n) * (sumLnYiSqr - Math.Pow(sumLnYi, 2) / n);
            return rsqr;
        }

        public double calcTHot(double tCold, double rCold, double rHot, double tempCoeff)
        {
            return tCold + (tempCoeff + tCold) * (rHot - rCold) / rCold;
        }
        public double calcRHot(double tCold, double tHot, double rCold, double tempCoeff)
        {
            return rCold * ((tHot - tCold) / (tCold + tempCoeff) + 1);
        }
    }
}
