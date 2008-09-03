using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace EntityLayer
{
    public static class XmlServices
    {
        public static void writeToXml(string path, Root root)
        {
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(Root),
                    new Type[] { 
                        typeof(DcColdMeasurenments),
                        typeof(AcHotMeasurenments),
                        typeof(DcCoolingMeasurenments),
                        typeof(TransformerProperties),
                        typeof(List<RessistanceChannel>),
                        typeof(RessistanceChannel),
                        typeof(List<RessistanceChannelMeasurenment>),
                        typeof(RessistanceChannelMeasurenment), 
                        typeof(List<RessistanceMeasurenment>),
                        typeof(RessistanceMeasurenment), 
                        typeof(List<RessistanceTransformerChannel>),
                        typeof(RessistanceTransformerChannel),
                        typeof(DateTime),
                        typeof(List<TempChannel>), 
                        typeof(TempChannel), 
                        typeof(List<TempChannelMeasurenment>),
                        typeof(TempChannelMeasurenment),
                        typeof(List<TempMeasurenment>),
                        typeof(TempMeasurenment)
                     });
                    xmlFormat.Serialize(fStream, root);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static Root readXml(string path)
        {
            Root root;
            try
            {
                using (Stream fStream = new FileStream(path,
                                FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(DcColdMeasurenments),
                    new Type[] { 
                        typeof(DcColdMeasurenments),
                        typeof(AcHotMeasurenments),
                        typeof(DcCoolingMeasurenments),
                        typeof(TransformerProperties),
                        typeof(List<RessistanceChannel>),
                        typeof(RessistanceChannel),
                        typeof(List<RessistanceChannelMeasurenment>),
                        typeof(RessistanceChannelMeasurenment), 
                        typeof(List<RessistanceMeasurenment>),
                        typeof(RessistanceMeasurenment), 
                        typeof(List<RessistanceTransformerChannel>),
                        typeof(RessistanceTransformerChannel),
                        typeof(DateTime),
                        typeof(List<TempChannel>), 
                        typeof(TempChannel), 
                        typeof(List<TempChannelMeasurenment>),
                        typeof(TempChannelMeasurenment),
                        typeof(List<TempMeasurenment>),
                        typeof(TempMeasurenment)
                     });
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
        public static void writeToXmlTest(string path)
        {
            DcColdMeasurenments dcColdMeasurenments = new DcColdMeasurenments(
                1, 5,
                new List<TempChannel>() { new TempChannel(1, true, false), new TempChannel(2, true, false) },
                new List<TempChannel>() { new TempChannel(1, true, false), new TempChannel(2, true, false) },
                new List<TempMeasurenment>() { 
                    new TempMeasurenment(
                        new DateTime(2008, 1, 1, 10, 0, 0), 
                        new List<TempChannelMeasurenment>(){
                            new TempChannelMeasurenment(1, 25), 
                            new TempChannelMeasurenment(2, 25) } )
                },
                new List<RessistanceTransformerChannel>()
                {
                    new RessistanceTransformerChannel(
                        1, 1, 1, 1, 
                        new List<RessistanceChannel>(){new RessistanceChannel(1, true), new RessistanceChannel(2, true)},
                        new List<RessistanceChannel>(){new RessistanceChannel(1, true), new RessistanceChannel(2, true)},
                        new List<RessistanceMeasurenment>(){
                            new RessistanceMeasurenment(
                                new DateTime(2008, 1, 1, 10, 0, 0), 
                                new List<RessistanceChannelMeasurenment>() {
                                    new RessistanceChannelMeasurenment(1, 1, 1),
                                    new RessistanceChannelMeasurenment(2, 10,1)} )
                            }
                            )
                        }
                );

            Root root = new Root();
            root.DcColdMeasurenments = dcColdMeasurenments;
            root.AcHotMeasurenments = new AcHotMeasurenments();
            root.DcCoolingMeasurenments = new DcCoolingMeasurenments();
            root.TransformerProperties = new TransformerProperties();
                
            writeToXml(path, root);
        }
    }

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
    }

    /// <summary>
    /// Мерење на ладно
    /// </summary>
    [Serializable]
    public class DcColdMeasurenments
    {
        //Температури   
        /// <summary>
        /// Тековна состојба на SampleRate за температурни мерења
        /// </summary>
        public int TempSampleRateCurrentState { get; set; }
        /// <summary>
        /// Тековна состојба на NoOfSamples за температурни мерења
        /// </summary>
        public int TempNoOfSamplesCurrentState { get; set; }
        /// <summary>
        /// Тековна состојба на каналите за температурни мерења
        /// </summary>
        public List<TempChannel> TempChannelsCurrentState { get; set; }
        /// <summary>
        /// Состојба на каналите за самите температурни мерења
        /// </summary>
        public List<TempChannel> TempChannelsMeasurenmentConfiguration { get; set; }
        /// <summary>
        /// Температурните мерења
        /// </summary>
        public List<TempMeasurenment> TempMeasurenments { get; set; }

        //Отпори
        /// <summary>
        /// Каналите на трансформаторот и нивните конфигурации и мерења
        /// </summary>
        public List<RessistanceTransformerChannel> RessistanceTransformerChannels { get; set; }

        public DcColdMeasurenments(int tempSampleRateCurrentState, int tempNoOfSamplesCurrentState,
            List<TempChannel> tempChannelsCurrentState, List<TempChannel> tempChannelsMeasurenmentConfiguration,
            List<TempMeasurenment> tempMeasurenments, List<RessistanceTransformerChannel> ressistanceTransformerChannels)
        {
            TempSampleRateCurrentState = tempSampleRateCurrentState;
            TempNoOfSamplesCurrentState = tempNoOfSamplesCurrentState;

            TempChannelsCurrentState = tempChannelsCurrentState;
            TempChannelsMeasurenmentConfiguration = tempChannelsMeasurenmentConfiguration;

            TempMeasurenments = tempMeasurenments;
            RessistanceTransformerChannels = ressistanceTransformerChannels;
        }
        public DcColdMeasurenments()
            : this(1, 1, new List<TempChannel>(), new List<TempChannel>(),
                    new List<TempMeasurenment>(), new List<RessistanceTransformerChannel>())
        {
        }
       
    }
    /// <summary>
    /// Мерење при загревање
    /// </summary>
    [Serializable]
    public class AcHotMeasurenments
    {
        /// <summary>
        /// Тековна состојба на SampleRate за температурни мерења
        /// </summary>
        public int TempSampleRateCurrentState { get; set; }
        /// <summary>
        /// Тековна состојба на каналите за температурни мерења
        /// </summary>
        public List<TempChannel> TempChannelsCurrentState { get; set; }
        /// <summary>
        /// Состојба на каналите за самите температурни мерења
        /// </summary>
        public List<TempChannel> TempChannelsMeasurenmentConfiguration { get; set; }
        /// <summary>
        /// Време на извршена редукција
        /// </summary>
        public DateTime TimeOfReduction { get; set; }
        /// <summary>
        /// Температурните мерења
        /// </summary>
        public List<TempMeasurenment> TempMeasurenments { get; set; }

        public AcHotMeasurenments(int tempSampleRateCurrentState, List<TempChannel> tempChannelsCurrentState, List<TempChannel> tempChannelsMeasurenmentConfiguration, DateTime timeOfReduction, List<TempMeasurenment> tempMeasurenments)
        {
            TempSampleRateCurrentState = tempSampleRateCurrentState;
            TempChannelsCurrentState = tempChannelsCurrentState;
            TempChannelsMeasurenmentConfiguration = tempChannelsMeasurenmentConfiguration;
            TimeOfReduction = timeOfReduction;
            TempMeasurenments = tempMeasurenments;
        }
        public AcHotMeasurenments() : this(1, new List<TempChannel>(), new List<TempChannel>(), DateTime.Now, new List<TempMeasurenment>()) { }
    }
    /// <summary>
    /// Мерење при ладење
    /// </summary>
    [Serializable]
    public class DcCoolingMeasurenments
    {
        //Отпори
        /// <summary>
        /// Каналот за кој се врши мерење на ладно негоците конфигурации и мерења
        /// </summary>
        public RessistanceTransformerChannel RessistanceTransformerChannels { get; set; }

        public DcCoolingMeasurenments(RessistanceTransformerChannel ressistanceTransformerChannels)
        {
            RessistanceTransformerChannels = ressistanceTransformerChannels;
        }
        public DcCoolingMeasurenments() : this(new RessistanceTransformerChannel()) { }
    }
    public class TransformerProperties
    {
        public enum ConnectionType { D, Y, Z };
        public enum Material { Copper, Aluminium, Other };

        public String TransformatorSeries { get; set; }
        public String TransformatorSerialNo { get; set; }
        public String PresentAtTest { get; set; }
        public String Comment { get; set; }
        public ConnectionType HV { get; set; }
        public ConnectionType LV { get; set; }
        public Material HvMaterial { get; set; }
        public Material LvMaterial { get; set; }
        public double HvTempCoefficient { get; set; }
        public double LvTempCoefficient { get; set; }

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
    public class RessistanceTransformerChannel
    {
        /// <summary>
        /// Број на каналот на трансформаторот
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// Струјата со која се вршени мерењата
        /// </summary>
        public float TestCurrent { get; set; }
        /// <summary>
        /// Тековна состојба на SampleRate за мерење на отпор
        /// </summary>
        public int RessistanceSampleRateCurrentState { get; set; }
        /// <summary>
        /// Тековна состојба на NoOfSamples за мерење на отпор
        /// </summary>
        public int RessistanceNoOfSamplesCurrentState { get; set; }
        /// <summary>
        /// Тековна состојба на каналите за мерење на отпор
        /// </summary>
        public List<RessistanceChannel> RessistanceChannelsCurrentState { get; set; }
        /// <summary>
        /// Состојба на каналите за самите мерење на отпор
        /// </summary>
        public List<RessistanceChannel> RessistanceChannelsMeasurenmentConfiguration { get; set; }
        /// <summary>
        /// Мерења на отпор
        /// </summary>
        public List<RessistanceMeasurenment> RessistanceMeasurenments { get; set; }

        public RessistanceTransformerChannel(int channelNo, int testCurrent,
            int ressistanceSampleRateCurrentState, int ressistanceNoOfSamplesCurrentState,
            List<RessistanceChannel> ressistanceChannelsCurrentState,
            List<RessistanceChannel> ressistanceChannelsMeasurenmentConfiguration,
            List<RessistanceMeasurenment> ressistanceMeasurenments)
        {
            ChannelNo = channelNo;
            TestCurrent = testCurrent;
            RessistanceSampleRateCurrentState = ressistanceSampleRateCurrentState;
            RessistanceNoOfSamplesCurrentState = ressistanceNoOfSamplesCurrentState;
            RessistanceChannelsCurrentState = ressistanceChannelsCurrentState;
            RessistanceChannelsMeasurenmentConfiguration = ressistanceChannelsMeasurenmentConfiguration;
            RessistanceMeasurenments = ressistanceMeasurenments;

        }
        public RessistanceTransformerChannel() : this(1, 1, 1, 1, new List<RessistanceChannel>(), 
            new List<RessistanceChannel>(), new List<RessistanceMeasurenment>()) { }
    }

    /// <summary>
    /// Состојба на еден канал за мерење на отпор
    /// </summary>
    [Serializable]
    public class RessistanceChannel
    {
        /// <summary>
        /// Број на каналот
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// Дали каналот е ON Или OFF
        /// </summary>
        public bool IsOn { get; set; }
        public RessistanceChannel(int channelNo, bool isOn)
        {
            ChannelNo = channelNo;
            
            IsOn = isOn;
        }
        public RessistanceChannel() : this(1, false) { }
    }
    [Serializable]
    public class RessistanceMeasurenment
    {
        /// <summary>
        /// Време на мерењето
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Мерењата за секој канал
        /// </summary>
        public List<RessistanceChannelMeasurenment> RessistanceChannelMeasurenment { get; set; }
        public RessistanceMeasurenment(DateTime time, List<RessistanceChannelMeasurenment> ressistanceChannelMeasurenment)
        {
            Time = time;
            RessistanceChannelMeasurenment = ressistanceChannelMeasurenment;
        }
        public RessistanceMeasurenment(DateTime time) : this(time, new List<RessistanceChannelMeasurenment>()) { }
        public RessistanceMeasurenment() : this(DateTime.Now) { }
    }
    /// <summary>
    /// Мерење на напон и струја
    /// </summary>
    [Serializable]
    public class RessistanceChannelMeasurenment
    {
        /// <summary>
        /// Број на канал
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// Напонот добиен од мерењето
        /// </summary>
        public double Voltage { get; set; }
        /// <summary>
        /// Струјата добиена од мерењето
        /// </summary>
        public double Current { get; set; }
        public RessistanceChannelMeasurenment(int channelNo, double voltage, double current)
        {
            ChannelNo = channelNo;
            Voltage = voltage;
            Current = current;
        }
        public RessistanceChannelMeasurenment() : this(1, 0, 1){ }
    }

    /// <summary>
    /// Сотојба на еден температурен канал
    /// </summary>
    [Serializable]
    public class TempChannel
    {
        /// <summary>
        /// Број на каналот
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// Дали каналот е ON Или OFF
        /// </summary>
        public bool IsOn { get; set; }
        /// <summary>
        /// Дали се работи за Oil канал или Amb канал
        /// </summary>
        public bool IsOil { get; set; }
        public TempChannel(int channelNo, bool isOn, bool isOil)
        {
            ChannelNo = channelNo;
            IsOn = isOn;
            IsOil = isOil;
        }
        public TempChannel() : this(1, false, false) { }
    }
    /// <summary>
    /// Едно температурно мерење со мерените вредности на сите канали.
    /// </summary>
    [Serializable]
    public class TempMeasurenment
    {
        /// <summary>
        /// Време на мерењето
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Мерењата за секој канал
        /// </summary>
        public List<TempChannelMeasurenment> TempChannelMeasurenments { get; set; }

        public TempMeasurenment(DateTime time, List<TempChannelMeasurenment> tempChannelMeasurenments)
        {
            Time = time;
            TempChannelMeasurenments = tempChannelMeasurenments;
        }
        public TempMeasurenment(DateTime time) : this(time, new List<TempChannelMeasurenment>()) { }
        public TempMeasurenment() : this(DateTime.Now) { }
    }
    /// <summary>
    /// Температурно мерење со мерените вредности на еден канал.
    /// </summary>
    [Serializable]
    public class TempChannelMeasurenment
    {
        /// <summary>
        /// Број на канал
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// Вредноста добиена од мерењето
        /// </summary>
        public float Value { get; set; }

        public TempChannelMeasurenment(int channelNo, float value)
        {
            ChannelNo = channelNo;
            Value = value;
        }
        public TempChannelMeasurenment() : this(1, 0) { }
    }
}
