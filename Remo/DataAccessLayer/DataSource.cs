using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    #region dzabe rabota
    class RessistanceTransformerChannel
    {
        private EntityLayer.RessistanceTransformerChannel ressistanceTransformerChannel;
        /*
        private int ChNo;
        private double TestCrr;
        private int RessistanceSampleRateCurrentSt;
        private int RessistanceNoOfSamplesCurrentSmp;
        private bool IsCh1On;
        private bool IsCh2On;
         */
        //private EntityLayer.ListWithChangeEvents<RessistanceMeasurenment> RessistanceMeasurenments { get; set; }

        /// <summary>
        /// Број на каналот.
        /// </summary>
        public int ChannelNo
        {
            get { return ressistanceTransformerChannel.ChannelNo; }
            set
            {
                if (ressistanceTransformerChannel.ChannelNo != value)
                {
                    ressistanceTransformerChannel.ChannelNo = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("ChannelNo"));
                }
            }
        }

        public double TestCurrent
        {
            get { return ressistanceTransformerChannel.TestCurrent; }
            set
            {
                if (ressistanceTransformerChannel.TestCurrent != value)
                {
                    ressistanceTransformerChannel.TestCurrent = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("TestCurrent"));
                }
            }
        }

        public int RessistanceSampleRateCurrentState
        {
            get { return ressistanceTransformerChannel.RessistanceSampleRateCurrentState; }
            set
            {
                if (ressistanceTransformerChannel.RessistanceSampleRateCurrentState != value)
                {
                    ressistanceTransformerChannel.RessistanceSampleRateCurrentState = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("RessistanceSampleRateCurrentState"));
                }
            }
        }

        public int RessistanceNoOfSamplesCurrentState
        {
            get { return ressistanceTransformerChannel.RessistanceNoOfSamplesCurrentState; }
            set
            {
                if (ressistanceTransformerChannel.RessistanceNoOfSamplesCurrentState != value)
                {
                    ressistanceTransformerChannel.RessistanceNoOfSamplesCurrentState = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("RessistanceNoOfSamplesCurrentState"));
                }
            }
        }

        public bool IsChannel1On
        {
            get { return ressistanceTransformerChannel.IsChannel1On; }
            set
            {
                if (ressistanceTransformerChannel.IsChannel1On != value)
                {
                    ressistanceTransformerChannel.IsChannel1On = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("IsChannelOn"));
                }
            }
        }

        public bool IsChannel2On
        {
            get { return ressistanceTransformerChannel.IsChannel2On; }
            set
            {
                if (ressistanceTransformerChannel.IsChannel2On != value)
                {
                    ressistanceTransformerChannel.IsChannel2On = value;
                    ressistanceTransformerChannel.OnPropertyChanged(new PropertyChangedEventArgs("IsChanne2On"));
                }
            }
        }

        public EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment> RessistanceMeasurenments
        {
            get { return ressistanceTransformerChannel.RessistanceMeasurenments; }
            set
            {
                if (ressistanceTransformerChannel.RessistanceMeasurenments != value)
                {
                    ressistanceTransformerChannel.RessistanceMeasurenments = value;
                }
            }
        }

        public RessistanceTransformerChannel()
        {
            ressistanceTransformerChannel = new EntityLayer.RessistanceTransformerChannel();
        }
    }
    class TempMeasurenementConfiguration
        {
            

            private EntityLayer.TempMeasurenementConfiguration tempMeasurenementConfiguration;
            public DateTime TimeOfReduction
            {
                get { return tempMeasurenementConfiguration.TimeOfReduction; }
                set
                {
                    if (tempMeasurenementConfiguration.TimeOfReduction != value)
                    {
                        tempMeasurenementConfiguration.TimeOfReduction = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("TimeOfReduction"));
                    }
                }
            }
            public int TempSampleRateCurrentState
            {
                get { return tempMeasurenementConfiguration.TempSampleRateCurrentState; }
                set
                {
                    if (tempMeasurenementConfiguration.TempSampleRateCurrentState != value)
                    {
                        tempMeasurenementConfiguration.TempSampleRateCurrentState = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("TempSampleRateCurrentState"));
                    }
                }
            }
            public int TempNoOfSamplesCurrentState
            {
                get { return tempMeasurenementConfiguration.TempNoOfSamplesCurrentState; }
                set
                {
                    if (tempMeasurenementConfiguration.TempNoOfSamplesCurrentState != value)
                    {
                        tempMeasurenementConfiguration.TempNoOfSamplesCurrentState = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("TempNoOfSamplesCurrentState"));
                    }
                }
            }
            public bool IsChannel1On
            {
                get { return tempMeasurenementConfiguration.IsChannel1On; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel1On != value)
                    {
                        tempMeasurenementConfiguration.IsChannel1On = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1On"));
                    }
                }
            }
            public bool IsChannel2On
            {
                get { return tempMeasurenementConfiguration.IsChannel2On; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel2On != value)
                    {
                        tempMeasurenementConfiguration.IsChannel2On = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel2On"));
                    }
                }
            }
            public bool IsChannel3On
            {
                get { return tempMeasurenementConfiguration.IsChannel3On; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel3On != value)
                    {
                        tempMeasurenementConfiguration.IsChannel3On = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel3On"));
                    }
                }
            }
            public bool IsChannel4On
            {
                get { return tempMeasurenementConfiguration.IsChannel4On; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel4On != value)
                    {
                        tempMeasurenementConfiguration.IsChannel4On = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel4On"));
                    }
                }
            }

            public bool IsChannel1Oil
            {
                get { return tempMeasurenementConfiguration.IsChannel1Oil; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel1Oil != value)
                    {
                        tempMeasurenementConfiguration.IsChannel1Oil = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1Oil"));
                    }
                }
            }
            public bool IsChannel2Oil
            {
                get { return tempMeasurenementConfiguration.IsChannel2Oil; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel2Oil != value)
                    {
                        tempMeasurenementConfiguration.IsChannel2Oil = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel1Oil"));
                    }
                }
            }
            public bool IsChanne31Oil
            {
                get { return tempMeasurenementConfiguration.IsChannel1Oil; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel3Oil != value)
                    {
                        tempMeasurenementConfiguration.IsChannel3Oil = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel3Oil"));
                    }
                }
            }
            public bool IsChannel4Oil
            {
                get { return tempMeasurenementConfiguration.IsChannel4Oil; }
                set
                {
                    if (tempMeasurenementConfiguration.IsChannel4Oil != value)
                    {
                        tempMeasurenementConfiguration.IsChannel4Oil = value;
                        tempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs("IsChannel4Oil"));
                    }
                }
            }
            public EntityLayer.ListWithChangeEvents<EntityLayer.TempMeasurenment> TempMeasurenments
            {
                get { return tempMeasurenementConfiguration.TempMeasurenments; }
                set
                {
                    if (tempMeasurenementConfiguration.TempMeasurenments != value)
                    {
                        tempMeasurenementConfiguration.TempMeasurenments = value;
                    }
                }
            }
            public TempMeasurenementConfiguration()
            {
                //   tempMeasurenementConfiguration=new EntityLayer.TempMeasurenementConfiguration(
            }
        }

    class TransformerProperties
        {
            private EntityLayer.TransformerProperties transformerProperties;

            public String TransformatorSeries
            {
                get { return transformerProperties.TransformatorSeries; }
                set
                {
                    if (transformerProperties.TransformatorSeries != value)
                    {
                        transformerProperties.TransformatorSeries = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("TransformatorSeries"));
                    }
                }
            }
            public String TransformatorSerialNo
            {
                get { return transformerProperties.TransformatorSerialNo; }
                set
                {
                    if (transformerProperties.TransformatorSerialNo != value)
                    {
                        transformerProperties.TransformatorSerialNo = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("TransformatorSerialNo"));
                    }
                }
            }
            public String PresentAtTest
            {
                get { return transformerProperties.PresentAtTest; }
                set
                {
                    if (transformerProperties.PresentAtTest != value)
                    {
                        transformerProperties.PresentAtTest = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("PresentAtTest"));
                    }
                }
            }
            public String Comment
            {
                get { return transformerProperties.Comment; }
                set
                {
                    if (transformerProperties.Comment != value)
                    {
                        transformerProperties.Comment = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
                    }
                }
            }
            public EntityLayer.TransformerProperties.ConnectionType HV
            {
                get { return transformerProperties.HV; }
                set
                {
                    if (transformerProperties.HV != value)
                    {
                        transformerProperties.HV = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("HV"));

                    }
                }
            }
            public EntityLayer.TransformerProperties.ConnectionType LV
            {
                get { return transformerProperties.LV; }
                set
                {
                    if (transformerProperties.LV != value)
                    {
                        transformerProperties.LV = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("LV"));
                    }
                }
            }
            public EntityLayer.TransformerProperties.Material HvMaterial
            {
                get { return transformerProperties.HvMaterial; }
                set
                {
                    if (transformerProperties.HvMaterial != value)
                    {
                        transformerProperties.HvMaterial = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("HvMaterial"));
                    }
                }
            }
            public EntityLayer.TransformerProperties.Material LvMaterial
            {
                get { return transformerProperties.LvMaterial; }
                set
                {
                    if (transformerProperties.LvMaterial != value)
                    {
                        transformerProperties.LvMaterial = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("LvMaterial"));
                    }
                }
            }
            public double HvTempCoefficient
            {
                get { return transformerProperties.HvTempCoefficient; }
                set
                {
                    if (transformerProperties.HvTempCoefficient != value)
                    {
                        transformerProperties.HvTempCoefficient = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("HvTempCoefficient"));
                    }
                }
            }
            public double LvTempCoefficient
            {
                get { return transformerProperties.LvTempCoefficient; }
                set
                {
                    if (transformerProperties.LvTempCoefficient != value)
                    {
                        transformerProperties.LvTempCoefficient = value;
                        transformerProperties.OnPropertyChanged(new PropertyChangedEventArgs("LvTempCoefficient"));
                    }
                }
            }
        }
