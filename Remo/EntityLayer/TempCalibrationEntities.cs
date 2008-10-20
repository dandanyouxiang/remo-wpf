using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace EntityLayer
{
    [Serializable]
    public class TempCalibration
    {
        public ListWithChangeEvents<TempCalMeasurenment> TempCalMeasurenments { get; set; }

        #region XmlSerialize services
        private Type[] types = new Type[] { 
                        typeof(TempCalibration),
                        typeof(ListWithChangeEvents<TempCalMeasurenment>),
                        typeof(TempCalMeasurenment)
                     };

        public void writeToXml(string path)
        {
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(TempCalibration), types);
                    xmlFormat.Serialize(fStream, this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void readXml(string path)
        {
            TempCalibration root;
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(TempCalibration), types);
                    root = (TempCalibration)xmlFormat.Deserialize(fStream);
                }
                this.TempCalMeasurenments = root.TempCalMeasurenments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        #endregion
    }
    [Serializable]
    public class TempCalMeasurenment : INotifyPropertyChanged
    {
        private DateTime _time;
        /// <summary>
        /// Време на мерењето
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Time"));
                }
            }
        }
        private double _t1;
        /// <summary>
        /// Измерена температура од канал бр. 1
        /// </summary>
        public double T1
        {
            get { return _t1; }
            set
            {
                if (_t1 != value)
                {
                    _t1 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T1"));
                }
            }
        }
        private double _t2;
        /// <summary>
        /// Измерена температура од канал бр. 2
        /// </summary>
        public double T2
        {
            get { return _t2; }
            set
            {
                if (_t2 != value)
                {
                    _t2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T2"));
                }
            }
        }

        private double _t3;
        /// /// <summary>
        /// Измерена температура од канал бр. 3
        /// </summary>
        public double T3
        {
            get { return _t3; }
            set
            {
                if (_t3 != value)
                {
                    _t3 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T3"));
                }
            }
        }

        private double _t4;
        /// /// <summary>
        /// Измерена температура од канал бр. 4
        /// </summary>
        public double T4
        {
            get { return _t4; }
            set
            {
                if (_t4 != value)
                {
                    _t4 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T4"));
                }
            }
        }

        private double _t1Ref;
        /// <summary>
        /// Референтна температура за канал бр. 1
        /// </summary>
        public double T1Ref
        {
            get { return _t1Ref; }
            set
            {
                if (_t1Ref != value)
                {
                    _t1Ref = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T1Ref"));
                }
            }
        }
        private double _t2Ref;
        /// <summary>
        /// Референтна температура за канал бр. 2
        /// </summary>
        public double T2Ref
        {
            get { return _t2Ref; }
            set
            {
                if (_t2Ref != value)
                {
                    _t2Ref = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T2Ref"));
                }
            }
        }

        private double _t3Ref;
        /// /// <summary>
        ///Референтна температура за канал бр. 3
        /// </summary>
        public double T3Ref
        {
            get { return _t3Ref; }
            set
            {
                if (_t3Ref != value)
                {
                    _t3Ref = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T3Ref"));
                }
            }
        }

        private double _t4Ref;
        /// /// <summary>
        /// Референтна температура за канал бр. 4
        /// </summary>
        public double T4Ref
        {
            get { return _t4Ref; }
            set
            {
                if (_t4Ref != value)
                {
                    _t4Ref = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T4Ref"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        public TempCalMeasurenment()
        {
        }
        //Todo не е добро PresentationLayer да е свесен за EntityLayer
        public TempCalMeasurenment(DateTime time, double t1, double t1Ref, double t2, double t2Ref, double t3, double t3Ref, double t4, double t4Ref)
        {
            Time = time;
            T1 = t1;
            T1Ref = t1Ref;
            T2 = t2;
            T2Ref = t2Ref;
            T3 = t3;
            T3Ref = t3Ref;
            T4 = t4;
            T4Ref = t4Ref;
            
        }
    }
}
