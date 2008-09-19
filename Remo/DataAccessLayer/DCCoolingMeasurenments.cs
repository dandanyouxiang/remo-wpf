using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    public partial class DataSource
    {
        #region Settings
        #region Private Members
            private double EndAcTmp;
            private double KDropInOl;
        #endregion
        #region Public Members
            public double EndAcTemp 
            {
                get { return EndAcTmp; }
                set 
                {
                    if (EndAcTmp != value) 
                    {
                        EndAcTmp = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("EndAcTemp"));
                    }
                }
            }
            public double KDropInOil
            {
                get{return KDropInOl;}
                set
                {
                    if(KDropInOl!=value) 
                    {
                        KDropInOl=value;
                        OnPropertyChanged(new PropertyChangedEventArgs("KDropInOil"));
                    }
                }
            }
        #endregion
            #region Functions
            public IEnumerable DCCoolingTable()
            {
                IEnumerable DCValues = from dc in Root.DcCoolingMeasurenments.RessistanceTransformerChannels.RessistanceMeasurenments
                                       select new
                                       {
                                           Time = dc.Time.ToLongTimeString(),
                                           No = Root.DcCoolingMeasurenments.RessistanceTransformerChannels.RessistanceMeasurenments.IndexOf(dc),
                                           R1 = (dc.ChannelNo == 0) ? dc.Voltage / dc.Current : 0,
                                           R1Diff = 0,//Todo: da se dopravi R1Diff
                                           R2 = (dc.ChannelNo == 1) ? dc.Voltage / dc.Current : 0,
                                           R2Diff = 0,//Todo: da se dopravi R1Diff
                                       };
                return DCValues;
            }
            private double evalKDropInOil() 
            {
                return -1;
            }
            private double evalEndAcTemp() 
            {
                return -1;
            }
            #endregion
        #endregion
        #region Results

        #region Private Members

        private double T1T0;
            private double T1Rise;
            private double FT1;

            private double T2T0;
            private double T2Rise;
            private double FT2;

        #endregion

        #region Public Members
            public double T1_T0
            {
                get { return T1T0; }
                set
                {
                    if (T1T0 != value)
                    {
                        T1T0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T1_T0"));
                    }
                }
            }
            public double T1_Rise
            {
                get { return T1Rise; }
                set
                {
                    if (T1Rise != value)
                    {
                        T1Rise = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T1_Rise"));
                    }
                }
            }
            public double F_T1
            {
                get { return T1Rise; }
                set
                {
                    if (FT1 != value)
                    {
                        FT1 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("F_T1"));
                    }
                }
            }
            public double T2_T0
            {
                get { return T2T0; }
                set
                {
                    if (T2T0 != value)
                    {
                        T2T0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T2_T0"));
                    }
                }
            }
            public double T2_Rise
            {
                get { return T2Rise; }
                set
                {
                    if (T2Rise != value)
                    {
                        T2Rise = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T2_Rise"));
                    }
                }
            }
            public double F_T2
            {
                get { return T2Rise; }
                set
                {
                    if (FT2 != value)
                    {
                        FT2 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("F_T2"));
                    }
                }
            }
        #endregion
        #region Functions

            private double evalT1T0() 
            {
                return -1;
            }
            private double evalT1Rise() 
            {
                return -1;
            }
            private double evalFT1() 
            {
                return -1;
            }
            private double evalT2T0()
            {
                return -1;
            }
            private double evalT2Rise()
            {
                return -1;
            }
            private double evalFT2() 
            {
                return -1;
            }

        #endregion
        #endregion
    }
}
