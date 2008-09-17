using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    public class ACTableHeader
    {
        public string Time { get; set; }
        public string No { get; set; }
        public string T1 { get; set; }
        public string T2 { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TAmb { get; set; }
        public string TOil { get; set; }
        public string TempRise { get; set; }

        public ACTableHeader(string time, string no, string t1, string t2, string t3, string t4, string tAmb, string tOil, string tempRise)
        {
            this.Time = time;
            this.No = no;
            this.T1 = t1;
            this.T2 = t2;
            this.T3 = t3;
            this.T4 = t4;
            this.TAmb = tAmb;
            this.TOil = tOil;
            this.TempRise = tempRise;
        }
    } 

    public partial class DataSource
    {
        #region Settings
            
        #endregion
        #region Results
        #region Private Memebers
        private ACTableHeader ACHeatingTableHdr;
        private int MinSampleRate;
        private int SecSampleRate;
        private int SamplesDn;
        #endregion

        #region Public Memebers

        public int MinutesSampleRate 
        {
            get { return MinSampleRate; }
            set 
            {
                if (MinSampleRate != value)
                {
                    MinSampleRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MinutesSampleRate"));
                }
            }
        }

        public int SecondesSampleRate 
        {
            get { return SecSampleRate; }
            set 
            {
                if (SecSampleRate != value) 
                {
                    SecSampleRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SecondesSampleRate"));
                }
            }
        }

        public int SamplesDone 
        {
            get { return SamplesDn; }
            set 
            {
                if (SamplesDn != value)
                {
                    SamplesDn = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SamplesDone"));
                }
            }
        }

        public ACTableHeader ACHeatingTableHeader 
        {
            get { return ACHeatingTableHdr; }
            set 
            {
                if (ACHeatingTableHdr != value) 
                {
                    ACHeatingTableHdr = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ACHeatingTableHeader"));
                }
            }
        }

        /// <summary>
        /// Табела во вториот таб AC-Heating Measurenments,во вториот подтаб Results
        /// </summary>
        /// <returns></returns>
        public IEnumerable ACHeatingTable()
        {
            var acch = root.AcHotMeasurenments.TempMeasurenementConfiguration;
            IEnumerable ACValues = from ac in root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments
                                   select new
                                   {
                                       Time = ac.Time.ToLongTimeString(),
                                       No = root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.IndexOf(ac),
                                       T1 = ac.T1,
                                       T2 = ac.T2,
                                       T3 = ac.T3,
                                       T4 = ac.T4,
                                       TAmb = (((!acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / 4,
                                       TOil = (((acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / 4,
                                       TempRise = ((((!acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / 4) - ((((acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / 4),
                                   };

            return ACValues;
        }
        /*
        public string[] ACHeatingTableHeader()
        {
            string[] strRet = new string[9];
            var acch = root.AcHotMeasurenments.TempMeasurenementConfiguration;

            strRet[0] = "Time";
            strRet[1] = "No";
            strRet[2] = "T1" + ((acch.IsChannel1Oil) ? "(Oil)" : "(Amb)");
            strRet[3] = "T2" + ((acch.IsChannel2Oil) ? "(Oil)" : "(Amb)");
            strRet[4] = "T3" + ((acch.IsChannel3Oil) ? "(Oil)" : "(Amb)");
            strRet[5] = "T4" + ((acch.IsChannel4Oil) ? "(Oil)" : "(Amb)");
            strRet[6] = "T Amb";
            strRet[7] = "T Oil";
            strRet[8] = "Temp Rise";

            return strRet;
        }
         */
        private ACTableHeader evalACHeatingTableHeader() 
        {
            var acch = root.AcHotMeasurenments.TempMeasurenementConfiguration;
            return new ACTableHeader("Time", "No", "T1" + ((acch.IsChannel1Oil) ? "(Oil)" : "(Amb)"), "T2" + ((acch.IsChannel2Oil) ? "(Oil)" : "(Amb)"), "T3" + ((acch.IsChannel3Oil) ? "(Oil)" : "(Amb)"), "T4" + ((acch.IsChannel4Oil) ? "(Oil)" : "(Amb)"), "T Amb", "T Oil", "Temp Rise"); 
        }

        private int evalMinutesSampleRate() 
        {
            return -1;
        }

        private int evalSecondesSampleRate() 
        {
            return -1;
        }

        private int evalSamplesDone() 
        {
            return -1;
        }

        #endregion
        #endregion
    }
}