#endregion
    public partial class DataSource : INotifyPropertyChanged
    {
        /// <summary>
        /// Објектот со сите ентитети во него.
        /// </summary>
        public EntityLayer.Root root;

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
            
            /// <summary>
            /// Во DcColdMeasurenments,во подтабот RessistanceMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DcColdMeasurenments_RessistanceTransformerChannels_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                R1AtStdTemp = evalR1AtStdTemp();
                R2AtStdTemp = evalR2AtStdTemp();
                R1Phase = evalR1Phase();
                R2Phase = evalR1Phase();
                R1Cold = evalR1Cold();
                R2Cold = evalR2Cold();

                StdDevTempR1 = evalStdDevTempR1(selectedChannelIndex);
                StdDevTempR2 = evalStdDevTempR2(selectedChannelIndex);

                
            }
            /// <summary>
            /// Во DcColdMeasurenments,во подтабот TemperatureMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DcColdMeasurenments_TempMeasurenementConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                T1MeanDCColdTempTableHeader = evalT1MeanDCColdTempTableHeader();
                T1MeanDCColdTempTable = evalT1MeanDCColdTempTable();

                T2MeanDCColdTempTableHeader = evalT2MeanDCColdTempTableHeader();
                T2MeanDCColdTempTable = evalT2MeanDCColdTempTable();

                T3MeanDCColdTempTableHeader = evalT3MeanDCColdTempTableHeader();
                T3MeanDCColdTempTable = evalT3MeanDCColdTempTable();

                T4MeanDCColdTempTableHeader = evalT4MeanDCColdTempTableHeader();
                T4MeanDCColdTempTable = evalT4MeanDCColdTempTable();

                OilDCColdTempTable = evalOilDCColdTempTable();
                AmbDCColdTempTable = evalAmbDCColdTempTable();

                TCold = evalTCold();

                DCColdTemperatureTableHeader = evalDCColdTemperatureTableHeader();

            }
            /// <summary>
            /// Во AcHotMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void AcHotMeasurenments_TempMeasurenementConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ACHeatingTableHeader=evalACHeatingTableHeader();

                MinutesSampleRate=evalMinutesSampleRate();
                SecondesSampleRate=evalSecondesSampleRate();
                //Todo: Najverojatno ova treba da se trgne.
                SamplesDone=evalSamplesDone();


            }
            /// <summary>
            /// Во DcCoolingMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DcCoolingMeasurenments_RessistanceTransformerChannels_PropertyChanged(object sender, PropertyChangedEventArgs e) 
            {
                TCold = evalTCold();

                T1_T0=evalT1T0();
                T1Rise=evalT1Rise();
                FT1=evalFT1();
                T2_T0 = evalT2T0();
                T2Rise = evalT2Rise();
                FT2 = evalFT2(); 
            
            }
            /// <summary>
            /// Во TransformerProperties Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void TransformerProperties_PropertyChanged(object sender, PropertyChangedEventArgs e) 
            {
                //Settings
                EndAcTemp = evalEndAcTemp();
                KDropInOil = evalKDropInOil();

                //Results
                T1_T0 = evalT1T0();
                T1_Rise = evalT1Rise();
                F_T1 = evalFT1();

                T2_T0 = evalT2T0();
                T2_Rise = evalT2Rise();
                F_T2 = evalFT2();
            }
            public DataSource(string path) 
            {
                //Citanje na podatocite
                EntityLayer.XmlServices serv = new EntityLayer.XmlServices();


                serv.writeToXmlTest(path);

                new EntityLayer.XmlServices().writeToXmlTest(path);




                root = serv.readXml(path);

                root = new EntityLayer.XmlServices().readXml(path);


                //DCCold
                root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //ACHeating
                root.AcHotMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(AcHotMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //DCCooling
                root.DcCoolingMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DcCoolingMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                //TransformatorProperties
                root.TransformerProperties.PropertyChanged += new PropertyChangedEventHandler(TransformerProperties_PropertyChanged);

                root.DcColdMeasurenments.TempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }

    }
}