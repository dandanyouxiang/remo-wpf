using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EntityLayer.DataSourceLayer
{
    //////////////
    //Prv tab////
    ////////////
    public class RessistanceMeasurenment:INotifyPropertyChanged
    {
        
        ////////
        
        private int RCA;
        private int RCa;
        private int RC;
        private int Rc;
        /// <summary>
        /// Средни вредности за првиот отпонички канал.
        /// </summary>
        private int RCAMean;
        /// <summary>
        /// Средни вредности за вториот отпонички канал.
        /// </summary>
        private int RCaMean;
        /// <summary>
        /// Ентитен преку кој се поврзуваат податоци.
        /// </summary>
        private EntityLayer.DcColdMeasurenments dcColdMeasurenments;

        //private 

        ///////

        /// <summary>
        /// За промена на пропертијата.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public RessistanceMeasurenment(EntityLayer.DcColdMeasurenments dcColdMeasurenments) 
        {
            this.dcColdMeasurenments = dcColdMeasurenments;
        }
        /// <summary>
        /// За промена на пропертијата.
        /// </summary>
        /// <param name="e"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        /// <summary>
        /// Температурни канали.
        /// </summary>
        public static List<string> Channels = new List<string> { "A-B", "B-C", "C-A" };

        public int R_C_A 
        {
            get { return RCA; }
            set 
            {
                if (RCA != value)
                {
                    RCA = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_C_A")); 
                }
            }
        }

        public int R_C_a
        {
            get { return RCa; }
            set
            {
                if (RCa != value)
                {
                    RCa = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_C_a"));
                }
            }
        }

        public int R_C
        {
            get { return RCa; }
            set
            {
                if (RC != value)
                {
                    RC = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_C"));
                }
            }
        }

        public int R_c
        {
            get { return Rc; }
            set
            {
                if (Rc != value)
                {
                    Rc = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_c"));
                }
            }
        }
        /// <summary>
        /// Средна вредност од измерените отпори за првиот отпорнички канал.
        /// </summary>
        public int R_C_A_Mean
        {
            get { return RCAMean; }
            set
            {
                if (RCAMean != value)
                {
                    RCAMean = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_C_A_Mean"));
                }
            }
        }
        /// <summary>
        /// Средна вредност од измерените отпори за вториот отпорничи канал.
        /// </summary>
        public int R_C_a_Mean
        {
            get { return RCaMean; }
            set
            {
                if (RCaMean != value)
                {
                    RCaMean = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("R_C_a_Mean"));
                }
            }
        }

    }
    public class TemperatureMeasurenment 
    {

    }
    public class DCColdMeasurenment 
    {


        TemperatureMeasurenment temperatureMeasurenment = new TemperatureMeasurenment();
        RessistanceMeasurenment ressistanceMeasurenment = new RessistanceMeasurenment();
    }
    ///////////////

    /////////////
    //Vtor Tab//
    ///////////
    public class ACHeatingMeasurenmentSetting
    {
    }
    public class ACHeatingMeasurenmentResults : INotifyPropertyChanged
    {
        /// <summary>
        /// За промена на пропертијата.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// За промена на пропертијата.
        /// </summary>
        /// <param name="e"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private double T1T0;
        private double T1Rise;
        private double FT1;

        private double T2T0;
        private double T2Rise;
        private double FT2;

        public double T1_T0 
        {
            get { return T1T0; }
            set 
            {
                if (T1T0 != value) 
                {
                    T1T0 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T1_T0")); 
                }
            }
        }
        public double T1_Rise
        {
            get { return T1Rise; }
            set
            {
                if (T1Rise != value)
                {
                    T1Rise = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("T1_Rise"));
                }
            }
        }
        public double F_T1
        {
            get { return T1Rise; }
            set
            {
                if (FT1 != value)
                {
                    FT1 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("F_T1"));
                }
            }
        }
        
    }
    public class ACHeatingMeasurenment 
    {
    
    }
    ///////////

    /////////////
    //Tred Tab//
    //////////
    public class DCCoolingMeasurenmentSetting 
    {
    
    }
    public class DCCoolingMeasurenmentResults
    {
    }
    public class DCCoolingMeasurenment 
    {
    }
    /////////
}
