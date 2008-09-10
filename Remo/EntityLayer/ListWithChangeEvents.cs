﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace EntityLayer
{
        // A class that works just like List, but sends event
        // notifications whenever the list changes.
    public class ListWithChangeEvents<T> : List<T>, INotifyPropertyChanged where T : INotifyPropertyChanged 
        {

            public new void Add(T value)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(null));
                base.Add(value);
                value.PropertyChanged += new PropertyChangedEventHandler(value_PropertyChanged);
            }
            private void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(null));
            }

            public new void Clear()
            {
                base.Clear();
                OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
            public new bool Remove(T value)
            {
                bool flag = base.Remove(value);
                if (flag == true)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs(null));
                    value.PropertyChanged -= new PropertyChangedEventHandler(value_PropertyChanged);
                }
                return flag;
            }

            public new T this[int index]
            {
                get
                {
                    return base[index];
                }
                set
                {
                    if(base[index].Equals(value) == false)
                    {
                        base[index] = value;
                        OnPropertyChanged(new PropertyChangedEventArgs(null));
                    }
                    
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }

        }

}
