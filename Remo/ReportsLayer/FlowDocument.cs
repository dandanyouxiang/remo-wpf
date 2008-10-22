using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using DataAccessLayer;
using EntityLayer;
using DNBSoft.WPF.WPFGraph;  

namespace ReportsLayer
{
    /// <summary>
    /// Каков тип на репорт треба да се генерира.
    /// </summary>
    public enum FlowDocumentReportType
    {
        DcColdMeasurenments,
        AcHotMeasurenments,
        DcCoolingMeasurenments
    }
    /// <summary>
    /// Класа за генерирање на репорти во форма на флоу (тековна содржина) документ.
    /// </summary>
    class FlowDocumentReport
    {
        private FlowDocument flowDocument;
        private DataSource dataSource;


        private FlowDocument DcColdMeasurenmentsDocument;
        private FlowDocument AcHotMeasurenmentsDocument;
        private FlowDocument DcCoolingMeasurenmentsDocument;

        #region Graph Properties

        private WPFScatterGraph ACHotGraph;
        private WPFScatterGraph GraphT1;
        private WPFScatterGraph GraphT2;

        #endregion Graph Properties


        public FlowDocumentReport() 
        {
            flowDocument = new FlowDocument(); 
        }

        public DataSource dataSourceValue 
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public FlowDocumentReport(DataSource dataSource)
        {
            this.dataSource = dataSource;
            
            DcColdMeasurenmentsDocument = new FlowDocument();
            AcHotMeasurenmentsDocument = new FlowDocument();
            DcCoolingMeasurenmentsDocument = new FlowDocument();

            makeDcColdMeasurenmentsDocument(DcColdMeasurenmentsDocument);
            makeAcHotMeasurenmentsDocument(AcHotMeasurenmentsDocument);
            makeDcCoolingMeasurenmentsDocument(DcCoolingMeasurenmentsDocument);

            DcColdMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");
            AcHotMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");
            DcCoolingMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");

            DcColdMeasurenmentsDocument.DataContext = dataSource;
            AcHotMeasurenmentsDocument.DataContext = dataSource;
            DcCoolingMeasurenmentsDocument.DataContext = dataSource;
        }
        public FlowDocumentReport(DataSource dataSource, WPFScatterGraph aCHotGraph, WPFScatterGraph graphT1, WPFScatterGraph graphT2)
        {
            this.dataSource = dataSource;
            //todo da se trgne
            //dataSource.Root.TransformerProperties = new EntityLayer.TransformerProperties("12345", "6789", "Gjore", "Nesto", EntityLayer.TransformerProperties.ConnectionType.D, EntityLayer.TransformerProperties.ConnectionType.Y, EntityLayer.TransformerProperties.Material.Aluminium, EntityLayer.TransformerProperties.Material.Aluminium, 20, 20);

            copyGraph(ref ACHotGraph, aCHotGraph);
            copyGraph(ref GraphT1, graphT1);
            copyGraph(ref GraphT2, graphT2);
            //Поставување на графиците
            /*
            this.ACHotGraph = ACHotGraph;
            this.GraphT1 = GraphT1;
            this.GraphT2 = GraphT2;
            */
            DcColdMeasurenmentsDocument = new FlowDocument();
            AcHotMeasurenmentsDocument = new FlowDocument();
            DcCoolingMeasurenmentsDocument = new FlowDocument();

            makeDcColdMeasurenmentsDocument(DcColdMeasurenmentsDocument);
            makeAcHotMeasurenmentsDocument(AcHotMeasurenmentsDocument);
            makeDcCoolingMeasurenmentsDocument(DcCoolingMeasurenmentsDocument);

            DcColdMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");
            AcHotMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");
            DcCoolingMeasurenmentsDocument.FontFamily = new FontFamily("Courier New");

            DcColdMeasurenmentsDocument.DataContext = dataSource;
            AcHotMeasurenmentsDocument.DataContext = dataSource;
            DcCoolingMeasurenmentsDocument.DataContext = dataSource;
        }
        public FlowDocument returnDocument(FlowDocumentReportType flowDocumentReportType)
        {
            switch (flowDocumentReportType)
            {
                case FlowDocumentReportType.DcColdMeasurenments: return DcColdMeasurenmentsDocument;
                case FlowDocumentReportType.AcHotMeasurenments: return AcHotMeasurenmentsDocument;
                case FlowDocumentReportType.DcCoolingMeasurenments: return DcCoolingMeasurenmentsDocument;
                default: return flowDocument;
            }
        }


