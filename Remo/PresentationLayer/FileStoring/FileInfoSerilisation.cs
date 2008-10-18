using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace PresentationLayer
{
    
        static class XmlFileServices
        {
            /*
            private Type[] types = new Type[] { 
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
                        typeof(TempMeasurenment)
                     };
            */
            static public void writeToXml(string path, FileStoring fileStoring)
            {
                try
                {
                    using (Stream fStream = new FileStream(path,
                                    FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(FileStoring));
                        xmlFormat.Serialize(fStream, fileStoring);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            static public FileStoring readXml(string path)
            {
                FileStoring fileStoring;
                try
                {
                    using (Stream fStream = new FileStream(path,
                                    FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(FileStoring));
                        fileStoring = (FileStoring)xmlFormat.Deserialize(fStream);
                    }
                    return fileStoring;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }
    
}
