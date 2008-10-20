using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAccessLayer
{
    public enum FileCommand 
        {
            New,
            Open,
            Save
        }

    public partial class DataSource : INotifyPropertyChanged
    {
        /// <summary>
        /// Објектот со сите ентитети во него.
        /// </summary>
        public EntityLayer.Root Root { get; set; }

        public bool IsDirty { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
            /// <summary>
            /// За промена на пропертијата.
            /// </summary>
            /// <param name="e"></param>
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
            
            /// <summary>
            /// Во DcColdMeasurenments,во подтабот RessistanceMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DcColdMeasurenments_RessistanceTransformerChannels_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                this.calcDcResValues();
            }
            public void calcDcResValues()
            {
                R1Cold = evalR1Cold(SelectedChannel);
                R2Cold = evalR2Cold(SelectedChannel);

                R1AtStdTemp = evalR1AtStdTemp();
                R2AtStdTemp = evalR2AtStdTemp();
                R1Phase = evalR1Phase();
                R2Phase = evalR2Phase();

                StdDevTempR1 = evalStdDevTempR1(SelectedChannel);
                StdDevTempR2 = evalStdDevTempR2(SelectedChannel);

                R1ColdAtDcCool = evalR1Cold(SelectedChannelDcCool);
                R2ColdAtDcCool = evalR2Cold(SelectedChannelDcCool);
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

                if(ACHeatingTableHeader==null)ACHeatingTableHeader = evalACHeatingTableHeader();

            }
            /// <summary>
            /// Во AcHotMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void AcHotMeasurenments_TempMeasurenementConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                ACHeatingTableHeader = evalACHeatingTableHeader();
                //MinutesSampleRate = evalMinutesSampleRate();
                //SecondesSampleRate = evalSecondesSampleRate();
                //Todo: Najverojatno ova treba da se trgne.
                SamplesDone=evalSamplesDone();
                EndAcTemp = evalEndAcTemp();
                KDropInOil = evalKDropInOil();


            }
            /// <summary>
            /// Во DcCoolingMeasurenments Ако се смени некој прочитан податок, одново да се пресметаат вредностите на полињата што се добиваат преку некако функција.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void DcCoolingMeasurenments_RessistanceTransformerChannels_PropertyChanged(object sender, PropertyChangedEventArgs e) 
            {
                //Todo dali treba?
                TCold = evalTCold();
            }

            public void TransformerProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {

            }

            public DataSource(string path,FileCommand fileCommand) 
            {
                //Citanje na podatocite
                EntityLayer.XmlServices serv = new EntityLayer.XmlServices();


                switch (fileCommand) 
                {
                    case FileCommand.New:
                        Root = new EntityLayer.Root();
                        break;
                    case FileCommand.Open:
                        Root = serv.readXml(path); 
                        break;
                    case FileCommand.Save:
                        Root = serv.readXml(path); 
                        break;
                    default: Root = new EntityLayer.Root(); break;
                }

                //new EntityLayer.XmlServices().writeToXmlTest(path);
                //new EntityLayer.XmlServices().writeToXml(path, new EntityLayer.Root());
    
                //DCCold
                Root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                Root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //ACHeating
                Root.AcHotMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(AcHotMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //DCCooling
                Root.DcCoolingMeasurenments.RessistanceTransformerChannel.PropertyChanged += new PropertyChangedEventHandler(DcCoolingMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                //TransformatorProperties
                Root.TransformerProperties.PropertyChanged += new PropertyChangedEventHandler(TransformerProperties_PropertyChanged);

                Root.DcColdMeasurenments.TempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
            public DataSource(string path,bool newSource)
            {
                //Citanje na podatocite
                EntityLayer.XmlServices serv = new EntityLayer.XmlServices();

                new EntityLayer.XmlServices().writeToXmlNew(path);

                Root = serv.readXml(path);

                Root = new EntityLayer.XmlServices().readXml(path);


                //DCCold
                Root.DcColdMeasurenments.RessistanceTransformerChannels.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                Root.DcColdMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(DcColdMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //ACHeating
                Root.AcHotMeasurenments.TempMeasurenementConfiguration.PropertyChanged += new PropertyChangedEventHandler(AcHotMeasurenments_TempMeasurenementConfiguration_PropertyChanged);
                //DCCooling
                Root.DcCoolingMeasurenments.RessistanceTransformerChannel.PropertyChanged += new PropertyChangedEventHandler(DcCoolingMeasurenments_RessistanceTransformerChannels_PropertyChanged);
                //TransformatorProperties
                Root.TransformerProperties.PropertyChanged += new PropertyChangedEventHandler(TransformerProperties_PropertyChanged);

                Root.DcColdMeasurenments.TempMeasurenementConfiguration.OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
            
            public void saveData(string path)
            {
                new EntityLayer.XmlServices().writeToXml(path, this.Root);
            }
    }
    
}