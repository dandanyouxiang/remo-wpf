using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EntityLayer
{
    [Serializable]
    public class RessistanceCalibration
    {
        public ListWithChangeEvents<RessistanceCalMeasurenment> RessistanceCalMeasurenments { get; set; }
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
            get { return RErr/RMeas; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}
