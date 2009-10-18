using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace EntityLayer
{
    #region XmlServices
    public class XmlServices
    {
        private  Type[] types = new Type[] { 
                        typeof(DcColdMeasurenments),
                        typeof(AcHotMeasurenments),
                        typeof(DcCoolingMeasurenments),
                        typeof(TransformerProperties),
                        typeof(ListWithChangeEvents<RessistanceMeasurenment>),
                        typeof(RessistanceMeasurenment), 
                        typeof(ListWithChangeEvents<RessistanceTransformerChannel>),
                        typeof(RessistanceTransformerChannel),
                        typeof(DateTime),
                        typeof(ListWithChangeEvents<TempMeasurenment>),
                        typeof(TempMeasurenementConfiguration),
                        typeof(TempMeasurenment),
                        typeof(Property<int>),
                        typeof(Property<double>),
                        typeof(Property),
                        typeof(int),
                        typeof(bool),
                        typeof(double)
                     };

        public void writeToXml(string path, Root root)
        {
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                     XmlSerializer xmlFormat = new XmlSerializer(typeof(Root), types);
                    xmlFormat.Serialize(fStream, root);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public Root readXml(string path)
        {
            Root root;
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(Root), types);
                    root = (Root)xmlFormat.Deserialize(fStream);
                }
                return root;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public void writeToXmlTest(string path)
        {
            DcColdMeasurenments dcColdMeasurenments = new DcColdMeasurenments(
                new TempMeasurenementConfiguration(true, true,true,true,false, false, false, true, 
                    new ListWithChangeEvents<TempMeasurenment>(){
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 21.1),
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,2),20.1, 20.2, 20.3, 22.1),
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,3),20.1, 20.2, 20.3, 23.1),
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,4),20.1, 20.2, 20.3, 24.1),
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 25.1),
                        new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 26.1)
                    }),
                
                new ListWithChangeEvents<RessistanceTransformerChannel>()
                {
                    new RessistanceTransformerChannel(
                        1, 1, 1, 4, true, true,                       
                        new ListWithChangeEvents<RessistanceMeasurenment>(){
                            new RessistanceMeasurenment( new DateTime(2008, 1, 1, 10, 0, 0), 1, 1, 1),
                            new RessistanceMeasurenment( new DateTime(2008, 1, 1, 10, 0, 1), 2, 2, 1),
                            new RessistanceMeasurenment( new DateTime(2008, 1, 1, 10, 0, 2), 1, 3, 1),
                            new RessistanceMeasurenment( new DateTime(2008, 1, 1, 10, 0, 3), 2, 4, 1)
                            }),
                    new RessistanceTransformerChannel( 1, 1, 1, 4, true, true, new ListWithChangeEvents<RessistanceMeasurenment>()),
                    new RessistanceTransformerChannel( 1, 1, 1, 4, true, true, new ListWithChangeEvents<RessistanceMeasurenment>())
            }
            );
            AcHotMeasurenments acHotMeasurenments=new AcHotMeasurenments(new TempMeasurenementConfiguration(true,true,true,false,true,false,true,true,new ListWithChangeEvents<TempMeasurenment>(){
                            new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 21.1),
                                new TempMeasurenment(new DateTime(2008,1,1,1,1,2),20.1, 20.2, 20.3, 22.1),
                                new TempMeasurenment(new DateTime(2008,1,1,1,1,3),20.1, 20.2, 20.3, 23.1),
                                new TempMeasurenment(new DateTime(2008,1,1,1,1,4),20.1, 20.2, 20.3, 24.1),
                                new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 25.1),
                                new TempMeasurenment(new DateTime(2008,1,1,1,1,1),20.1, 20.2, 20.3, 26.1)
            }));

            Root root = new Root();
            root.DcColdMeasurenments = dcColdMeasurenments;
            root.AcHotMeasurenments = acHotMeasurenments;
            root.DcCoolingMeasurenments = new DcCoolingMeasurenments(new RessistanceTransformerChannel(1,12,1,1,true,false,new ListWithChangeEvents<RessistanceMeasurenment>()
            {
                new RessistanceMeasurenment(DateTime.Now),
                new RessistanceMeasurenment(DateTime.Now),
                new RessistanceMeasurenment(DateTime.Now),
                new RessistanceMeasurenment(DateTime.Now),
            })
                );
            root.TransformerProperties = new TransformerProperties();
                
            writeToXml(path, root);
        }
        public void writeToXmlNew(string path)
        {
            

            Root root = new Root();
            

            writeToXml(path, root);
        }
    }
    #endregion

    /// <summary>
    /// Коренот на XML документот
    /// </summary>
    [Serializable]
    public class Root
    {
        public TransformerProperties TransformerProperties { get; set; }
        public DcColdMeasurenments DcColdMeasurenments { get; set; }
        public AcHotMeasurenments AcHotMeasurenments { get; set; }
        public DcCoolingMeasurenments DcCoolingMeasurenments { get; set; }
        public Root()
        {
            TransformerProperties = new TransformerProperties();
            DcColdMeasurenments = new DcColdMeasurenments(new object());
            AcHotMeasurenments = new AcHotMeasurenments();
            DcCoolingMeasurenments = new DcCoolingMeasurenments();
        }
    }

    /// <summary>
    /// Мерење на ладно
    /// </summary>
    [Serializable]
    public class DcColdMeasurenments
    {
        //Температури
        /// <summary>
        /// Конфигурација на Температурни Канали и вредности на Температурните мерења
        /// </summary>
        public TempMeasurenementConfiguration TempMeasurenementConfiguration { get; set; }

        //Отпори
        /// <summary>
        /// Каналите на трансформаторот и нивните конфигурации и мерења
        /// </summary>
        public ListWithChangeEvents<RessistanceTransformerChannel> RessistanceTransformerChannels { get; set; }

        public DcColdMeasurenments(TempMeasurenementConfiguration tempMeasurenementConfiguration
            , ListWithChangeEvents<RessistanceTransformerChannel> ressistanceTransformerChannels)
        {
            this.TempMeasurenementConfiguration = tempMeasurenementConfiguration;
            this.RessistanceTransformerChannels = ressistanceTransformerChannels;
        }
        
        public DcColdMeasurenments(object anything)
            : this(new TempMeasurenementConfiguration(), 
            new ListWithChangeEvents<RessistanceTransformerChannel>() 
                { new RessistanceTransformerChannel(), new RessistanceTransformerChannel() , new RessistanceTransformerChannel()})
        {
        }
        public DcColdMeasurenments() { }
       
    }
    /// <summary>
    /// Мерење при загревање
    /// </summary>
    [Serializable]
    public class AcHotMeasurenments
    {
        /// <summary>
        /// Конфигурација на Температурни Канали и вредности на Температурните мерења
        /// </summary>
        public TempMeasurenementConfiguration TempMeasurenementConfiguration { get; set; }

        public AcHotMeasurenments(TempMeasurenementConfiguration tempMeasurenementConfiguration)
        {
            this.TempMeasurenementConfiguration = tempMeasurenementConfiguration;
        }
        public AcHotMeasurenments() : this(new TempMeasurenementConfiguration()) { }
    }
    /// <summary>
    /// Мерење при ладење
    /// </summary>
    [Serializable]
    public class DcCoolingMeasurenments
    {
        //Results Data
        public Property<double> EndAcTemp { get; set; }
        public Property<double> KDropInOil { get; set; }
        public Property<double> TCold { get; set; }
        public Property<double> R1Cold { get; set; }
        public Property<double> R2Cold { get; set; }
        public Property<bool?> IsTempDataMeasured { get; set; }
        public Property<int> SelectedDcColdChannel { get; set; }

        //Отпори
        /// <summary>
        /// Каналот за кој се врши мерење на ладно.
        /// </summary>
        public RessistanceTransformerChannel RessistanceTransformerChannel { get; set; }
        /// <summary>
        /// Времето на t=0 
        /// </summary>
        public DateTime TNullTime { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public DcCoolingMeasurenments(RessistanceTransformerChannel ressistanceTransformerChannels)
        {
            RessistanceTransformerChannel = ressistanceTransformerChannels;
            EndAcTemp = new Property<double>("EndAcTemp", double.NaN);
            KDropInOil = new Property<double>("EndAcTemp", double.NaN);
            TCold = new Property<double>("TCold", double.NaN);
            R1Cold = new Property<double>("R1Cold", double.NaN);
            R2Cold = new Property<double>("R2Cold", double.NaN);
            IsTempDataMeasured = new Property<bool?>("IsTempDataMeasured", true);
            SelectedDcColdChannel = new Property<int>("SelectedDcColdChannel", -1);
        }

        public DcCoolingMeasurenments() : this(new RessistanceTransformerChannel(1, double.NaN, -1, -1, true, true, new ListWithChangeEvents<RessistanceMeasurenment>())) { }
    
    }

    public class TransformerProperties : INotifyPropertyChanged
    {
        public enum ConnectionType { D, Y, Z };
        public enum Material { Copper, Aluminium, Other };

        private String _transformatorSeries;
        public String TransformatorSeries
        {
            get { return _transformatorSeries; }
            set
            {
                if (_transformatorSeries != value)
                {
                    _transformatorSeries = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TransformatorSeries"));
                }
            }
        }

        public String _transformatorSerialNo;
        public String TransformatorSerialNo
        {
            get { return _transformatorSerialNo; }
            set
            {
                if (_transformatorSerialNo != value)
                {
                    _transformatorSerialNo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TransformatorSerialNo"));
                }
            }
        }

        public String _presentAtTest;
        public String PresentAtTest
        {
            get { return _presentAtTest; }
            set
            {
                if (_presentAtTest != value)
                {
                    _presentAtTest = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PresentAtTest"));
                }
            }
        }

        private String _comment;
        public String Comment
        {
            get { return _comment; }
            set
            {
                if(_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
                }
            }
        }

        public ConnectionType _hv;
        public ConnectionType HV
        {
            get { return _hv; }
            set
            {
                if (_hv != value)
                {
                    _hv = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HV"));
                }
            }
        }

        public ConnectionType _lv;
        public ConnectionType LV
        {
            get { return _lv; }
            set
            {
                if (_lv != value)
                {
                    _lv = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("LV"));
                }
            }
        }

        private Material _hvMaterial;
        public Material HvMaterial
        {
            get { return _hvMaterial; }
            set
            {
                if (_hvMaterial != value)
                {
                    _hvMaterial = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HvMaterial"));
                }
            }
        }

        private Material _lvMaterial;
        public Material LvMaterial
        {
            get { return _lvMaterial; }
            set
            {
                if (_lvMaterial != value)
                {
                    _lvMaterial = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("LvMaterial"));
                }
            }
        }

        private double _hvTempCoefficient;
        public double HvTempCoefficient
        {
            get { return _hvTempCoefficient; }
            set
            {
                if (_hvTempCoefficient != value)
                {
                    _hvTempCoefficient = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HvTempCoefficient"));
                }
            }
        }

        private double _lvTempCoefficient;
        public double LvTempCoefficient
        {
            get { return _lvTempCoefficient; }
            set
            {
                if (_lvTempCoefficient != value)
                {
                    _lvTempCoefficient = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("LvTempCoefficient"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public TransformerProperties(String transformatorSeries, String transformatorSerialNo,
            String presentAtTest, String comment,
            ConnectionType hv, ConnectionType lv, 
            Material hvMaterial, Material lvMaterial, 
            double hvTempCoefficient, double lvTempCoefficient)
        {
            TransformatorSeries = transformatorSeries;
            TransformatorSerialNo = transformatorSerialNo;
            PresentAtTest = presentAtTest;
            Comment = comment;
            HV = hv;
            LV = lv;
            HvMaterial = hvMaterial;
            LvMaterial = lvMaterial;
            HvTempCoefficient = hvTempCoefficient;
            LvTempCoefficient = lvTempCoefficient;

        }
        public TransformerProperties() : 
            this("", "", "", "", ConnectionType.D, ConnectionType.D, Material.Copper, Material.Copper, 235, 235) 
        { }
    }
    /// <summary>
    /// Состојба на еден канал на трансформаторот
    /// </summary>
    [Serializable]
    public class RessistanceTransformerChannel : INotifyPropertyChanged
    {
        private int _channelNo;
        /// <summary>
        /// Број на каналот на трансформаторот
        /// </summary>
        public int ChannelNo
        {
            get { return _channelNo; }
            set
            {
                if (_channelNo != value)
                {
                    _channelNo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelNo"));
                }
            }
        }

        private double _testCurrent;
        /// <summary>
        /// Струјата со која се вршени мерењата
        /// </summary>
        public double TestCurrent
        {
            get { return _testCurrent; }
            set
            {
                if (_testCurrent != value)
                {
                    _testCurrent = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TestCurrent"));
                }
            }
        }
        private int _sampleRate;
        /// <summary>
        /// SampleRate за мерење на отпор
        /// </summary>
        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (_sampleRate != value)
                {
                    _sampleRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RessistanceSampleRateCurrentState"));
                }
            }
        }

        private int _noOfSamples;
        /// <summary>
        /// NoOfSamples за мерење на отпор
        /// </summary>
        public int NoOfSamples
        {
            get { return _noOfSamples; }
            set
            {
                if (_noOfSamples != value)
                {
                    _noOfSamples = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("RessistanceNoOfSamplesCurrentState"));
                }
            }
        }

        private bool _isChannel1On;
        /// <summary>
        /// Дали Канал 1 е On Или Off
        /// </summary>
        public bool IsChannel1On
        {
            get { return _isChannel1On; }
            set
            {
                if (_isChannel1On != value)
                {
                    _isChannel1On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1On"));
                }
            }
        }
        private bool _isChannel2On;
        /// <summary>
        /// Дали Канал 2 е On Или Off
        /// </summary>
        public bool IsChannel2On
        {
            get { return _isChannel2On; }
            set
            {
                if (_isChannel2On != value)
                {
                    _isChannel2On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel2On"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        /// <summary>
        /// Мерења на отпор
        /// </summary>
        public ListWithChangeEvents<RessistanceMeasurenment> RessistanceMeasurenments { get; set; }
        public void RessistanceMeasurenments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
        public RessistanceTransformerChannel(
            int channelNo, 
            double testCurrent,
            int sampleRate, 
            int noOfSamples,
            bool isChannel1On,
            bool isChannel2On,
            ListWithChangeEvents<RessistanceMeasurenment> ressistanceMeasurenments)
        {
            ChannelNo = channelNo;
            TestCurrent = testCurrent;
            SampleRate = sampleRate;
            NoOfSamples = noOfSamples;
            this.IsChannel1On = isChannel1On;
            this.IsChannel2On = isChannel2On;

            RessistanceMeasurenments = ressistanceMeasurenments;
            RessistanceMeasurenments.PropertyChanged+=new PropertyChangedEventHandler(RessistanceMeasurenments_PropertyChanged);
        }
        public RessistanceTransformerChannel() : this(1, 1, 1, 1, true, true, new ListWithChangeEvents<RessistanceMeasurenment>()) { }
    }

    [Serializable]
    public class RessistanceMeasurenment : INotifyPropertyChanged
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
        private int _channelNo;
        /// <summary>
        /// Број на канал
        /// </summary>
        public int ChannelNo
        {
            get { return _channelNo; }
            set
            {
                if (_channelNo != value)
                {
                    _channelNo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelNo"));
                }
            }
        }

        private double _voltage;
        /// <summary>
        /// Напонот добиен од мерењето
        /// </summary>
        public double Voltage
        {
            get { return _voltage; }
            set
            {
                if (_voltage != value)
                {
                    _voltage = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Voltage"));
                }
            }
        }

        private double _current;
        /// <summary>
        /// Струјата добиена од мерењето
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public RessistanceMeasurenment(DateTime time, int channelNo, double voltage, double current)
        {
            Time = time;
            ChannelNo = channelNo;
            Voltage = voltage;
            Current = current;
        }
        public RessistanceMeasurenment(DateTime time) : this(time, 1, 0, 1) { }
        public RessistanceMeasurenment() : this(DateTime.Now) { }
    }
    [Serializable]
    public class TempMeasurenementConfiguration : INotifyPropertyChanged
    {
        private int _tempSampleRateCurrentState = 1;
        /// <summary>
        /// Тековна состојба на SampleRate за температурни мерења
        /// </summary>
        public int TempSampleRateCurrentState
        {
            get { return _tempSampleRateCurrentState; }
            set
            {
                if (_tempSampleRateCurrentState != value)
                {
                    _tempSampleRateCurrentState = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TempSampleRateCurrentState"));
                }
            }
        }
        private int _tempNoOfSamplesCurrentState = 1;
        /// <summary>
        /// Тековна состојба на NoOfSamples за температурни мерења
        /// </summary>
        public int TempNoOfSamplesCurrentState
        {
            get { return _tempNoOfSamplesCurrentState; }
            set
            {
                if (_tempNoOfSamplesCurrentState != value)
                {
                    _tempNoOfSamplesCurrentState = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TempNoOfSamplesCurrentState"));
                }
            }
        }
        private bool _isChannel1On;
        /// <summary>
        /// Дали Канал 1 е On Или Off
        /// </summary>
        public bool IsChannel1On
        {
            get { return _isChannel1On; }
            set
            {
                if (_isChannel1On != value)
                {
                    _isChannel1On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1On"));
                }
            }
        }
        private bool _isChannel2On;
        /// <summary>
        /// Дали Канал 2 е On Или Off
        /// </summary>
        public bool IsChannel2On
        {
            get { return _isChannel2On; }
            set
            {
                if (_isChannel2On != value)
                {
                    _isChannel2On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel2On"));
                }
            }
        }
        private bool _isChannel3On;
        /// <summary>
        /// Дали Канал 3 е On Или Off
        /// </summary>
        public bool IsChannel3On
        {
            get { return _isChannel3On; }
            set
            {
                if (_isChannel3On != value)
                {
                    _isChannel3On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel3On"));
                }
            }
        }
        private bool _isChannel4On;
        /// <summary>
        /// Дали Канал 4 е On Или Off
        /// </summary>
        public bool IsChannel4On
        {
            get { return _isChannel4On; }
            set
            {
                if (_isChannel4On != value)
                {
                    _isChannel4On = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel4On"));
                }
            }
        }
        private bool _isChannel1Oil;
        /// <summary>
        /// Дали Канал 1 е Oil или Amb
        /// </summary>
        public bool IsChannel1Oil
        {
            get { return _isChannel1Oil; }
            set
            {
                if (_isChannel1Oil != value)
                {
                    _isChannel1Oil = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1Oil"));
                }
            }
        }
        private bool _isChannel2Oil;
        /// <summary>
        /// Дали Канал 2 е Oil или Amb
        /// </summary>
        public bool IsChannel2Oil
        {
            get { return _isChannel2Oil; }
            set
            {
                if (_isChannel2Oil != value)
                {
                    _isChannel2Oil = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel2Oil"));
                }
            }
        }

        private bool _isChannel3Oil;
        /// <summary>
        /// Дали Канал 3 е Oil или Amb
        /// </summary>
        public bool IsChannel3Oil
        {
            get { return _isChannel3Oil; }
            set
            {
                if (_isChannel3Oil != value)
                {
                    _isChannel3Oil = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel3Oil"));
                }
            }
        }

        private bool _isChannel4Oil;
        /// <summary>
        /// Дали Канал 4 е Oil или Amb
        /// </summary>
        public bool IsChannel4Oil
        {
            get { return _isChannel4Oil; }
            set
            {
                if (_isChannel4Oil != value)
                {
                    _isChannel4Oil = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsChannel4Oil"));
                }
            }
        }

        /// <summary>
        /// Температурните мерења
        /// </summary>
        public ListWithChangeEvents<TempMeasurenment> TempMeasurenments { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            //Call this method on the Right Thread
            if (PropertyChanged != null)
            {
                System.Windows.Threading.DispatcherObject d = PropertyChanged.Target as System.Windows.Threading.DispatcherObject;
                if (d == null)
                    PropertyChanged(this, e);
                else
                    ((System.Windows.Threading.DispatcherObject)PropertyChanged.Target).Dispatcher.
                         BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, PropertyChanged, this, e);
            }
        }

        private void TempMeasurenments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        public TempMeasurenementConfiguration(bool isChannel1On, bool isChannel2On, bool isChannel3On, bool isChannel4On, bool isChannel1Oil, bool isChannel2Oil, bool isChannel3Oil, bool isChannel4Oil, ListWithChangeEvents<TempMeasurenment> tempMeasurenments)
        {
            IsChannel1On = isChannel1On;
            IsChannel2On = isChannel2On;
            IsChannel3On = isChannel3On;
            IsChannel4On = isChannel4On;

            IsChannel1Oil = isChannel1Oil;
            IsChannel2Oil = isChannel2Oil;
            IsChannel3Oil = isChannel3Oil;
            IsChannel4Oil = isChannel4Oil;

            TempMeasurenments = tempMeasurenments;
            TempMeasurenments.PropertyChanged += new PropertyChangedEventHandler(TempMeasurenments_PropertyChanged);
        }
        public TempMeasurenementConfiguration() : this(false, false, false, false, false, false, false, false, new ListWithChangeEvents<TempMeasurenment>())
        {
        }
        
    }
    /// <summary>
    /// Едно температурно мерење со мерените вредности на сите канали.
    /// </summary>
    [Serializable]
    public class TempMeasurenment : INotifyPropertyChanged
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
        /// Температура од канал бр. 1
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
        /// Температура од канал бр. 2
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
        /// Температура од канал бр. 3
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
        /// Температура од канал бр. 4
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

        private bool _isSampleReduced;
        /// <summary>
        /// Дали се работи за sample кој е редуциран. Се користи при AC Heating мерење.
        /// Само еден sample може да биде редуциран.
        /// </summary>
        public bool IsSampleReduced
        {
            get { return _isSampleReduced; }
            set { _isSampleReduced = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public TempMeasurenment(DateTime time, double t1, double t2, double t3, double t4)
        {
            Time = time;

            this.T1 = t1;
            this.T2 = t2;
            this.T3 = t3;
            this.T4 = t4;
        }

        public TempMeasurenment(DateTime time) : this(time, 20, 21, 220, 23) { }
        public TempMeasurenment() : this(DateTime.Now) { }
    }
}
