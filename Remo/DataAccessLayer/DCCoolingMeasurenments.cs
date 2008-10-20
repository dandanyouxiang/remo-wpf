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
            private int _selectedChannelDcCool;
            private double _r1ColdAtDcCool;
            private double _r2ColdAtDcCool;
            private double EndAcTmp;
            private double KDropInOl;

        #endregion
        #region Public Members

            /// <summary>
            /// Избраниот канал на трансформаторот на кој ќе се врши мерење на ладење
            /// </summary>
            public int SelectedChannelDcCool
            {
                get { return _selectedChannelDcCool; }
                set
                {
                    if (_selectedChannelDcCool != value)
                    {
                        _selectedChannelDcCool = value;
                        R1ColdAtDcCool = evalR1Cold(_selectedChannelDcCool);
                        R2ColdAtDcCool = evalR2Cold(_selectedChannelDcCool);
                    }
                }
            }
            public double R1ColdAtDcCool
            {
                get { return _r1ColdAtDcCool; }
                set
                {
                    if (_r1ColdAtDcCool != value)
                    {
                        _r1ColdAtDcCool = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1ColdAtDcCool"));
                    }
                }
            }
            public double R2ColdAtDcCool
            {
                get { return _r2ColdAtDcCool; }
                set
                {
                    if (_r2ColdAtDcCool != value)
                    {
                        _r2ColdAtDcCool = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2ColdAtDcCool"));
                    }
                }
            }
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
                EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment> meas = Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments;
                IEnumerable DCValues = from dc in Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments
                                       select new
                                       {
                                           Time = (dc.Time - Root.DcCoolingMeasurenments.TNullTime).TotalSeconds,
                                           No = Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments.IndexOf(dc) + 1,
                                           R1 = (dc.ChannelNo == 1) ? dc.Voltage / dc.Current : double.NaN,
                                           R1Diff = (dc.ChannelNo == 1) ? 
                                               (meas.IndexOf(dc) >= 2 ? 
                                                    ( 100 * (dc.Voltage / dc.Current - meas[meas.IndexOf(dc) - 2].Voltage / meas[meas.IndexOf(dc) - 2].Current)/ (meas[meas.IndexOf(dc) - 2].Voltage / meas[meas.IndexOf(dc) - 2].Current) )
                                                    : double.NaN)
                                                : double.NaN,
                                           R2 = (dc.ChannelNo == 2) ? dc.Voltage / dc.Current : double.NaN,
                                           R2Diff = (dc.ChannelNo == 2) ?
                                               (meas.IndexOf(dc) >= 2 ?
                                                    (100 * (dc.Voltage / dc.Current - meas[meas.IndexOf(dc) - 2].Voltage / meas[meas.IndexOf(dc) - 2].Current) / (meas[meas.IndexOf(dc) - 2].Voltage / meas[meas.IndexOf(dc) - 2].Current) )
                                                    : double.NaN)
                                                : double.NaN
                                       };
                return DCValues;
            }
            private double evalKDropInOil() 
            {
                EntityLayer.TempMeasurenementConfiguration tempch = Root.DcColdMeasurenments.TempMeasurenementConfiguration;
                EntityLayer.TempMeasurenment reducedTempMeasurenment=Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Find(Utils.isReduced);
                EntityLayer.TempMeasurenment lastTempMeasurenment = Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last<EntityLayer.TempMeasurenment>();

                //Измерената температура во времето на редуцирање
                double meanReducedTemp =
                    (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? reducedTempMeasurenment.T1 : 0)
                    + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? reducedTempMeasurenment.T2 : 0)
                    + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? reducedTempMeasurenment.T3 : 0)
                    + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? reducedTempMeasurenment.T4 : 0))
                    / (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0)
                        + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0)
                        + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0)
                        + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)
                    );
                //Последната измерена температура
                double lastReducedTemp =
                    (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? lastTempMeasurenment.T1 : 0)
                    + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? lastTempMeasurenment.T2 : 0)
                    + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? lastTempMeasurenment.T3 : 0)
                    + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? lastTempMeasurenment.T4 : 0))
                    / (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0)
                        + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0)
                        + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0)
                        + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)
                    );

                return meanReducedTemp - lastReducedTemp;
            }
            private double evalEndAcTemp() 
            {
                return -1;
            }
            public double evalR1ColdAtDcCool() 
            {
                return -1;
            }
            public double evalR2ColdAtDcCool()
            {
                return -1;
            }
            #endregion
        #endregion
        #region Results

        #region Private Members

            private double r1AtT0;
            private double t1AtT0;
            private double t1Rise;
            private string ft1;
            private double aot1;
            private double r1AtAOT;

            private double r2AtT0;
            private double t2AtT0;
            private double t2Rise;
            private string ff2;
            private double aot2;
            private double r2AtAOT;

        #endregion

        #region Public Members
            public double R1AtT0
            {
                get { return r1AtT0; }
                set
                {
                    if (r1AtT0 != value)
                    {
                        r1AtT0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T1_T0"));
                    }
                }
            }
            public double T1AtT0
            {
                get { return t1AtT0; }
                set
                {
                    if (t1AtT0 != value)
                    {
                        t1AtT0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T1AtT0"));
                    }
                }
            }
            public double AOT1
            {
                get { return aot1; }
                set
                {
                    if (aot1 != value)
                    {
                        aot1 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("AOT1"));
                    }
                }
            }
            public double T1_Rise
            {
                get { return t1Rise; }
                set
                {
                    if (t1Rise != value)
                    {
                        t1Rise = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T1_Rise"));
                    }
                }
            }
            public string F_T1
            {
                get { return ft1 ; }
                set
                {
                    if (ft1 != value)
                    {
                        ft1 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("F_T1"));
                    }
                }
            }
            public double R1AtAOT
            {
                get { return r1AtAOT; }
                set
                {
                    if (r1AtAOT != value)
                    {
                        r1AtAOT = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1AtAOT"));
                    }
                }
            }
            public List<MeasurenmentEntity> TempMeasurenments1 { get; private set; }

            public double R2AtT0
            {
                get { return r2AtT0; }
                set
                {
                    if (r2AtT0 != value)
                    {
                        r2AtT0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2AtT0"));
                    }
                }
            }
            public double T2AtT0
            {
                get { return t2AtT0; }
                set
                {
                    if (t2AtT0 != value)
                    {
                        t2AtT0 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T2AtT0"));
                    }
                }
            }
            public double AOT2
            {
                get { return aot2; }
                set
                {
                    if (aot2 != value)
                    {
                        aot2 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("AOT2"));
                    }
                }
            }
            public double T2_Rise
            {
                get { return t2Rise; }
                set
                {
                    if (t2Rise != value)
                    {
                        t2Rise = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("T2_Rise"));
                    }
                }
            }
            public string F_T2
            {
                get { return ff2; }
                set
                {
                    if (ff2 != value)
                    {
                        ff2 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("F_T2"));
                    }
                }
            }
            public double R2AtAOT
            {
                get { return r2AtAOT; }
                set
                {
                    if (r2AtAOT != value)
                    {
                        r2AtAOT = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2AtAOT"));
                    }
                }
            }
            public List<MeasurenmentEntity> TempMeasurenments2 { get; private set; }
        #endregion
        #region Functions
            public void calculateResults()
            {
                //Channel 1
                List<MeasurenmentEntity> ressMeasurenments1 = new List<MeasurenmentEntity>();
                DateTime tNullTime = Root.DcCoolingMeasurenments.TNullTime;
                foreach (EntityLayer.RessistanceMeasurenment m in Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments)
                    if (m.ChannelNo == 1)
                        ressMeasurenments1.Add(new MeasurenmentEntity((m.Time - tNullTime).TotalSeconds, m.Voltage / m.Current));
                CoolingCurveCalc c1 = new CoolingCurveCalc();
                c1.TCold = TCold;
                c1.RCold = R1ColdAtDcCool;
                c1.TempCoeff = this.Root.TransformerProperties.HvTempCoefficient;
                c1.RessMeasurenments = ressMeasurenments1;
                c1.calculate();
                F_T1 = c1.Func;
                T1AtT0 = c1.TAtT0;
                T1_Rise = c1.TAtT0 - c1.TCold;
                R1AtT0 = c1.RAtT0;
                AOT1 = c1.AOT;
                R1AtT0 = c1.RAOT;
                TempMeasurenments1 = c1.TempMeasurenments;

                //Channel 2
                List<MeasurenmentEntity> ressMeasurenments2 = new List<MeasurenmentEntity>();
                foreach (EntityLayer.RessistanceMeasurenment m in Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments)
                    if (m.ChannelNo == 2)
                        ressMeasurenments2.Add(new MeasurenmentEntity((m.Time - tNullTime).TotalSeconds, m.Voltage / m.Current));
                CoolingCurveCalc c2 = new CoolingCurveCalc();
                c2.TCold = TCold;
                c2.RCold = R2ColdAtDcCool;
                c2.TempCoeff = this.Root.TransformerProperties.LvTempCoefficient;
                c2.RessMeasurenments = ressMeasurenments2;

                c2.calculate();

                F_T2 = c2.Func;
                T2AtT0 = c2.TAtT0;
                T2_Rise = c2.TAtT0 - c2.TCold;
                R2AtT0 = c2.RAtT0;
                AOT2 = c2.AOT;
                R2AtT0 = c2.RAOT;
                TempMeasurenments2 = c2.TempMeasurenments;
            }

        #endregion
        #endregion
    }
}
