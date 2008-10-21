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
    public class RessistanceCalibration
    {
        public ListWithChangeEvents<RessistanceCalMeasurenment> RessistanceCalMeasurenments { get; set; }

        #region XmlSerialize services
        private Type[] types = new Type[] { 
                        typeof(RessistanceCalibration),
                        typeof(ListWithChangeEvents<RessistanceCalMeasurenment>),
                        typeof(RessistanceCalMeasurenment)
                     };

        public void writeToXml(string path)
        {
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(RessistanceCalibration), types);
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
            RessistanceCalibration root;
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(RessistanceCalibration), types);
                    root = (RessistanceCalibration)xmlFormat.Deserialize(fStream);
                }
                this.RessistanceCalMeasurenments = root.RessistanceCalMeasurenments;
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
    public class RessistanceCalMeasurenment : INotifyPropertyChanged
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
        private double _rMeas;
        /// <summary>
        /// Измерен отпор
        /// </summary>
        public double RMeas
        {
            get { return _rMeas; }
            set
            {
                if (_rMeas != value)
                {
                    _rMeas = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RMeas"));
                }
            }
        }
        private double _rRef;
        /// <summary>
        /// Референтен отпор
        /// </summary>
        public double RRef
        {
            get { return _rRef; }
            set
            {
                if (_rRef != value)
                {
                    _rRef = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RRef"));
                }
            }
        }
        private double _current;
        /// <summary>
        /// Струјата со која е извршено мерењето
        /// </summary>
        public double Current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Current"));
                }
            }
        }

        public double RErr
        {
            get { return RMeas-RRef; }
        }

        public double RErrPerc
        {
            get { return 100 * RErr / RMeas; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
