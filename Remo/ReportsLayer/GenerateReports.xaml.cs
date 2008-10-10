using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReportsLayer
{
    /// <summary>
    /// Interaction logic for GenerateReports.xaml
    /// </summary>
    public partial class GenerateReports : Window
    {
        private FlowDocumentReport flowDocumentReport;
        private DataAccessLayer.DataSource dataSource;

        public GenerateReports()
        {
            InitializeComponent();
            dataSource = new DataAccessLayer.DataSource(@"E:\root.xml");

            dataSource.Root.TransformerProperties = new EntityLayer.TransformerProperties("12345", "6789", "Gjore", "Nesto", EntityLayer.TransformerProperties.ConnectionType.D, EntityLayer.TransformerProperties.ConnectionType.Y, EntityLayer.TransformerProperties.Material.Aluminium, EntityLayer.TransformerProperties.Material.Aluminium, 20, 20);
            
            flowDocumentReport = new FlowDocumentReport(dataSource);

            //flowExample = flowDocumentReport.createDocument(FlowDocumentReportType.AcHotMeasurenments);
            flowExample = flowDocumentReport.createDocument(FlowDocumentReportType.DcColdMeasurenments);
            flowDocumentScrollViewer.Document = flowDocumentReport.createDocument(FlowDocumentReportType.DcColdMeasurenments);
        }

        private void DCCold_Checked(object sender, RoutedEventArgs e)
        {
            //this.FontFamily = new FontFamily("Courier New");
            FlowDocumentReportType flowDocumentReportType = FlowDocumentReportType.DcColdMeasurenments;

            switch (((RadioButton)sender).Name) 
            {
                case "DCCold": flowDocumentReportType = FlowDocumentReportType.DcColdMeasurenments; break;
                case "ACHot": flowDocumentReportType = FlowDocumentReportType.AcHotMeasurenments; break;
                case "DCCooling": flowDocumentReportType = FlowDocumentReportType.DcCoolingMeasurenments; break;
            }
            if(flowDocumentScrollViewer!=null)
            flowDocumentScrollViewer.Document = flowDocumentReport.createDocument(flowDocumentReportType);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = flowDocumentScrollViewer.Document;
                // Save all the existing settings.
                double pageHeight = doc.PageHeight;
                double pageWidth = doc.PageWidth;
                Thickness pagePadding = doc.PagePadding;
                double columnGap = doc.ColumnGap;
                double columnWidth = doc.ColumnWidth;
                // Make the FlowDocument page match the printed page.
                doc.PageHeight = 1200;
                doc.PageWidth = 750;
                doc.PagePadding = new Thickness(60);

                printDialog.PrintDocument(
                ((IDocumentPaginatorSource)doc).DocumentPaginator,
                "A Flow Document");
            }
        }
    }
}
