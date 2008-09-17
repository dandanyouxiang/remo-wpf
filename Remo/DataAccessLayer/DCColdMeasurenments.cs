﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    public class TableTempHeader 
    {
        public string Date{get;set;}
        public string Time{get;set;}
        public string T1{get;set;}
        public string T2{get;set;}
        public string T3{get;set;}
        public string T4{get;set;}
        public string TAmb{get;set;}
        public string TOil{get;set;}

        public TableTempHeader(string date, string time, string t1, string t2,string t3, string t4, string tAmb, string tOil) 
        {
            this.Date = date;
            this.Time = time;
            this.T1 = t1;
            this.T2 = t2;
            this.T3 = t3;
            this.T4 = t4;
            this.TAmb = tAmb;
            this.TOil = tOil;
        }
    } 

    public partial class DataSource : INotifyPropertyChanged
    {

        #region DCColdTemp

        #region Private Properties
                    private double T1MeanDCColdTempTbl;
                    private string T1MeanDCColdTempTblHdr;
                    private double T2MeanDCColdTempTbl;
                    private string T2MeanDCColdTempTblHdr;
                    private double T3MeanDCColdTempTbl;
                    private string T3MeanDCColdTempTblHdr;
                    private double T4MeanDCColdTempTbl;
                    private string T4MeanDCColdTempTblHdr;

                    private double OilDCColdTempTbl;
                    private double AmbDCColdTempTbl;

                    private int SelectedChnnl;
                    private TableTempHeader DCColdTemperatureTableHdr;
     
            #endregion

            #region Public Properties

                    public static List<string> Channels = new List<string> { "A-B", "B-C", "C-A" };

                    public int selectedChannelIndex {get;set; }

                    public int SelectedChannel 
                    {
                        get { return SelectedChnnl; }
                        set 
                        {
                            if (SelectedChnnl != value) 
                            {
                                SelectedChnnl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs(null));

                            }
                        }
                    }

                    public TableTempHeader DCColdTemperatureTableHeader
                    {
                        get { return DCColdTemperatureTableHdr; }
                        set
                        {
                            if (DCColdTemperatureTableHdr != value)
                            {
                                DCColdTemperatureTableHdr = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("DCColdTemperatureTableHeader"));

                            }
                        }
                    }
                    
                    //////////////////////////////

                    /// <summary>
                    /// Средната вредноста за првиот канал.
                    /// </summary>
                    public double T1MeanDCColdTempTable 
                    {
                        get{return T1MeanDCColdTempTbl;}
                        set 
                        {
                            if(T1MeanDCColdTempTbl!=value) 
                            {
                                T1MeanDCColdTempTbl=value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T1MeanDCColdTempTable"));
                            }
                        }
                    }
                    /// <summary>
                    /// Текстот над келијата.
                    /// </summary>
                    public string T1MeanDCColdTempTableHeader 
                    {
                        get { return T1MeanDCColdTempTblHdr; }
                        set 
                        {
                            if (T1MeanDCColdTempTblHdr != value) 
                            {
                                T1MeanDCColdTempTblHdr = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T1MeanDCColdTempTableHeader"));
                            }
                        }
                    }
                    /// <summary>
                    /// Средната вредноста за вториот канал.
                    /// </summary>
                    public double T2MeanDCColdTempTable 
                    {
                        get { return T2MeanDCColdTempTbl; }
                        set 
                        {
                            if (T2MeanDCColdTempTbl != value) 
                            {
                                T2MeanDCColdTempTbl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T2MeanDCColdTempTable"));
                            }
                        }
                    }
                    /// <summary>
                    /// Текстот над келијата.
                    /// </summary>
                    public string T2MeanDCColdTempTableHeader 
                    {
                        get { return T2MeanDCColdTempTblHdr; }
                        set 
                        {
                            if (T2MeanDCColdTempTblHdr != value) 
                            {
                                T2MeanDCColdTempTblHdr = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T2MeanDCColdTempTableHeader"));
                            }
                        }
                    }
                    /// <summary>
                    /// Средната вредноста за третиот канал.
                    /// </summary>
                    public double T3MeanDCColdTempTable
                    {
                        get { return T3MeanDCColdTempTbl; }
                        set
                        {
                            if (T3MeanDCColdTempTbl != value)
                            {
                                T3MeanDCColdTempTbl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T3MeanDCColdTempTable"));
                            }
                        }
                    }
                    /// <summary>
                    /// Текстот над келијата.
                    /// </summary>
                    public string T3MeanDCColdTempTableHeader
                    {
                        get { return T3MeanDCColdTempTblHdr; }
                        set
                        {
                            if (T3MeanDCColdTempTblHdr != value)
                            {
                                T3MeanDCColdTempTblHdr = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T3MeanDCColdTempTableHeader"));
                            }
                        }
                    }
                    /// <summary>
                    /// Средната вредноста за четвртиот канал.
                    /// </summary>
                    public double T4MeanDCColdTempTable
                    {
                        get { return T4MeanDCColdTempTbl; }
                        set
                        {
                            if (T4MeanDCColdTempTbl != value)
                            {
                                T4MeanDCColdTempTbl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T4MeanDCColdTempTable"));
                            }
                        }
                    }
                    /// <summary>
                    /// Текстот над келијата.
                    /// </summary>
                    public string T4MeanDCColdTempTableHeader
                    {
                        get { return T4MeanDCColdTempTblHdr; }
                        set
                        {
                            if (T4MeanDCColdTempTblHdr != value)
                            {
                                T4MeanDCColdTempTblHdr = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("T4MeanDCColdTempTableHeader"));
                            }
                        }
                    }
                    /// <summary>
                    /// Средната температура во мосло.
                    /// </summary>
                    public double OilDCColdTempTable 
                    {
                        get { return OilDCColdTempTbl; }
                        set 
                        {
                            if (OilDCColdTempTbl != value) 
                            {
                                OilDCColdTempTbl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("OilDCColdTempTable"));
                            }
                        }
                    }
                    /// <summary>
                    /// Средната температура во воздух.
                    /// </summary>
                    public double AmbDCColdTempTable
                    {
                        get { return AmbDCColdTempTbl; }
                        set
                        {
                            if (AmbDCColdTempTbl != value)
                            {
                                AmbDCColdTempTbl = value;
                                OnPropertyChanged(new PropertyChangedEventArgs("AmbDCColdTempTable"));
                            }
                        }
                    }

            #endregion

        ////////////////////////////////////////
        //Polenenje na redicite vo tabelata////
        //////////////////////////////////////

        /// <summary>
        /// DataSource за табелата со температури во табот DCColdMeasurenment во подтабот TemperatureMeasurenment
        /// </summary>
        /// <returns></returns>
        public IEnumerable DCColdTemperatureTable()
        {
            var tempch = root.DcColdMeasurenments.TempMeasurenementConfiguration;

            IEnumerable exp = from temp in root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments
                              select new
                              {
                                  Time = temp.Time.Date,
                                  Date = temp.Time.ToLongTimeString(),
                                  T1 = temp.T1,
                                  T2 = temp.T2,
                                  T3 = temp.T3,
                                  T4 = temp.T4,
                                  TAmb = (((!tempch.IsChannel1Oil && tempch.IsChannel1On) ? temp.T1 : 0) + ((!tempch.IsChannel2Oil && tempch.IsChannel2On) ? temp.T2 : 0) + ((!tempch.IsChannel3Oil && tempch.IsChannel3On) ? temp.T3 : 0) + ((!tempch.IsChannel4Oil && tempch.IsChannel4On) ? temp.T4 : 0)) / (((!tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0) + ((!tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0) + ((!tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0) + ((!tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)),
                                  TOil = (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? temp.T1 : 0) + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? temp.T2 : 0) + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? temp.T3 : 0) + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? temp.T4 : 0)) / (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0) + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0) + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0) + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)),
                                  

                              };
            return exp;
        }

        //Todo: Da se napravi properti za hederot
        /// <summary>
        /// Хедерот за табелата со температури во табот DCColdMeasurenment во подтабот TemperatureMeasurenment
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        /*
        public string[] DCColdTemperatureTableHeader()
        {
            string[] strRet = new string[8];

            var tempch = root.DcColdMeasurenments.TempMeasurenementConfiguration;

            strRet[0] = "Date";
            strRet[1] = "Time";
            strRet[2] = "T1" + ((tempch.IsChannel1Oil) ? "(Oil)" : "(Amb)");
            strRet[3] = "T2" + ((tempch.IsChannel2Oil) ? "(Oil)" : "(Amb)");
            strRet[4] = "T3" + ((tempch.IsChannel3Oil) ? "(Oil)" : "(Amb)");
            strRet[5] = "T4" + ((tempch.IsChannel4Oil) ? "(Oil)" : "(Amb)");
            strRet[6] = "T Amb";
            strRet[7] = "T Oil";

            return strRet;
        }*/

        public TableTempHeader evalDCColdTemperatureTableHeader()
        {
            var tempch = root.DcColdMeasurenments.TempMeasurenementConfiguration;
            return new TableTempHeader("Date","Time","T1" + ((tempch.IsChannel1Oil) ? "(Oil)" : "(Amb)"),"T2" + ((tempch.IsChannel2Oil) ? "(Oil)" : "(Amb)"),"T3" + ((tempch.IsChannel3Oil) ? "(Oil)" : "(Amb)"),"T4" + ((tempch.IsChannel4Oil) ? "(Oil)" : "(Amb)"),"T Amb","T Oil");
        }
        /////////////////////////////////////////

        ////////////////////////////////////////////////////
        ///Funkcii za presmetuvanje na poodelnite kelii vo tabelata///
        //////////////////////////////////////////////////

 

        /// <summary>
        /// Ја пресметува средната вредноста за првиот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT1MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T1;
            }
            retValue /= root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT1MeanDCColdTempTableHeader()
        {
            return ((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil) ? "T1(Oil)" : "T1(Amb)");
        }

        /// <summary>
        /// Ја пресметува средната вредноста за вториот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT2MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T2;
            }
            retValue /= root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT2MeanDCColdTempTableHeader()
        {
            return ((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil) ? "T2(Oil)" : "T2(Amb)");
        }
        /// <summary>
        /// Ја пресметува средната вредноста за третиот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT3MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T3;
            }
            retValue /= root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT3MeanDCColdTempTableHeader()
        {
            return ((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil) ? "T3(Oil)" : "T3(Amb)");
        }
        /// <summary>
        /// Ја пресметува средната вредноста за четвртиот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT4MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T4;
            }
            retValue /= root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }

        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT4MeanDCColdTempTableHeader()
        {
            return ((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil) ? "T4(Oil)" : "T4(Amb)");
        }
        /// <summary>
        /// Пресметка на средната вредност на температурата во масло.
        /// </summary>
        /// <returns></returns>
        private double evalOilDCColdTempTable() 
        {
            int channelInOil=((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil)?1:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil)?1:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil)?1:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil)?1:0);
            double channelInOilSum=((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil)?T1MeanDCColdTempTable:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil)?T2MeanDCColdTempTable:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil)?T3MeanDCColdTempTable:0)+((root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil)?T4MeanDCColdTempTable:0);

            return channelInOilSum / channelInOil;
        }

        /// <summary>
        /// Пресметка на средната вредност на температурата во воздух.
        /// </summary>
        /// <returns></returns>
        private double evalAmbDCColdTempTable()
        {
            int channelInOil = ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil) ? 1 : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil) ? 1 : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil) ? 1 : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil) ? 1 : 0);
            double channelInOilSum = ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil) ? T1MeanDCColdTempTable : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil) ? T2MeanDCColdTempTable : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil) ? T3MeanDCColdTempTable : 0) + ((!root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil) ? T4MeanDCColdTempTable : 0);

            return channelInOilSum / channelInOil;
        }

        ////////////////////////////////////////////////////

        #endregion
        #region DCColdRessis

        ///////////////////////////////
        #region Private memebers

            private double R1AtStdT;
            private double R2AtStdT;
            private double R1Ph;
            private double R2Ph;
            private int StdT;
            /// <summary>
            /// Средни вредности за првиот отпонички канал.
            /// </summary>
            private double R1Cld;
            /// <summary>
            /// Средни вредности за вториот отпонички канал.
            /// </summary>
            private double R2Cld;          

            private double StdDevTR1;
            private double StdDevTR2;

            private double TCld;

        #endregion

        #region Public Properties

            ////////////////////////
            public double TCold
            {
                get { return TCld; }
                set
                {
                    if (TCld != value)
                    {
                        TCld = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("TCold"));
                    }
                }
            }
        
            public double R1AtStdTemp
            {
                get { return R1AtStdT; }
                set
                {
                    if (R1AtStdT != value)
                    {
                        R1AtStdT = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1AtStdTemp"));
                    }
                }
            }

            public double R2AtStdTemp
            {
                get { return R2AtStdT; }
                set
                {
                    if (R2AtStdT != value)
                    {
                        R2AtStdT = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2AtStdTemp"));
                    }
                }
            }

            public double R1Phase
            {
                get { return R2AtStdT; }
                set
                {
                    if (R1Ph != value)
                    {
                        R1Ph = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1Phase"));
                    }
                }
            }

            public double R2Phase
            {
                get { return R2Ph; }
                set
                {
                    if (R2Ph != value)
                    {
                        R2Ph = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2Phase"));
                    }
                }
            }
            /// <summary>
            /// Средна вредност од измерените отпори за првиот отпорнички канал.
            /// </summary>
            public double R1Cold
            {
                get { return R1Cld; }
                set
                {
                    if (R1Cld != value)
                    {
                        R1Cld = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1Cold"));
                    }
                }
            }
            /// <summary>
            /// Средна вредност од измерените отпори за вториот отпорничи канал.
            /// </summary>
            public double R2Cold
            {
                get { return R2Cld; }
                set
                {
                    if (R2Cld != value)
                    {
                        R2Cld = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2Cold"));
                    }
                }
            }

            /// <summary>
            /// Се задава, за пресметување на полињата подоле.
            /// </summary>
            public int StdTemp
            {
                get { return StdT; }
                set
                {
                    if (StdT != value)
                    {
                        StdT = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("StdTemp"));
                    }
                }
            }    
            ///////////////////////

            /// <summary>
            /// Стандарна девијација на мерените отпори на првиот канал.
            /// </summary>
            public double StdDevTempR1 
            {
                get { return StdDevTR1; }
                set 
                {
                    if (StdDevTR1 != value) 
                    {
                        StdDevTR1 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("StdDevTempR1"));
                    }
                }
            }
            /// <summary>
            /// Стандарна девијација на мерените отпори на вториот канал.
            /// </summary>
            public double StdDevTempR2 
            {
                get { return StdDevTR2; }
                set
                {
                    if (StdDevTR2 != value)
                    {
                        StdDevTR2 = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("StdDevTempR2"));
                    }
                }
            }
            
        #endregion



        /// <summary>
        /// DataSource за табелата со отпори во табот DCColdMeasurenment во подтабот RessistanceMeasurenment
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        public IEnumerable DCColdRessistanceTable(int channelIndex)
        {
            IEnumerable exp = from ress in root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                              select new
                              {
                                  Time = ress.Time.Date,
                                  Date = ress.Time.ToLongTimeString(),
                                  RCA = (ress.ChannelNo == 1) ? (ress.Voltage / ress.Voltage).ToString() : "-",
                                  Rca = (ress.ChannelNo == 2) ? (ress.Voltage / ress.Voltage).ToString() : "-"
                              };
            return exp;
        }

        ////////////////////////////////////////////
        //Funkcii za presmetuvanje na vrednostite//
        //////////////////////////////////////////


        /// <summary>
        /// Пресметка на R1AtStdTemp
        /// </summary>
        /// <returns></returns>
        private double evalR1AtStdTemp()
        {
            //  f(R1Cold, StdTemp, TempCoeffR1)
            return -1;
        }

        /// <summary>
        /// Пресметка на R2AtStdTemp
        /// </summary>
        /// <returns></returns>
        private double evalR2AtStdTemp()
        {
            //  f(R2Cold, StdTemp, TempCoeffR2)
            return -1;
        }
        /// <summary>
        /// Пресметка на R1Phase
        /// </summary>
        /// <returns></returns>
        private double evalR1Phase()
        {
            //   f(R1AtStdTemp, HV)
            return -1;
        }
        /// <summary>
        /// Пресметка на R2Phase
        /// </summary>
        /// <returns></returns>
        private double evalR2Phase()
        {
            //   f(R2AtStdTemp, HV)
            return -1;
        }
        /// <summary>
        /// Пресметка на R1Cold
        /// </summary>
        /// <returns></returns>
        private double evalR1Cold()
        {
            // Средна вредност од мерените отпори за првиот канал (се пресметува).
            return -1;
        }
        /// <summary>
        /// Пресметка на R2Cold
        /// </summary>
        /// <returns></returns>
        private double evalR2Cold()
        {
            // Средна вредност од мерените отпори за вториот канал (се пресметува).
            return -1;
        }

        private double evalTCold() 
        {
            //Средна вредност од температурите.    
            return -1;
        }

        ////////////////////////////


        /// <summary>
        /// Пресметка на стандарна девијација на мерените отпори на првиот канал.
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        private double evalStdDevTempR1(int channelIndex)
        {
            double r1M = 0;
            double r1StdDev = 0;

            var r1 = from ress in root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                     where ress.ChannelNo == 0
                     select new { Ressistance = ress.Voltage / ress.Current };

            foreach (var rIter in r1)
            {
                r1M += (double)rIter.Ressistance;
            }
            r1M /= r1.Count();

            foreach (var rIter in r1)
            {
                r1StdDev += Math.Pow((double)rIter.Ressistance - r1M, 2);
            }

            r1StdDev = Math.Sqrt(r1StdDev / r1.Count());

            return r1StdDev;
        }
        /// <summary>
        /// Пресметка на стандарна девијација на мерените отпори на вториот канал.
        /// </summary>
        /// <param name="channelIndex">Кој канал</param>
        /// <returns></returns>
        private double evalStdDevTempR2(int channelIndex)
        {
            double r2M = 0;
            double r2StdDev = 0;

            //Оделување на отпорите во низа
            var r2 = from ress in root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                     where ress.ChannelNo == 1
                     select new { Ressistance = ress.Voltage / ress.Current };

            //Средна вредност
            foreach (var rIter in r2)
            {
                r2M += (double)rIter.Ressistance;
            }
            r2M /= r2.Count();


            foreach (var rIter in r2)
            {
                r2StdDev += Math.Pow((double)rIter.Ressistance - r2M, 2);
            }

            r2StdDev = Math.Sqrt(r2StdDev / r2.Count());

            return r2StdDev;
        }

        #endregion
    }
}