using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
namespace Test
{
    class Program
    {
        List<MeasurenmentEntity> ressMeasurenments;
        double tCold { get; set; }
        double rCold { get; set; }
        double tempCoeff { get; set; }

        public Program()
        {
            tCold = 20.2;
            rCold = 0.82293996;
            tempCoeff = 234.5;

            ressMeasurenments = new List<MeasurenmentEntity>();
            ressMeasurenments.Add(new MeasurenmentEntity(12.2, 0.99632));
            ressMeasurenments.Add(new MeasurenmentEntity(17.8, 0.9961));
            ressMeasurenments.Add(new MeasurenmentEntity(22.5, 0.99603));
            ressMeasurenments.Add(new MeasurenmentEntity(27.6, 0.99584));
            ressMeasurenments.Add(new MeasurenmentEntity(32.6, 0.99584));
            ressMeasurenments.Add(new MeasurenmentEntity(37.7, 0.99556));
            ressMeasurenments.Add(new MeasurenmentEntity(42.6, 0.99527));
            ressMeasurenments.Add(new MeasurenmentEntity(47.6, 0.99523));
            ressMeasurenments.Add(new MeasurenmentEntity(52.6, 0.99527));
            ressMeasurenments.Add(new MeasurenmentEntity(57.6, 0.99517));
            ressMeasurenments.Add(new MeasurenmentEntity(62.6, 0.99477));
            ressMeasurenments.Add(new MeasurenmentEntity(67.7, 0.99488));
            ressMeasurenments.Add(new MeasurenmentEntity(72.6, 0.99470));
            ressMeasurenments.Add(new MeasurenmentEntity(77.6, 0.99463));
            ressMeasurenments.Add(new MeasurenmentEntity(82.6, 0.99443));
            ressMeasurenments.Add(new MeasurenmentEntity(87.6, 0.99445));
            ressMeasurenments.Add(new MeasurenmentEntity(92.6, 0.99419));
            ressMeasurenments.Add(new MeasurenmentEntity(97.7, 0.99408));
            ressMeasurenments.Add(new MeasurenmentEntity(102.6, 0.99388));
            ressMeasurenments.Add(new MeasurenmentEntity(107.6, 0.99386));
            ressMeasurenments.Add(new MeasurenmentEntity(112.6, 0.99373));
            ressMeasurenments.Add(new MeasurenmentEntity(117.6, 0.99369));

            CoolingCurveCalc c = new CoolingCurveCalc();
            c.TCold = tCold;
            c.RCold = rCold;
            c.TempCoeff = tempCoeff;
            c.RessMeasurenments = ressMeasurenments;
            c.calculate();

            Console.WriteLine("f(t): " + c.Func);
            Console.WriteLine("RAOT: " + c.RAOT);
            Console.WriteLine("AOT: " + c.AOT);
            Console.WriteLine("r sqr: " + c.rSqr);
            Console.WriteLine("R at T0: " + c.RAtT0);
            Console.WriteLine("T at T0: " + c.TAtT0);

        }
        static void Main(string[] args)
        {
            //new Program();
            new InterpolationCal().test();
            //Console.WriteLine(new CoolingCurveCalc().calcTHot(20.2, 0.82293996, 0.985137, 234.5));
            // double r;
            //Double.TryParse("+1.67899100E-02",System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out r);
            //Console.Write(r);
            Console.ReadKey();
        }
    }
}
