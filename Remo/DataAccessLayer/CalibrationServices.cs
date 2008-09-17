using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace DataAccessLayer
{
    public class TempCalibrationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Entity објект кој ги чува калибрирањата за температури
        /// </summary>
        public EntityLayer.TempCalibration TempCalibrationEntity { get; set; }

        public class TempCalMeasurenmentsView
        {
            public DateTime Time { get; set; }
            public double T1 { get; set; }
            public double T1Ref { get; set; }
            public double T1Err { get; set; }
            public double T2 { get; set; }
            public double T2Ref { get; set; }
            public double T2Err { get; set; }
            public double T3 { get; set; }
            public double T3Ref { get; set; }
            public double T3Err { get; set; }
            public double T4 { get; set; }
            public double T4Ref { get; set; }
            public double T4Err { get; set; }
        }

        public IEnumerable<TempCalMeasurenmentsView> TempCalMeasurenments
        {
            get
            {
                try
                {

                    IEnumerable<TempCalMeasurenmentsView> x =
                        (IEnumerable<TempCalMeasurenmentsView>)from T in TempCalibrationEntity.TempCalMeasurenments
                                                               orderby T.Time
                                                               select new TempCalMeasurenmentsView
                                                               {
                                                                   Time = T.Time,
                                                                   T1 = T.T1,
                                                                   T1Ref = T.T1Ref,
                                                                   T2 = T.T2,
                                                                   T2Ref = T.T2Ref,
                                                                   T3 = T.T3,
                                                                   T3Ref = T.T3Ref,
                                                                   T4 = T.T4,
                                                                   T4Ref = T.T4Ref,
                                                                   T1Err = T.T1Ref - T.T1,
                                                                   T2Err = T.T2Ref - T.T2,
                                                                   T3Err = T.T3Ref - T.T3,
                                                                   T4Err = T.T4Ref - T.T4
                                                               };
                    return x;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                return null;
                
            }
        }
        /// <summary>
        /// Property Change Event за листата TempCalibrationEntity.TempCalMeasurenments (ListWithChangeEvents)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempCalMeasurenments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(e.PropertyName));
        }       

        //Todo 
        public TempCalibrationService(EntityLayer.TempCalibration tempCalibrationEntity)
        {
            this.TempCalibrationEntity = tempCalibrationEntity;
            //Регистрирај Event handler на листата TempCalibrationEntity.TempCalMeasurenments (ListWithChangeEvents)
            TempCalibrationEntity.TempCalMeasurenments.PropertyChanged+=new PropertyChangedEventHandler(TempCalMeasurenments_PropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
