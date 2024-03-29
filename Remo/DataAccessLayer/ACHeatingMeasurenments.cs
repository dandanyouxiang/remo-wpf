﻿using System;
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
        private ACTableHeader _acHeatingTableHeader;
        private int _minSampleRate;
        private int _secondsSampleRate = 1;
        private int _samplesDone;
        #endregion

        #region Public Memebers

        public int MinutesSampleRate 
        {
            get { return _minSampleRate; }
            set 
            {
                if (_minSampleRate != value)
                {
                    _minSampleRate = value;
                    Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState = _minSampleRate * 60 + _secondsSampleRate;
                    OnPropertyChanged(new PropertyChangedEventArgs("MinutesSampleRate"));
                }
            }
        }

        public int SecondesSampleRate 
        {
            get { return _secondsSampleRate; }
            set 
            {
                if (_secondsSampleRate != value) 
                {
                    _secondsSampleRate = value;
                    Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState = _minSampleRate * 60 + _secondsSampleRate;
                    OnPropertyChanged(new PropertyChangedEventArgs("SecondesSampleRate"));
                }
            }
        }

        public int SamplesDone 
        {
            get { return _samplesDone; }
            set 
            {
                if (_samplesDone != value)
                {
                    _samplesDone = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SamplesDone"));
                }
            }
        }

        public ACTableHeader ACHeatingTableHeader 
        {
            get { return _acHeatingTableHeader; }
            set 
            {
                if (_acHeatingTableHeader != value) 
                {
                    _acHeatingTableHeader = value;
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
            var acch = Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            IEnumerable ACValues = from ac in Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments
                                   select new
                                   {
                                       Date = ac.Time.ToShortDateString(),
                                       Time = ac.Time.ToLongTimeString(),
                                       No = Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.IndexOf(ac) + 1,
                                       T1 = ac.T1,
                                       T2 = ac.T2,
                                       T3 = ac.T3,
                                       T4 = ac.T4,
                                       TAmb = (((!acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / (((!acch.IsChannel1Oil && acch.IsChannel1On) ? 1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? 1 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? 1 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? 1 : 0)),
                                       TOil = (((acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / (((acch.IsChannel1Oil && acch.IsChannel1On) ? 1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? 1 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? 1 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? 1 : 0)),
                                       TempRise = (((acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / (((acch.IsChannel1Oil && acch.IsChannel1On) ? 1 : 0) + ((acch.IsChannel2Oil && acch.IsChannel2On) ? 1 : 0) + ((acch.IsChannel3Oil && acch.IsChannel3On) ? 1 : 0) + ((acch.IsChannel4Oil && acch.IsChannel4On) ? 1 : 0)) - (((!acch.IsChannel1Oil && acch.IsChannel1On) ? ac.T1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? ac.T2 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? ac.T3 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? ac.T4 : 0)) / (((!acch.IsChannel1Oil && acch.IsChannel1On) ? 1 : 0) + ((!acch.IsChannel2Oil && acch.IsChannel2On) ? 1 : 0) + ((!acch.IsChannel3Oil && acch.IsChannel3On) ? 1 : 0) + ((!acch.IsChannel4Oil && acch.IsChannel4On) ? 1 : 0)),
                                       Reduced = ac.IsSampleReduced,
                                   };
            return ACValues;
        }

        private ACTableHeader evalACHeatingTableHeader() 
        {
            var acch = Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            return new ACTableHeader("Time", "No", "T1" + ((acch.IsChannel1Oil) ? "(Oil)" : "(Amb)"), "T2" + ((acch.IsChannel2Oil) ? "(Oil)" : "(Amb)"), "T3" + ((acch.IsChannel3Oil) ? "(Oil)" : "(Amb)"), "T4" + ((acch.IsChannel4Oil) ? "(Oil)" : "(Amb)"), "T Amb", "T Oil", "Temp Rise"); 
        }

        private int evalMinutesSampleRate() 
        {
            return Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState / 60;
        }

        private int evalSecondesSampleRate() 
        {

            return Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempSampleRateCurrentState % 60;
        }
        //mislam deka e vo red
        private int evalSamplesDone() 
        {
            return Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count;
        }

        #endregion
        #endregion
    }
}
