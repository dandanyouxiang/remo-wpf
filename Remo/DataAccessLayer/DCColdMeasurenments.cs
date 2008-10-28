using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    public enum TempChannelType 
    {
        A_C,
        B_C,
        C_A
    }

    public class TableTempHeader 
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string T1 { get; set; }
        public string T2 { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TAmb { get; set; }
        public string TOil { get; set; }

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

    public class TableRessHeader
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }

        public TableRessHeader(string date, string time, string r1, string r2)
        {
            this.Date = date;
            this.Time = time;
            this.R1 = r1;
            this.R2 = r2;
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

                    private int _selectedChannel;
                    private TableTempHeader DCColdTemperatureTableHdr;
     
            #endregion

            #region Public Properties

                    public static List<string> Channels = new List<string> { "A-B", "B-C", "C-A" };

                    public int SelectedChannel 
                    {
                        get { return _selectedChannel; }
                        set 
                        {
                            if (_selectedChannel != value) 
                            {
                                _selectedChannel = value;

                                OnPropertyChanged(new PropertyChangedEventArgs(null));

                                //labelite nad polinjata za otporite
                                
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
            IEnumerable exp;
            var tempch = Root.DcColdMeasurenments.TempMeasurenementConfiguration;
            try
            {
                            exp = from temp in Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments
                                  select new
                                  {
                                      Date = temp.Time.Date.ToShortDateString(),
                                      Time = temp.Time.ToLongTimeString(),
                                      T1 = temp.T1,
                                      T2 = temp.T2,
                                      T3 = temp.T3,
                                      T4 = temp.T4,
                                      TAmb = (((!tempch.IsChannel1Oil && tempch.IsChannel1On) ? temp.T1 : 0) + ((!tempch.IsChannel2Oil && tempch.IsChannel2On) ? temp.T2 : 0) + ((!tempch.IsChannel3Oil && tempch.IsChannel3On) ? temp.T3 : 0) + ((!tempch.IsChannel4Oil && tempch.IsChannel4On) ? temp.T4 : 0)) / (((!tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0) + ((!tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0) + ((!tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0) + ((!tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)),
                                      TOil = (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? temp.T1 : 0) + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? temp.T2 : 0) + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? temp.T3 : 0) + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? temp.T4 : 0)) / (((tempch.IsChannel1Oil && tempch.IsChannel1On) ? 1 : 0) + ((tempch.IsChannel2Oil && tempch.IsChannel2On) ? 1 : 0) + ((tempch.IsChannel3Oil && tempch.IsChannel3On) ? 1 : 0) + ((tempch.IsChannel4Oil && tempch.IsChannel4On) ? 1 : 0)),


                                  };
            }
            catch 
            {
                return null;
            }
            return exp;
        }

        public TableTempHeader evalDCColdTemperatureTableHeader()
        {
            var tempch = Root.DcColdMeasurenments.TempMeasurenementConfiguration;
            return new TableTempHeader("Date","Time","T1" + ((tempch.IsChannel1Oil) ? "(Oil) " : "(Amb) "),"T2" + ((tempch.IsChannel2Oil) ? "(Oil) " : "(Amb) "),"T3" + ((tempch.IsChannel3Oil) ? "(Oil) " : "(Amb) "),"T4" + ((tempch.IsChannel4Oil) ? "(Oil) " : "(Amb) "),"T Amb","T Oil");
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
            for (int i = 0; i < Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T1;
            }
            retValue /= Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT1MeanDCColdTempTableHeader()
        {
            return ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil) ? "T1(Oil)" : "T1(Amb)");
        }

        /// <summary>
        /// Ја пресметува средната вредноста за вториот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT2MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T2;
            }
            retValue /= Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT2MeanDCColdTempTableHeader()
        {
            return ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil) ? "T2(Oil)" : "T2(Amb)");
        }
        /// <summary>
        /// Ја пресметува средната вредноста за третиот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT3MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T3;
            }
            retValue /= Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }
        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT3MeanDCColdTempTableHeader()
        {
            return ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil) ? "T3(Oil)" : "T3(Amb)");
        }
        /// <summary>
        /// Ја пресметува средната вредноста за четвртиот канал.
        /// </summary>
        /// <returns></returns>
        private double evalT4MeanDCColdTempTable()
        {
            double retValue = 0;
            for (int i = 0; i < Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count; i++)
            {
                retValue += Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments[i].T4;
            }
            retValue /= Root.DcColdMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
            return retValue;
        }

        /// <summary>
        /// Што да пишува над келијата.
        /// </summary>
        /// <returns></returns>
        private string evalT4MeanDCColdTempTableHeader()
        {
            return ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil) ? "T4(Oil)" : "T4(Amb)");
        }
        /// <summary>
        /// Пресметка на средната вредност на температурата во масло.
        /// </summary>
        /// <returns></returns>
        private double evalOilDCColdTempTable() 
        {
            int channelInOil = 
                ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1On) ? 1 : 0)
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2On) ? 1 : 0)
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3On) ? 1 : 0) 
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4On) ? 1 : 0);
            double channelInOilSum =
                ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1On) ? T1MeanDCColdTempTable : 0)
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2On) ? T2MeanDCColdTempTable : 0)
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3On) ? T3MeanDCColdTempTable : 0)
                + ((Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4On) ? T4MeanDCColdTempTable : 0);

            return channelInOil == 0 ? double.NaN : channelInOilSum / channelInOil;
        }

        /// <summary>
        /// Пресметка на средната вредност на температурата во воздух.
        /// </summary>
        /// <returns></returns>
        private double evalAmbDCColdTempTable()
        {
            int channelsInAmb =
                ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1On) ? 1 : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2On) ? 1 : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3On) ? 1 : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4On) ? 1 : 0);
            double channelInAmbSum =
                ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel1On) ? T1MeanDCColdTempTable : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel2On) ? T2MeanDCColdTempTable : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel3On) ? T3MeanDCColdTempTable : 0)
                + ((!Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4Oil && Root.DcColdMeasurenments.TempMeasurenementConfiguration.IsChannel4On) ? T4MeanDCColdTempTable : 0);

            return channelInAmbSum / channelsInAmb;
        }

        ////////////////////////////////////////////////////

        #endregion
        #region DCColdRessis

        ///////////////////////////////
        #region Private memebers

            private double _r1AtStdTemp;
            private double _r2AtStdTemp;
            private double _r1Phase;
            private double _r2Phase;
            private double _stdTemp;
            private string _r1AtStdTempHeader;
            private string _r2AtStdTempHeader;
            private string _r1PhaseHeader;
            private string _r2PhaseHeader;
           
            /// <summary>
            /// Средни вредности за првиот отпонички канал.
            /// </summary>
            private double _r1Cold;
            /// <summary>
            /// Средни вредности за вториот отпонички канал.
            /// </summary>
            private double _r2Cold;          

            private double StdDevTR1;
            private double StdDevTR2;
            private string StdDevTR1Hdr;
            private string StdDevTR2Hdr;

            private TableRessHeader DCColdRessistanceTableHdr;    

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
                get { return _r1AtStdTemp; }
                set
                {
                    if (_r1AtStdTemp != value)
                    {
                        _r1AtStdTemp = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1AtStdTemp"));
                    }
                }
            }

            public double R2AtStdTemp
            {
                get { return _r2AtStdTemp; }
                set
                {
                    if (_r2AtStdTemp != value)
                    {
                        _r2AtStdTemp = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2AtStdTemp"));
                    }
                }
            }

            public double R1Phase
            {
                get { return _r1Phase; }
                set
                {
                    if (_r1Phase != value)
                    {
                        _r1Phase = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1Phase"));
                    }
                }
            }

            public double R2Phase
            {
                get { return _r2Phase; }
                set
                {
                    if (_r2Phase != value)
                    {
                        _r2Phase = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2Phase"));
                    }
                }
            }

            public string R1AtStdTempHeader
            {
                get { return _r1AtStdTempHeader; }
                set
                {
                    if (_r1AtStdTempHeader != value)
                    {
                        _r1AtStdTempHeader = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1AtStdTempHeader"));
                    }
                }
            }

            public string R2AtStdTempHeader
            {
                get { return _r2AtStdTempHeader; }
                set
                {
                    if (_r2AtStdTempHeader != value)
                    {
                        _r2AtStdTempHeader = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2AtStdTempHeader"));
                    }
                }
            }

            public string R1PhaseHeader
            {
                get { return _r1PhaseHeader; }
                set
                {
                    if (_r1PhaseHeader != value)
                    {
                        _r1PhaseHeader = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1PhaseHeader"));
                    }
                }
            }

            public string R2PhaseHeader
            {
                get { return _r2PhaseHeader; }
                set
                {
                    if (_r2PhaseHeader != value)
                    {
                        _r2PhaseHeader = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2PhaseHeader"));
                    }
                }
            }
            
            /// <summary>
            /// Средна вредност од измерените отпори за првиот отпорнички канал.
            /// </summary>
            public double R1Cold
            {
                get { return _r1Cold; }
                set
                {
                    if (_r1Cold != value)
                    {
                        _r1Cold = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R1Cold"));
                    }
                }
            }
            /// <summary>
            /// Средна вредност од измерените отпори за вториот отпорничи канал.
            /// </summary>
            public double R2Cold
            {
                get { return _r2Cold; }
                set
                {
                    if (_r2Cold != value)
                    {
                        _r2Cold = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("R2Cold"));
                    }
                }
            }

            /// <summary>
            /// Се задава, за пресметување на полињата подоле.
            /// </summary>
            public double StdTemp
            {
                get { return _stdTemp; }
                set
                {
                    if (_stdTemp != value)
                    {
                        _stdTemp = value;
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
            public string StdDevTempR1Header
            {
                get { return StdDevTR1Hdr; }
                set
                {
                    if (StdDevTR1Hdr != value)
                    {
                        StdDevTR1Hdr = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("StdDevTempR1Header"));
                    }
                }
            }
            public string StdDevTempR2Header
            {
                get { return StdDevTR2Hdr; }
                set
                {
                    if (StdDevTR2Hdr != value)
                    {
                        StdDevTR2Hdr = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("StdDevTempR2Header"));
                    }
                }
            }

            public TableRessHeader DCColdRessistanceTableHeader 
            {
                get { return DCColdRessistanceTableHdr; }
                set 
                {
                    if (DCColdRessistanceTableHdr != value) 
                    {
                        DCColdRessistanceTableHdr = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("DCColdRessistanceTableHeader"));
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
            IEnumerable exp;
            try
            {
                exp = from ress in Root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                      select new
                      {
                          Date = ress.Time.Date.ToShortDateString(),
                          Time = ress.Time.ToLongTimeString(),
                          RCA = (ress.ChannelNo == 1) ? (ress.Voltage / ress.Current) : double.NaN,
                          Rca = (ress.ChannelNo == 2) ? (ress.Voltage / ress.Current) : double.NaN
                      };
            }
            catch 
            {
                return null;
            }
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
            double r1TempCoeff = Root.TransformerProperties.HvTempCoefficient;
            double value = R1Cold * (StdTemp + r1TempCoeff) / (TCold + r1TempCoeff);
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// Пресметка на R2AtStdTemp
        /// </summary>
        /// <returns></returns>
        private double evalR2AtStdTemp()
        {
            double r2TempCoeff = Root.TransformerProperties.LvTempCoefficient;
            double value = R2Cold * (StdTemp + r2TempCoeff) / (TCold + r2TempCoeff);
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// Пресметка на R1Phase
        /// </summary>
        /// <returns></returns>
        private double evalR1Phase()
        {
            double val1 = 0.5;
            if (Root.TransformerProperties.HV == EntityLayer.TransformerProperties.ConnectionType.D)
                val1 = 1.5;
            double value = evalR1AtStdTemp() * val1;
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// Пресметка на R2Phase
        /// </summary>
        /// <returns></returns>
        private double evalR2Phase()
        {
            double val1 = 0.5;
            if (Root.TransformerProperties.LV == EntityLayer.TransformerProperties.ConnectionType.D)
                val1 = 1.5;
            double value = evalR2AtStdTemp() * val1;
            return value == 0 ? double.NaN : value;
        }
        /// <summary>
        /// Пресметка на R1Cold. Средна вредност од мерените отпори за првиот канал (се пресметува).
        /// </summary>
        /// <returns></returns>
        private double evalR1Cold(int selectedChannel)
        {
           double sum = 0;
           double n = 0;
           foreach(EntityLayer.RessistanceMeasurenment meas in Root.DcColdMeasurenments.RessistanceTransformerChannels[selectedChannel].RessistanceMeasurenments)
           {
               if(meas.ChannelNo == 1)
               {
                   sum += meas.Voltage / meas.Current;
                   n++;
               }
           }
           if (n == 0)
               return double.NaN;
           else
               return sum / n;
        }
        /// <summary>
        /// Пресметка на R2Cold. Средна вредност од мерените отпори за вториот канал (се пресметува).
        /// </summary>
        /// <returns></returns>
        private double evalR2Cold(int selectedChannel)
        {
            double sum = 0;
            double n = 0;
            foreach (EntityLayer.RessistanceMeasurenment meas in Root.DcColdMeasurenments.RessistanceTransformerChannels[selectedChannel].RessistanceMeasurenments)
            {
                if (meas.ChannelNo == 2)
                {
                    sum += meas.Voltage / meas.Current;
                    n++;
                }
            }
            if (n == 0)
                return double.NaN;
            else
                return sum / n;
        }
        private double evalTCold() 
        {
            return OilDCColdTempTable;
        }
        
        ////////////////////////////


        /// <summary>
        /// Пресметка на стандарна девијација на мерените отпори на првиот канал.
        /// </summary>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        private double evalStdDevTempR1(int channelIndex)
        {
            double mean = 0;
            double stdDev = 0;

            var ressistances = from ress in Root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                     where ress.ChannelNo == 1
                     select new { Ressistance = ress.Voltage / ress.Current };
            if (ressistances.Count() == 0)
                return double.NaN;
            else
            {
                //Sum
                foreach (var rIter in ressistances)
                {
                    mean += (double)rIter.Ressistance;
                }
                //Mean
                mean /= ressistances.Count();

                foreach (var rIter in ressistances)
                {
                    stdDev += Math.Pow((double)rIter.Ressistance - mean, 2);
                }

                stdDev = Math.Sqrt(stdDev / ressistances.Count());

                return stdDev;
            }
        }
        /// <summary>
        /// Пресметка на стандарна девијација на мерените отпори на вториот канал.
        /// </summary>
        /// <param name="channelIndex">Кој канал</param>
        /// <returns></returns>
        private double evalStdDevTempR2(int channelIndex)
        {
            double mean = 0;
            double stdDev = 0;

            var ressistances = from ress in Root.DcColdMeasurenments.RessistanceTransformerChannels[channelIndex].RessistanceMeasurenments
                               where ress.ChannelNo == 2
                               select new { Ressistance = ress.Voltage / ress.Current };
            if (ressistances.Count() == 0)
                return double.NaN;
            else
            {
                //Sum
                foreach (var rIter in ressistances)
                {
                    mean += (double)rIter.Ressistance;
                }
                //Mean
                mean /= ressistances.Count();

                foreach (var rIter in ressistances)
                {
                    stdDev += Math.Pow((double)rIter.Ressistance - mean, 2);
                }

                stdDev = Math.Sqrt(stdDev / ressistances.Count());

                return stdDev;
            }
        }

        private string evalR1AtStdTempHeader() 
        {
            switch (SelectedChannel) 
            {
                case 0:return "R A - B"; 
                case 1:return "R B - C"; 
                case 2:return "R C - A"; 
                default: return "R A - B"; 
            }
        }

        private string evalR2AtStdTempHeader()
        {
            switch (SelectedChannel)
            {
                case 0: return "R a - b"; 
                case 1: return "R b - c"; 
                case 2: return "R c - a"; 
                default: return "R a - b";
            }
        }

        private string evalR1PhaseHeader() 
        {
            switch (SelectedChannel)
            {
                case 0: return "R A"; 
                case 1: return "R B"; 
                case 2: return "R C"; 
                default: return "R A";
            }
        }

        private string evalR2PhaseHeader()
        {
            switch (SelectedChannel)
            {
                case 0: return "R a";
                case 1: return "R b"; 
                case 2: return "R c"; 
                default: return "R a"; 
            }
        }
        private string evalStdDevTempR1Header() 
        {
            switch (SelectedChannel)
            {
                case 0: return "R A - B Std Dev";
                case 1: return "R B - C Std Dev";
                case 2: return "C - A Std Dev";
                default: return "R A - B Std Dev"; 
            }
        }

        private string evalStdDevTempR2Header()
        {
            switch (SelectedChannel)
            {
                case 0: return "R a - b Std Dev";
                case 1: return "R b - c Std Dev";
                case 2: return "R c - a Std Dev";
                default: return "R a - b Std Dev";
            }
        }
        private TableRessHeader evalDCColdRessistanceTableHeader() 
        {
            string r1="";
            string r2="";

            switch (SelectedChannel)
            {
                case 0: r1 = "R A - B"; r2 = "R a-b"; break;
                case 1: r1 = "R B - C"; r2 = "R b-c"; break;
                case 2: r1 = "R C - A"; r2 = "R c-a"; break;
                default: r1 = "R A - B"; r2 = "R a-b"; break;
            }
            return new TableRessHeader("Date", "Time", r1, r2);
        }

        public void setValuesForSelectedChannel() 
        {
            R1AtStdTempHeader = evalR1AtStdTempHeader();
            R1PhaseHeader = evalR1PhaseHeader();

            R2AtStdTempHeader = evalR2AtStdTempHeader();
            R2PhaseHeader = evalR2PhaseHeader();

            StdDevTempR1Header = evalStdDevTempR1Header();
            StdDevTempR2Header = evalStdDevTempR2Header();

            DCColdRessistanceTableHeader=evalDCColdRessistanceTableHeader();
        }

        #endregion
    }
}
