using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EntityLayer
{
    /// <summary>
    /// Заради типот при серијализација.
    /// </summary>
    [Serializable]
    public class Property
    {
        public Property()
        {
        }
    }
    [Serializable]
    public class Property<T> : Property, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string name;
        public String Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private T val;
        public T Value
        {
            get
            {
                return val;
            }
            set
            {
                if (val == null || !val.Equals(value))
                {
                    val = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        public Property()
        {
        }
        public Property(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
