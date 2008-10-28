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
using DataAccessLayer;
using DNBSoft.WPF.WPFGraph;

namespace ReportsLayer
{
    /// <summary>
    /// Interaction logic for GenerateReports.xaml
    /// </summary>
    public partial class GenerateReports : Window
    {
        private FlowDocumentReport flowDocumentReport;
        private DataSource dataSource;
        private FlowDocumentReportType flowDocumentReportType = FlowDocumentReportType.DcColdMeasurenments;


        public GenerateReports(DataSource dataSource, WPFScatterGraph ACHotGraph, WPFScatterGraph GraphT1, WPFScatterGraph GraphT2)
        {
            InitializeComponent();
            this.dataSource = dataSource;

            //dataSource.Root.TransformerProperties = new EntityLayer.TransformerProperties("12345", "6789", "Gjore", "Nesto", EntityLayer.TransformerProperties.ConnectionType.D, EntityLayer.TransformerProperties.ConnectionType.Y, EntityLayer.TransformerProperties.Material.Aluminium, EntityLayer.TransformerProperties.Material.Aluminium, 20, 20);
            flowDocumentReport = new FlowDocumentReport(dataSource,ACHotGraph,GraphT1,GraphT2);
            flowDocumentScrollViewer.Document = flowDocumentReport.returnDocument(FlowDocumentReportType.DcColdMeasurenments);
        }
        public GenerateReports(DataSource dataSource)
        {
            InitializeComponent();
            this.dataSource = dataSource;

            flowDocumentReport = new FlowDocumentReport(dataSource);
            flowDocumentScrollViewer.Document = flowDocumentReport.returnDocument(FlowDocumentReportType.DcColdMeasurenments);
        }

        public GenerateReports()
        {
            InitializeComponent();
            this.dataSource = new DataSource("E:\\root.xml", FileCommand.New);

            dataSource.Root.TransformerProperties = new EntityLayer.TransformerProperties("12345", "6789", "Gjore", "Nesto", EntityLayer.TransformerProperties.ConnectionType.D, EntityLayer.TransformerProperties.ConnectionType.Y, EntityLayer.TransformerProperties.Material.Aluminium, EntityLayer.TransformerProperties.Material.Aluminium, 20, 20);
            flowDocumentReport = new FlowDocumentReport(dataSource);
            flowDocumentScrollViewer.Document = flowDocumentReport.returnDocument(FlowDocumentReportType.DcColdMeasurenments);
        }

        private void DCCold_Checked(object sender, RoutedEventArgs e)
        {
            //this.FontFamily = new FontFamily("Courier New");
            flowDocumentReportType = FlowDocumentReportType.DcColdMeasurenments;

            switch (((RadioButton)sender).Name) 
            {
                case "DCCold": flowDocumentReportType = FlowDocumentReportType.DcColdMeasurenments; break;
                case "ACHot": flowDocumentReportType = FlowDocumentReportType.AcHotMeasurenments; break;
                case "DCCooling": flowDocumentReportType = FlowDocumentReportType.DcCoolingMeasurenments; break;
            }
            if(flowDocumentScrollViewer!=null)
                flowDocumentScrollViewer.Document = flowDocumentReport.returnDocument(flowDocumentReportType);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument doc=new FlowDocument();
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                doc = flowDocumentReport.returnDocument(flowDocumentReportType);
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