        /// <summary>
        /// Информации кои се прикажуваат на почетокот за секој од репортите.
        /// </summary>
        private void insertHeader(FlowDocument flowDocument)
        {
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns[0].Width = new GridLength(250);
            tempTable.FontWeight = FontWeights.Bold;

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            //Време на мерењето
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(("Transformator Series:")))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.TransformatorSeries.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Transformator Serial No:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.TransformatorSerialNo.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Present at Test:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.PresentAtTest.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Date:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(DateTime.Now.Date.ToShortDateString()))));



            flowDocument.Blocks.Add(tempTable);
            flowDocument.Blocks.Last().Margin = new Thickness(150, 0, 0, 50);

        }
        /// <summary>
        /// Креирање на извештајот во зависност од типот кој го избрал корисникот. 
        /// </summary>
        /// <param name="flowDocumentReportType"></param>
        /// <returns></returns>
        public FlowDocument createDocument(FlowDocumentReportType flowDocumentReportType) 
        {
            switch (flowDocumentReportType) 
            {
                case FlowDocumentReportType.DcColdMeasurenments: return makeDcColdMeasurenmentsDocument();
                case FlowDocumentReportType.AcHotMeasurenments:return makeAcHotMeasurenmentsDocument();
                case FlowDocumentReportType.DcCoolingMeasurenments: return makeDcCoolingMeasurenmentsDocument();
                default: return flowDocument;
            }
        }
        

        private void insertSignature(FlowDocument flowDocument)
        {
            Paragraph signatureParagraph = new Paragraph();
            signatureParagraph.Inlines.Add(new Run("Signature:\n"));
            signatureParagraph.Inlines.Add(new Run("\n"));
            signatureParagraph.Inlines.Add(new Run("_______________________\n"));
            flowDocument.Blocks.Add(signatureParagraph);
            flowDocument.Blocks.Last<Block>().Margin = new Thickness(400, 100, 0, 10);

        }


        private void insertHeader()
        {
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns[0].Width = new GridLength(250);
            tempTable.FontWeight = FontWeights.Bold;

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            //Време на мерењето
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(("Transformator Series:")))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.TransformatorSeries.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Transformator Serial No:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.TransformatorSerialNo.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Present at Test:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(dataSource.Root.TransformerProperties.PresentAtTest.ToString()))));

            tempTable.RowGroups[0].Rows.Add(new TableRow());
            tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

            tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Date:"))));
            //Реден број
            tempRow.Cells.Add(new TableCell(new Paragraph(new Run(DateTime.Now.Date.ToShortDateString()))));



            flowDocument.Blocks.Add(tempTable);
            flowDocument.Blocks.Last().Margin = new Thickness(150, 0, 0, 50);

        }
        private void insertTitle(FlowDocumentReportType flowDocumentReportType,FlowDocument flowDocument)
        {
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.Inlines.Add(new Run("REMO 60/50 Low Resistance Measuring System\n"));
            titleParagraph.Inlines.Add(new Run(flowDocumentReportType.ToString() + "\n"));

            flowDocument.Blocks.Add(titleParagraph);
            Block titleBlock = flowDocument.Blocks.Last<Block>();
            formatTitle(ref titleBlock);
        }
        private void formatTitle(ref Block titleBlock) 
        {
            titleBlock.TextAlignment = TextAlignment.Center;
            //titleBlock.FontWeight = FontWeights.Bold;
            titleBlock.FontSize = 18;
        }

        #region Prethodni
        /// <summary>
        /// Место каде треба да се потпише присутната личност на мерењето.
        /// </summary>
        private void insertSignature()
        {
            /*
             Paragraph signatureParagraph = new Paragraph();
             Floater signatureFloater = new Floater();
             signatureFloater.HorizontalAlignment = HorizontalAlignment.Right;
             Paragraph insideParagraph = new Paragraph();
             insideParagraph.Inlines.Add(new Run("Signature:\n"));
             insideParagraph.Inlines.Add(new Run("\n"));
             insideParagraph.Inlines.Add(new Run("_______________________\n"));
             signatureFloater.Blocks.Add(insideParagraph);
             signatureParagraph.Inlines.Add(signatureFloater);
             flowDocument.Blocks.Add(signatureParagraph);
             flowDocument.Blocks.Last<Block>().Margin = new Thickness(0, 50, 0, 10);
             */

            Paragraph signatureParagraph = new Paragraph();
            signatureParagraph.Inlines.Add(new Run("Signature:\n"));
            signatureParagraph.Inlines.Add(new Run("\n"));
            signatureParagraph.Inlines.Add(new Run("_______________________\n"));
            flowDocument.Blocks.Add(signatureParagraph);
            flowDocument.Blocks.Last<Block>().Margin = new Thickness(400, 100, 0, 10);

        }
        private void insertTitle(FlowDocumentReportType flowDocumentReportType)
        {
            Paragraph titleParagraph = new Paragraph();
            titleParagraph.Inlines.Add(new Run("REMO 60/50 Low Resistance Measuring System\n"));
            titleParagraph.Inlines.Add(new Run(flowDocumentReportType.ToString() + "\n"));

            flowDocument.Blocks.Add(titleParagraph);
            Block titleBlock = flowDocument.Blocks.Last<Block>();
            formatTitle(ref titleBlock);
        }
        #endregion Prethodni

        #region AcHotMeasurenmentsRegion

        
        private void insertAcHotMeasurenmentsHeader(FlowDocument flowDocument)
        {
            Paragraph AcHotMeasurenmentsHeaderParagraph = new Paragraph();

            //Total Samples
            AcHotMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Total Samples: " + dataSource.SamplesDone + "\n"));
            //Sample Rate:
            AcHotMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Sample Rate: " + dataSource.MinutesSampleRate + " min " + dataSource.SecondesSampleRate + " sec\n"));

            flowDocument.Blocks.Add(AcHotMeasurenmentsHeaderParagraph);
        }
        
        private void insertAcHotMeasurenments_TempMeasurenments(FlowDocument flowDocument)
        {
            List<TempMeasurenment> tempMeasurenments = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments;
            TempMeasurenementConfiguration tc = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());


            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());


            tempTable.BorderBrush = System.Windows.Media.Brushes.Black;
            tempTable.BorderThickness = new Thickness(2);

            //Поставување на хедерот
            insertTempMeasurenementConfiguration_TempMeasurenments_Header(tc, tempTable);

            //Приказ на сите мерења
            insertTempMeasurenementConfiguration_TempMeasurenments_TableCell(tc, tempMeasurenments, tempTable);
            tempTable.Columns[0].Width =new GridLength(30);

            flowDocument.Blocks.Add(tempTable);
        }

        /// <summary>
        /// Поставување на хедерот на табелата за температурните мерења за AcHotMeasurenments.
        /// </summary>
        /// <param name="tc">TempMeasurenementConfiguration.</param>
        /// <param name="tempTable">Табелата во документот.</param>
        private void insertTempMeasurenementConfiguration_TempMeasurenments_Header(TempMeasurenementConfiguration tc,Table tempTable) 
        {
            TableRow headerRow = tempTable.RowGroups[0].Rows[0];

            headerRow.Background= System.Windows.Media.Brushes.LightGray;

            //Поставување на стилот на фондот на хедерот на тбаелата
            headerRow.FontStyle = FontStyles.Oblique;
            
            //Реден број
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
            headerRow.Cells.Last().TextAlignment = TextAlignment.Center;
            //Кога е извршено мерењето
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Time"))));
            headerRow.Cells.Last().TextAlignment = TextAlignment.Center;
            //Т1
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T1" + ((tc.IsChannel1Oil) ? "(Oil)" : "(Amb)")))));
            //Т2
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T2" + ((tc.IsChannel2Oil) ? "(Oil)" : "(Amb)")))));
            //Т3
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T3" + ((tc.IsChannel3Oil) ? "(Oil)" : "(Amb)")))));
            //Т4
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T4" + ((tc.IsChannel4Oil) ? "(Oil)" : "(Amb)")))));
            //ТAmb
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T Amb"))));
            //TOil
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T Oil"))));
            //Разлика во температурите во воздух и во масло.
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("T. Rise"))));

        }

        /// <summary>
        /// Формирање на табелата за температурните мерења за AcHotMeasurenments.
        /// </summary>
        /// <param name="tc">TempMeasurenementConfiguration.</param>
        /// <param name="tempMeasurenments">Листа со ТempMeasurenment.</param>
        /// <param name="tempTable">Табелата во документот.</param>
        private void insertTempMeasurenementConfiguration_TempMeasurenments_TableCell(TempMeasurenementConfiguration tc, List<TempMeasurenment> tempMeasurenments, Table tempTable) 
        {
            foreach (TempMeasurenment tm in tempMeasurenments)
            {
                //Додавање на нова редица во табелата.
                tempTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

                //Реден број
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(tempMeasurenments.IndexOf(tm).ToString()))));
                //Време на мерењето
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(tm.Time.ToString("hh:mm:ss")))));

                if (!tm.IsSampleReduced)
                {
                    //Т1
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T1)))));
                    //Т2
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T2)))));
                    //Т3
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T3)))));
                    //Т4
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T4)))));
                    //TAmb
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(((((!tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((!tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0))))))));
                    //TOil
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater((((tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0)))))));
                    //Разлика во температурите во воздух и во масло.
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater((((!tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((!tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0)) - (((tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0)))))));
                }
                //Ако е редуцирано.
                else 
                {
                    for (int i = 0; i < 7; i++) 
                    {
                        tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Reduced"))));
                    }
                }
            }
        }
        
        private void insertMeanTempFields(FlowDocument flowDocument)
        {
            Paragraph meanTempFieldsParagraph = new Paragraph();

            double temp = 1.1234;

            if(dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Count>0)
            temp = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;

            meanTempFieldsParagraph.Inlines.Add(new Run("End Amb Temp:       " + ReportStringFormater.temperatureFormater( temp) + " \n"));
            meanTempFieldsParagraph.Inlines.Add(new Run("Avg Oil Temp (AOT): " +ReportStringFormater.temperatureFormater( dataSource.EndAcTemp) + " \n"));
            meanTempFieldsParagraph.Inlines.Add(new Run("K drop in Oil:      " +ReportStringFormater.temperatureFormater( dataSource.KDropInOil) + "\n"));
            flowDocument.Blocks.Add(meanTempFieldsParagraph);
        }

        private void insertBlanckBlock() 
        {
            flowDocument.Blocks.Add(new Paragraph(new Run("")));
        }

        private void insertGraph(FlowDocument flowDocument)
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();

            /*
            // Asign the button as the UI container's child.
            WPFScatterGraph ACHotGraph = new WPFScatterGraph() {XAxisTitle = "R Err",
            MinYRange = -0.001,
            MaxYRange = 0.001,FontSize=5,
            YAxisTitle = "R Meas",AxisFontSize=12 };
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;


            acGraphInit(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            */
            stackPanel.Width = 700;
            stackPanel.Children.Add(ACHotGraph);

            myInlineUIContainer.Child = stackPanel;
            graphParagraph.Inlines.Add(myInlineUIContainer);

            flowDocument.Blocks.Add(graphParagraph);


        }

        private void acGraphInit(ref WPFScatterGraph AcGraph,ref WPFGraphSeries seriesOilTemp,ref WPFGraphSeries seriesAmbTemp,ref WPFGraphSeries seriesTempRise)
        {
            
            //AcGraph.MinHeight = 200;
            //AcGraph.MinWidth = 200;
            AcGraph.Margin = new Thickness(50);
            AcGraph.XAxisType = WPFScatterGraph.XAxisTypeEnum._time;
            AcGraph.XAxisTitle = "R Err";
            AcGraph.YAxisTitle = "R Meas";
            AcGraph.MinYRange = -0.001;
            AcGraph.MaxYRange = 0.001;
            AcGraph.IntervalYRange = 0.0002;

            seriesOilTemp.Name = "TOil";
            seriesAmbTemp.Name = "TAmb";
            seriesTempRise.Name = "TRise";

            AcGraph.Refresh();
            AcGraph.InitializeComponent();
            AcGraph.Series.Add(seriesOilTemp);
            AcGraph.Series.Add(seriesAmbTemp);
            AcGraph.Series.Add(seriesTempRise);

            WPFGraphPointRenderers.RoundPoint series1PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series1PointRenderer.PointBrush = Brushes.Red;
            series1PointRenderer.PointSize = 5;
            seriesOilTemp.PointRenderer = series1PointRenderer;
            WPFGraphLineRenderers.DashedLine series1LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series1LineRenderer.LineBrush = Brushes.Pink;
            seriesOilTemp.LineRenderer = series1LineRenderer;

            WPFGraphPointRenderers.RoundPoint series2PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series2PointRenderer.PointBrush = Brushes.Blue;
            series2PointRenderer.PointSize = 5;
            seriesAmbTemp.PointRenderer = series2PointRenderer;
            WPFGraphLineRenderers.DashedLine series2LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series2LineRenderer.LineBrush = Brushes.LightBlue;
            seriesAmbTemp.LineRenderer = series2LineRenderer;

            WPFGraphPointRenderers.RoundPoint series3PointRenderer = new WPFGraphPointRenderers.RoundPoint();
            series3PointRenderer.PointBrush = Brushes.Black;
            series3PointRenderer.PointSize = 5;
            seriesTempRise.PointRenderer = series3PointRenderer;
            WPFGraphLineRenderers.DashedLine series3LineRenderer = new WPFGraphLineRenderers.DashedLine();
            series3LineRenderer.LineBrush = Brushes.LightGray;
            seriesTempRise.LineRenderer = series3LineRenderer;
        }

        private void copyGraph(ref WPFScatterGraph newGraph, WPFScatterGraph oldGraph) 
        {
            newGraph = new WPFScatterGraph();

            newGraph.Margin = oldGraph.Margin;
            newGraph.XAxisType = oldGraph.XAxisType;
            newGraph.XAxisTitle = oldGraph.XAxisTitle;
            newGraph.YAxisTitle = oldGraph.YAxisTitle;
            newGraph.MinYRange = oldGraph.MinXRange;
            newGraph.MaxYRange = oldGraph.MinYRange;
            newGraph.IntervalYRange = oldGraph.IntervalYRange;

            
            newGraph.InitializeComponent();
            if (oldGraph.Series.Count > 2)
            {
                newGraph.Series.Add(oldGraph.Series[0]);
                newGraph.Series.Add(oldGraph.Series[1]);
                newGraph.Series.Add(oldGraph.Series[2]);
            }
            newGraph.Refresh();
        }

        private void acGraphRefresh(ref WPFScatterGraph AcGraph, ref WPFGraphSeries seriesOilTemp, ref WPFGraphSeries seriesAmbTemp, ref WPFGraphSeries seriesTempRise)
        {
            double maxY = -1000;
            double minY = 1000;
            double maxX = -1000;
            double minX = (long)DateTime.Now.Ticks;

            seriesOilTemp.Points.Clear();
            foreach (EntityLayer.TempMeasurenment t in dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments)
            {
                EntityLayer.TempMeasurenementConfiguration tempConfig = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
                int numberOfOilTemps = (tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? 1 : 0) + (tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? 1 : 0) + (tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? 1 : 0) + (tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? 1 : 0);
                double oilTemps = (tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? t.T1 : 0) + (tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? t.T2 : 0) + (tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? t.T3 : 0) + (tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? t.T4 : 0);
                double meanOilTemp = oilTemps / numberOfOilTemps;
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = (double)t.Time.Ticks;
                point.Y = meanOilTemp;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                seriesOilTemp.Points.Add(point);
            }

            seriesAmbTemp.Points.Clear();
            foreach (EntityLayer.TempMeasurenment t in dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments)
            {
                EntityLayer.TempMeasurenementConfiguration tempConfig = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
                int numberOfOilTemps = (!tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? 1 : 0) + (!tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? 1 : 0) + (!tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? 1 : 0) + (!tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? 1 : 0);
                double oilTemps = (!tempConfig.IsChannel1Oil && tempConfig.IsChannel1On ? t.T1 : 0) + (!tempConfig.IsChannel2Oil && tempConfig.IsChannel2On ? t.T2 : 0) + (!tempConfig.IsChannel3Oil && tempConfig.IsChannel3On ? t.T3 : 0) + (!tempConfig.IsChannel4Oil && tempConfig.IsChannel4On ? t.T4 : 0);
                double meanOilTemp = oilTemps / numberOfOilTemps;
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = (double)t.Time.Ticks;
                point.Y = meanOilTemp;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                seriesAmbTemp.Points.Add(point);
            }

            seriesTempRise.Points.Clear();
            for (int i = 0; i < seriesAmbTemp.Points.Count; i++)
            {
                WPFGraphDataPoint point = new DNBSoft.WPF.WPFGraph.WPFGraphDataPoint();
                point.X = seriesAmbTemp.Points[i].X;
                point.Y = seriesOilTemp.Points[i].Y - seriesAmbTemp.Points[i].Y;
                if (point.Y > maxY)
                    maxY = point.Y;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.X > maxX)
                    maxX = point.X + 0.1;
                if (point.X < minX)
                    minX = point.X - 0.1;
                seriesTempRise.Points.Add(point);
            }

            AcGraph.MaxYRange = maxY;
            AcGraph.MinYRange = minY;
            AcGraph.MaxXRange = maxX;
            AcGraph.MinXRange = minX;
            AcGraph.IntervalYRange = (maxY - minY) / 20;
            AcGraph.IntervalXRange = (AcGraph.MaxXRange - AcGraph.MinXRange) / 10;

            AcGraph.Refresh();
        }

        private FlowDocument makeAcHotMeasurenmentsDocument(FlowDocument flowDocument)
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments,flowDocument);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader(flowDocument);
            //Поставување на стандардните почетни информации за секој AcHotMeasurenments извештај.
            insertAcHotMeasurenmentsHeader(flowDocument);
            //Табелата со температурните мерења
            insertAcHotMeasurenments_TempMeasurenments(flowDocument);
            //Пресметаните вредности
            insertMeanTempFields(flowDocument);

            insertGraph(flowDocument);
            //insertBlanckBlock(flowDocument);
            flowDocument.Blocks.Add(new Paragraph(new Run("")));

            //Потпис
            insertSignature(flowDocument);

            return flowDocument;
        }

        #region Prethodni

        /// <summary>
        /// Информации кои се прикажуваат на почетокот за секој од репортите за AcHotMeasurenments.
        /// </summary>
        private void insertAcHotMeasurenmentsHeader()
        {
            Paragraph AcHotMeasurenmentsHeaderParagraph = new Paragraph();

            //Total Samples
            AcHotMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Total Samples: " + dataSource.SamplesDone + "\n"));
            //Sample Rate:
            AcHotMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Sample Rate: " + dataSource.MinutesSampleRate + " min " + dataSource.SecondesSampleRate + " sec\n"));

            flowDocument.Blocks.Add(AcHotMeasurenmentsHeaderParagraph);
        }
        /// <summary>
        /// Внесување на табелата со температурните мерења.
        /// </summary>
        private void insertAcHotMeasurenments_TempMeasurenments()
        {
            List<TempMeasurenment> tempMeasurenments = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments;
            TempMeasurenementConfiguration tc = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());

            tempTable.BorderBrush = System.Windows.Media.Brushes.Black;
            tempTable.BorderThickness = new Thickness(2);

            //Поставување на хедерот
            insertTempMeasurenementConfiguration_TempMeasurenments_Header(tc, tempTable);

            //Приказ на сите мерења
            insertTempMeasurenementConfiguration_TempMeasurenments_TableCell(tc, tempMeasurenments, tempTable);

            flowDocument.Blocks.Add(tempTable);
        }
        /// <summary>
        /// Пресметани вредности од температурните мерења.
        /// </summary>
        private void insertMeanTempFields()
        {
            Paragraph meanTempFieldsParagraph = new Paragraph();

            double temp = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T1 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T2 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T3 + dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments.Last().T4;

            meanTempFieldsParagraph.Inlines.Add(new Run("End Amb Temp: " + String.Format("0:0.0", temp) + "\n"));
            meanTempFieldsParagraph.Inlines.Add(new Run("Avg Oil Temp (AOT): " + dataSource.EndAcTemp.ToString() + "\n"));
            meanTempFieldsParagraph.Inlines.Add(new Run("K drop in Oil: " + dataSource.KDropInOil + "\n"));
            flowDocument.Blocks.Add(meanTempFieldsParagraph);
        }
        private void insertGraph()
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();
            /*
            // Asign the button as the UI container's child.
            WPFScatterGraph ACHotGraph=new WPFScatterGraph();
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;
            

            acGraphInit(ref ACHotGraph,ref seriesOilTemp,ref seriesAmbTemp,ref seriesTempRise);
            acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            */
            stackPanel.Width = 900;
            //stackPanel.Height = 500;
            stackPanel.Children.Add(ACHotGraph);



            Paragraph inlineParagraph = new Paragraph();
            Floater graphFloater = new Floater();
            //graphFloater.Width = 500;
            graphFloater.HorizontalAlignment = HorizontalAlignment.Stretch;

            myInlineUIContainer.Child = stackPanel;

            inlineParagraph.Inlines.Add(myInlineUIContainer);
            graphFloater.Blocks.Add(inlineParagraph);

            graphParagraph.Inlines.Add(graphFloater);

            flowDocument.Blocks.Add(graphParagraph);


        }

        private FlowDocument makeAcHotMeasurenmentsDocument()
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader();
            //Поставување на стандардните почетни информации за секој AcHotMeasurenments извештај.
            insertAcHotMeasurenmentsHeader();
            //Табелата со температурните мерења
            insertAcHotMeasurenments_TempMeasurenments();
            //Пресметаните вредности
            insertMeanTempFields();

            insertGraph();
            insertBlanckBlock();
            flowDocument.Blocks.Add(new Paragraph(new Run("")));

            //Потпис
            insertSignature();

            return flowDocument;
        }

        #endregion Prethodni

        #endregion

        #region DcColdMeasurenmentsRegion


        private FlowDocument makeDcColdMeasurenmentsDocument(FlowDocument flowDocument)
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments, flowDocument);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader(flowDocument);

            insertDcColdMeasurenmentsHeader(flowDocument);

            insertDcColdChannel(TempChannelType.A_C, flowDocument);
            insertDcColdChannel(TempChannelType.B_C, flowDocument);
            insertDcColdChannel(TempChannelType.C_A, flowDocument);

            //Потпис
            insertSignature(flowDocument);

            return flowDocument;
        }

        private void insertDcColdMeasurenmentsHeader(FlowDocument flowDocument)
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("HV: " + dataSource.Root.TransformerProperties.HV + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("LV: " + dataSource.Root.TransformerProperties.LV + "\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff HV: " + dataSource.Root.TransformerProperties.HvTempCoefficient + " (" + dataSource.Root.TransformerProperties.HvMaterial + ")\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff LV: " + dataSource.Root.TransformerProperties.LvTempCoefficient + " (" + dataSource.Root.TransformerProperties.LvMaterial + ")\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));
            //Температура
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("T Cold:       " + ReportStringFormater.temperatureFormater(dataSource.retTCold()) + "\n"));
            //Струја со која се тестира.
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Test Current: " + ReportStringFormater.curentFormater(dataSource.Root.DcColdMeasurenments.RessistanceTransformerChannels[0].TestCurrent) + "\n"));


            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }

        private void insertDcColdChannel(TempChannelType tempChannelType,FlowDocument flowDocument)
        {
            // RessistanceTransformerChannel ressistanceTransformerChannel
            insertDcColdMeasurenmentsChannelHeader(tempChannelType,flowDocument);
        }


        /// <summary>
        /// Поставување на табелта со отпори за одреден канал.
        /// </summary>
        /// <param name="tempChannelType"></param>
        private void insertDcColdMeasurenmentsTableTable(TempChannelType tempChannelType)
        {
            List<TempMeasurenment> tempMeasurenments = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments;
            TempMeasurenementConfiguration tc = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());

            tempTable.BorderBrush = System.Windows.Media.Brushes.Black;
            tempTable.BorderThickness = new Thickness(2);

            //Поставување на хедерот
            //insertDcColdMeasurenmentsTableHeader(tempChannelType, tempTable);
        }

        private void insertDcColdMeasurenmentsChannelHeader(TempChannelType tempChannelType,FlowDocument flowDocument)
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Bold(new Run("Channel " + evalChannelName(tempChannelType) + "\n")));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Bold(new Run("\n")));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("STD Temp:            " + ReportStringFormater.temperatureFormater(dataSource.StdTemp) + " \n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType) + " at STD Temp: " + ReportStringFormater.resistanceFormater(dataSource.retR1AtStdTemp(Convert.ToInt32(tempChannelType))) + " \n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType).ToLower() + " at STD Temp: " + ReportStringFormater.resistanceFormater(dataSource.retR2AtStdTemp(Convert.ToInt32(tempChannelType))) + " \n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType)[0] + " at STD Temp:     " + ReportStringFormater.resistanceFormater(dataSource.retR1Phase(Convert.ToInt32(tempChannelType))) + " \n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType).ToLower()[0] + " at STD Temp:     " + ReportStringFormater.resistanceFormater(dataSource.retR2Phase(Convert.ToInt32(tempChannelType))) + " \n"));

            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }

        /// <summary>
        /// Метода која го врќа името на температурниот канал.
        /// </summary>
        /// <param name="tempChannelType">Енумератор според кој се определува за кој канал се работи.</param>
        /// <returns>Името на каналот.</returns>
        private string evalChannelName(TempChannelType tempChannelType)
        {
            switch (tempChannelType)
            {
                case TempChannelType.A_C: return "A - C";
                case TempChannelType.B_C: return "B - C";
                case TempChannelType.C_A: return "C - A";
                default: return "A-C";
            }

        }

        #region Prethodno

        /// <summary>
        /// Креирање на документот за DcColdMeasurenments мерењата.
        /// </summary>
        /// <returns></returns>
        private FlowDocument makeDcColdMeasurenmentsDocument()
        {
            //flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader();

            insertDcColdMeasurenmentsHeader();

            insertDcColdChannel(TempChannelType.A_C);
            insertDcColdChannel(TempChannelType.B_C);
            insertDcColdChannel(TempChannelType.C_A);

            //Потпис
            insertSignature();

            return flowDocument;
        }
        private void insertDcColdMeasurenmentsHeader()
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("HV: " + dataSource.Root.TransformerProperties.HV + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("LV: " + dataSource.Root.TransformerProperties.LV + "\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff HV: " + dataSource.Root.TransformerProperties.HvTempCoefficient + " (" + dataSource.Root.TransformerProperties.HvMaterial + ")\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff LV: " + dataSource.Root.TransformerProperties.LvTempCoefficient + " (" + dataSource.Root.TransformerProperties.LvMaterial + ")\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));
            //Температура
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("T Cold:       " + Math.Round(dataSource.retTCold(), 1) + " C\n"));
            //Струја со која се тестира.
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Test Current: " + Math.Round(dataSource.Root.DcColdMeasurenments.RessistanceTransformerChannels[0].TestCurrent, 1) + " A\n"));


            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }
        /// <summary>
        /// Внесување на делот од документот во кој се опфатени сите информации за еден канал.
        /// </summary>
        /// <param name="ressistanceTransformerChannel"></param>
        private void insertDcColdChannel(TempChannelType tempChannelType)
        {
            // RessistanceTransformerChannel ressistanceTransformerChannel
            insertDcColdMeasurenmentsChannelHeader(tempChannelType);
        }
        /// <summary>
        /// Почетни информации за секој канал посебно.
        /// </summary>
        /// <param name="ressistanceTransformerChannel"></param>
        private void insertDcColdMeasurenmentsChannelHeader(TempChannelType tempChannelType)
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Bold(new Run("Channel " + evalChannelName(tempChannelType) + "\n")));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Bold(new Run("\n")));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("STD Temp:            " + Math.Round(dataSource.StdTemp, 1) + " C\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType) + " at STD Temp: " + Math.Round(dataSource.retR1AtStdTemp(Convert.ToInt32(tempChannelType)), 7) + " Ohm\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType).ToLower() + " at STD Temp: " + Math.Round(dataSource.retR2AtStdTemp(Convert.ToInt32(tempChannelType)), 7) + " Ohm\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType)[0] + " at STD Temp:     " + Math.Round(dataSource.retR1Phase(Convert.ToInt32(tempChannelType)), 7) + " Ohm\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R " + evalChannelName(tempChannelType).ToLower()[0] + " at STD Temp:     " + Math.Round(dataSource.retR2Phase(Convert.ToInt32(tempChannelType)), 7) + " Ohm\n"));

            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }
        #endregion Prethodno

        #endregion

        #region DcCoolingMeasurenments

        private void insertDcCoolingMeasurenmentsHeader(FlowDocument flowDocument)
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Amb Temp - Cold:    " + ReportStringFormater.temperatureFormater(dataSource.retTCold()) + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("End AC Temp:        " + ReportStringFormater.temperatureFormater(dataSource.retEndAcTemp()) + "\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R1 Cold Resistance: " + ReportStringFormater.resistanceFormater(dataSource.retR1ColdAtDcCool()) + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R2 Cold Resistance: " + ReportStringFormater.resistanceFormater(dataSource.retR2ColdAtDcCool()) + " \n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff HV:      " + dataSource.Root.TransformerProperties.HvTempCoefficient + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff LV:      " + dataSource.Root.TransformerProperties.LvTempCoefficient + "\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));
            //Температура
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Total Samples:      " + dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceNoOfSamplesCurrentState + " \n"));
            //Струја со која се тестира.
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Sample Rate:        " + Math.Round(Convert.ToDouble(dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceSampleRateCurrentState), 1) + " S\n"));


            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }
        private void insertDcCoolingMeasurenmentsTableHeader(Table tempTable)
        {
            TableRow headerRow = tempTable.RowGroups[0].Rows[0];

            headerRow.Background = System.Windows.Media.Brushes.LightGray;

            //Поставување на стилот на фондот на хедерот на тбаелата
            headerRow.FontStyle = FontStyles.Oblique;

            //Време
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Time"))));
            //R1
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("R1 "))));
            //Порцентуална разлика од првото мерење на R1.
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("R1 DIFF%"))));
            //R2
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("R 2"))));
            //Порцентуална разлика од првото мерење на R2.
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("R2 DIFF%"))));
        }
        private void insertDcCoolingMeasurenmentsTableCell(Table tempTable)
        {
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns.Add(new TableColumn());
            tempTable.Columns[0].Width = new GridLength(80);
            EntityLayer.ListWithChangeEvents<EntityLayer.RessistanceMeasurenment> meas = dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments;
            foreach (RessistanceMeasurenment rm in dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceMeasurenments)
            {
                //Додавање на нова редица во табелата.
                tempTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow ressRow = tempTable.RowGroups[0].Rows.Last<TableRow>();

                //Време
                ressRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.secondFormater((rm.Time - dataSource.Root.DcCoolingMeasurenments.TNullTime).TotalSeconds)))));
                //R1
                ressRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.resistanceFormater((rm.ChannelNo == 1) ? rm.Voltage / rm.Current : double.NaN)))));
                //Порцентуална разлика од претходното мерење на R1.
                ressRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.percentFormater((rm.ChannelNo == 1) ?
                                               (meas.IndexOf(rm) >= 2 ?
                                                    (100 * (rm.Voltage / rm.Current - meas[meas.IndexOf(rm) - 2].Voltage / meas[meas.IndexOf(rm) - 2].Current) / (meas[meas.IndexOf(rm) - 2].Voltage / meas[meas.IndexOf(rm) - 2].Current))
                                                    : double.NaN)
                                                : double.NaN)))));
                //R2
                ressRow.Cells.Add(new TableCell(new Paragraph(new Run( ReportStringFormater.resistanceFormater((rm.ChannelNo == 2) ? rm.Voltage / rm.Current : double.NaN)))));
                //Порцентуална разлика од претходното мерење на R2.
                ressRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.percentFormater((rm.ChannelNo == 2) ?
                                               (meas.IndexOf(rm) >= 2 ?
                                                    (100 * (rm.Voltage / rm.Current - meas[meas.IndexOf(rm) - 2].Voltage / meas[meas.IndexOf(rm) - 2].Current) / (meas[meas.IndexOf(rm) - 2].Voltage / meas[meas.IndexOf(rm) - 2].Current))
                                                    : double.NaN)
                                                : double.NaN)))));
            }
            
        }
        private void insertDcCoolingMeasurenmentsTable(FlowDocument flowDocument)
        {
            List<TempMeasurenment> tempMeasurenments = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments;
            TempMeasurenementConfiguration tc = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());

            tempTable.BorderBrush = System.Windows.Media.Brushes.Black;
            tempTable.BorderThickness = new Thickness(2);

            //Поставување на хедерот
            insertDcCoolingMeasurenmentsTableHeader(tempTable);

            //Приказ на сите мерења
            insertDcCoolingMeasurenmentsTableCell(tempTable);

            flowDocument.Blocks.Add(tempTable);
        }
        public void insertResistanceChn1(FlowDocument flowDocument)
        {
            Paragraph ResistanceChn1Paragraph = new Paragraph();
            //dataSource.calculateResults();
            //todo treba da sto i presmetka na f(t)
            //dataSource.evalResults();
            ResistanceChn1Paragraph.Inlines.Add(new Bold(new Run("R1 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn1Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + ReportStringFormater.temperatureFormater(dataSource.retFT1()) + " \n"));
            //R1 at time t=0
            ResistanceChn1Paragraph.Inlines.Add(new Run("R1 at time t=0:            " + ReportStringFormater.resistanceFormater(dataSource.retT1Rise()) + " \n"));
            //Temp Rise at time t=0
            ResistanceChn1Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + ReportStringFormater.temperatureFormater( dataSource.retT1T0()) + " \n"));

            flowDocument.Blocks.Add(ResistanceChn1Paragraph);
        }
        public void insertResistanceChn2(FlowDocument flowDocument)
        {
            Paragraph ResistanceChn2Paragraph = new Paragraph();

            ResistanceChn2Paragraph.Inlines.Add(new Bold(new Run("R2 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn2Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + ReportStringFormater.temperatureFormater(dataSource.retFT2()) + " \n"));
            //R1 at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("R2 at time t=0:            " + ReportStringFormater.resistanceFormater(dataSource.retT2Rise()) + " \n"));
            //Temp Rise at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + ReportStringFormater.temperatureFormater(dataSource.retT2T0()) + " \n"));

            flowDocument.Blocks.Add(ResistanceChn2Paragraph);
        }
        private void insertGraphR1(FlowDocument flowDocument)
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();

            // Asign the button as the UI container's child.
            WPFScatterGraph DCCoolingGraph = new WPFScatterGraph();
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;


            //acGraphInit(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            //acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);

            stackPanel.Width = 700;
            //stackPanel.Children.Add(DCCoolingGraph);
            stackPanel.Children.Add(GraphT1);
            myInlineUIContainer.Child = stackPanel;

            graphParagraph.Inlines.Add(myInlineUIContainer);

            flowDocument.Blocks.Add(graphParagraph);

        }
        private void insertGraphR2(FlowDocument flowDocument)
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();

            // Asign the button as the UI container's child.
            WPFScatterGraph DCCoolingGraph = new WPFScatterGraph();
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;

            //acGraphInit(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            //acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);

            stackPanel.Width = 700;
            stackPanel.Height = 400;
            //stackPanel.Children.Add(DCCoolingGraph);
            stackPanel.Children.Add(GraphT2);
            myInlineUIContainer.Child = stackPanel;

            graphParagraph.Inlines.Add(myInlineUIContainer);

            flowDocument.Blocks.Add(graphParagraph);

        }
        private FlowDocument makeDcCoolingMeasurenmentsDocument(FlowDocument flowDocument)
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments,flowDocument);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader(flowDocument);

            insertDcCoolingMeasurenmentsHeader(flowDocument);

            insertDcCoolingMeasurenmentsTable(flowDocument);

            insertResistanceChn1(flowDocument);

            insertGraphR1(flowDocument);

            insertResistanceChn2(flowDocument);

            insertGraphR2(flowDocument);

            //Потпис
            insertSignature(flowDocument);

            return flowDocument;
        }

        #region Prethodni

        private void insertDcCoolingMeasurenmentsHeader()
        {
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();

            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Amb Temp - Cold:    " + dataSource.retTCold() + " C\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("End AC Temp:        " + dataSource.retEndAcTemp() + " C\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R1 Cold Resistance: " + dataSource.retR1ColdAtDcCool() + " Ohm\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("R2 Cold Resistance: " + dataSource.retR2ColdAtDcCool() + " Ohm\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff HV:      " + dataSource.Root.TransformerProperties.HvTempCoefficient + "\n"));
            //
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Temp Coeff LV:      " + dataSource.Root.TransformerProperties.LvTempCoefficient + "\n"));

            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("\n"));
            //Температура
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Total Samples:      " + dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceNoOfSamplesCurrentState + "\n"));
            //Струја со која се тестира.
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Sample Rate:        " + Math.Round(Convert.ToDouble(dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.RessistanceSampleRateCurrentState), 1) + " S\n"));


            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }

        private void insertDcCoolingMeasurenmentsTable()
        {
            List<TempMeasurenment> tempMeasurenments = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration.TempMeasurenments;
            TempMeasurenementConfiguration tc = dataSource.Root.AcHotMeasurenments.TempMeasurenementConfiguration;
            Table tempTable = new Table();
            tempTable.RowGroups.Add(new TableRowGroup());
            tempTable.RowGroups[0].Rows.Add(new TableRow());

            tempTable.BorderBrush = System.Windows.Media.Brushes.Black;
            tempTable.BorderThickness = new Thickness(2);

            //Поставување на хедерот
            insertDcCoolingMeasurenmentsTableHeader(tempTable);

            //Приказ на сите мерења
            insertDcCoolingMeasurenmentsTableCell(tempTable);

            flowDocument.Blocks.Add(tempTable);
        }
        public void insertResistanceChn1()
        {
            Paragraph ResistanceChn1Paragraph = new Paragraph();

            ResistanceChn1Paragraph.Inlines.Add(new Bold(new Run("R1 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn1Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + dataSource.retFT1() + " C\n"));
            //R1 at time t=0
            ResistanceChn1Paragraph.Inlines.Add(new Run("R1 at time t=0:            " + dataSource.retT1Rise() + " C\n"));
            //Temp Rise at time t=0
            ResistanceChn1Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + dataSource.retT1T0() + " C\n"));

            flowDocument.Blocks.Add(ResistanceChn1Paragraph);
        }
        public void insertResistanceChn2()
        {
            Paragraph ResistanceChn2Paragraph = new Paragraph();

            ResistanceChn2Paragraph.Inlines.Add(new Bold(new Run("R2 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn2Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + dataSource.retFT2() + " C\n"));
            //R1 at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("R2 at time t=0:            " + dataSource.retT2Rise() + " C\n"));
            //Temp Rise at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + dataSource.retT2T0() + " C\n"));

            flowDocument.Blocks.Add(ResistanceChn2Paragraph);
        }
        private void insertGraphR1()
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();

            // Asign the button as the UI container's child.
            WPFScatterGraph DCCoolingGraph = new WPFScatterGraph();
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;


            //acGraphInit(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            //acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);

            stackPanel.Width = 900;
            stackPanel.Children.Add(DCCoolingGraph);



            Paragraph inlineParagraph = new Paragraph();
            Floater graphFloater = new Floater();
            //graphFloater.Width = 500;
            graphFloater.HorizontalAlignment = HorizontalAlignment.Stretch;

            myInlineUIContainer.Child = stackPanel;

            inlineParagraph.Inlines.Add(myInlineUIContainer);
            graphFloater.Blocks.Add(inlineParagraph);

            graphParagraph.Inlines.Add(graphFloater);

            flowDocument.Blocks.Add(graphParagraph);

        }
        private void insertGraphR2()
        {
            Paragraph graphParagraph = new Paragraph();

            InlineUIContainer myInlineUIContainer = new InlineUIContainer();

            // Set the BaselineAlignment property to "Bottom" so that the 
            // Button aligns properly with the text.

            StackPanel stackPanel = new StackPanel();

            // Asign the button as the UI container's child.
            WPFScatterGraph DCCoolingGraph = new WPFScatterGraph();
            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            // GraphBackground="LightGray"/>

            //ACHotGraph.MinWidth = 500;


            //acGraphInit(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);
            //acGraphRefresh(ref ACHotGraph, ref seriesOilTemp, ref seriesAmbTemp, ref seriesTempRise);

            stackPanel.Width = 900;
            stackPanel.Children.Add(DCCoolingGraph);



            Paragraph inlineParagraph = new Paragraph();
            Floater graphFloater = new Floater();
            //graphFloater.Width = 500;
            graphFloater.HorizontalAlignment = HorizontalAlignment.Stretch;

            myInlineUIContainer.Child = stackPanel;

            inlineParagraph.Inlines.Add(myInlineUIContainer);
            graphFloater.Blocks.Add(inlineParagraph);

            graphParagraph.Inlines.Add(graphFloater);

            flowDocument.Blocks.Add(graphParagraph);

        }
        private FlowDocument makeDcCoolingMeasurenmentsDocument()
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments);
            //Поставување на стандардните почетни информации за секој извештај.
            insertHeader();

            insertDcCoolingMeasurenmentsHeader();

            insertDcCoolingMeasurenmentsTable();

            insertResistanceChn1();

            insertGraphR1();

            insertResistanceChn2();

            insertGraphR2();

            //Потпис
            insertSignature();

            return flowDocument;
        }
        #endregion Prethodni

        #endregion
    }
}
