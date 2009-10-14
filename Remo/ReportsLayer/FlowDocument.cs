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

        public FlowDocumentReport(DataSource dataSource, WPFScatterGraph aCHotGraph, WPFScatterGraph graphT1, WPFScatterGraph graphT2)
        {
            this.dataSource = dataSource;
            //todo da se trgne
            //dataSource.Root.TransformerProperties = new EntityLayer.TransformerProperties("12345", "6789", "Gjore", "Nesto", EntityLayer.TransformerProperties.ConnectionType.D, EntityLayer.TransformerProperties.ConnectionType.Y, EntityLayer.TransformerProperties.Material.Aluminium, EntityLayer.TransformerProperties.Material.Aluminium, 20, 20);

            copyGraph(ref ACHotGraph, aCHotGraph);
            copyGraph(ref GraphT1, graphT1);
            copyGraph(ref GraphT2, graphT2);

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
        /// Метода која го врќа името на температурниот канал.
        /// </summary>
        /// <param name="tempChannelType">Енумератор според кој се определува за кој канал се работи.</param>
        /// <returns>Името на каналот.</returns>
        private string evalChannelName(TempChannelType tempChannelType)
        {
            switch (tempChannelType)
            {
                case TempChannelType.A_C: return "A - B";
                case TempChannelType.B_C: return "B - C";
                case TempChannelType.C_A: return "C - A";
            }
            throw new Exception("Should not get here at all");
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

        private void insertSignature(FlowDocument flowDocument)
        {
            Paragraph signatureParagraph = new Paragraph();
            signatureParagraph.Inlines.Add(new Run("Signature:\n"));
            signatureParagraph.Inlines.Add(new Run("\n"));
            signatureParagraph.Inlines.Add(new Run("_______________________\n"));
            flowDocument.Blocks.Add(signatureParagraph);
            flowDocument.Blocks.Last<Block>().Margin = new Thickness(400, 100, 0, 10);
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

        private void insertDcColdMeasurenmentsChannelHeader(TempChannelType tempChannelType, FlowDocument flowDocument)
        {
            string boldChannelName = "Channel " + evalChannelName(tempChannelType) + "\n\n";
            string channel =
                "STD Temp:            " + ReportStringFormater.temperatureFormater(dataSource.StdTemp) + "\n" +
                "R " + evalChannelName(tempChannelType) + " at STD Temp: " + ReportStringFormater.resistanceFormater(dataSource.retR1AtStdTemp(Convert.ToInt32(tempChannelType))) + "\n" +
                "R " + evalChannelName(tempChannelType).ToLower() + " at STD Temp: " + ReportStringFormater.resistanceFormater(dataSource.retR2AtStdTemp(Convert.ToInt32(tempChannelType))) + "\n" +
                "R " + evalChannelName(tempChannelType)[0] + " at STD Temp:     " + ReportStringFormater.resistanceFormater(dataSource.retR1Phase(Convert.ToInt32(tempChannelType))) + "\n" +
                "R " + evalChannelName(tempChannelType).ToLower()[0] + " at STD Temp:     " + ReportStringFormater.resistanceFormater(dataSource.retR2Phase(Convert.ToInt32(tempChannelType)));
            
            Paragraph DcColdMeasurenmentsHeaderParagraph = new Paragraph();
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Bold(new Run(boldChannelName)));
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(channel);
            flowDocument.Blocks.Add(DcColdMeasurenmentsHeaderParagraph);
        }
        
        #endregion

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
            tempTable.Columns[0].Width = new GridLength(30);

            flowDocument.Blocks.Add(tempTable);
        }

        /// <summary>
        /// Поставување на хедерот на табелата за температурните мерења за AcHotMeasurenments.
        /// </summary>
        /// <param name="tc">TempMeasurenementConfiguration.</param>
        /// <param name="tempTable">Табелата во документот.</param>
        private void insertTempMeasurenementConfiguration_TempMeasurenments_Header(TempMeasurenementConfiguration tc, Table tempTable)
        {
            TableRow headerRow = tempTable.RowGroups[0].Rows[0];

            headerRow.Background = System.Windows.Media.Brushes.LightGray;

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

                //Т1
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T1)))));
                //Т2
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T2)))));
                //Т3
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T3)))));
                //Т4
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tm.T4)))));
                //TAmb
                double tAmb = ((((!tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((!tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((!tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((!tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((!tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0)));
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tAmb)))));
                double tOil = (((tc.IsChannel1Oil && tc.IsChannel1On) ? tm.T1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? tm.T2 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? tm.T3 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? tm.T4 : 0)) / (((tc.IsChannel1Oil && tc.IsChannel1On) ? 1 : 0) + ((tc.IsChannel2Oil && tc.IsChannel2On) ? 1 : 0) + ((tc.IsChannel3Oil && tc.IsChannel3On) ? 1 : 0) + ((tc.IsChannel4Oil && tc.IsChannel4On) ? 1 : 0));
                //TOil
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tOil)))));
                double tRise = tOil = tAmb;
                //Разлика во температурите во воздух и во масло.
                tempRow.Cells.Add(new TableCell(new Paragraph(new Run(ReportStringFormater.temperatureFormater(tRise)))));

                if (tm.IsSampleReduced)
                {
                    tempTable.RowGroups[0].Rows.Add(new TableRow());
                    tempRow = tempTable.RowGroups[0].Rows.Last<TableRow>();
                    tempRow.Cells.Add(new TableCell(new Paragraph(new Run("-"))));
                    for (int i = 0; i < 8; i++)
                    {
                        tempRow.Cells.Add(new TableCell(new Paragraph(new Run("Reduced"))));
                    }
                }
            }
        }

        private void insertMeanTempFields(FlowDocument flowDocument)
        {
            Paragraph meanTempFieldsParagraph = new Paragraph();
            meanTempFieldsParagraph.Inlines.Add(new Run("Avg Oil Temp (AOT): " + ReportStringFormater.temperatureFormater(dataSource.EndAcTemp) + " \n"));
            meanTempFieldsParagraph.Inlines.Add(new Run("K drop in Oil:      " + ReportStringFormater.temperatureFormater(dataSource.KDropInOil) + "\n"));
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
            stackPanel.Width = 900;
            stackPanel.Children.Add(ACHotGraph);

            myInlineUIContainer.Child = stackPanel;
            graphParagraph.Inlines.Add(myInlineUIContainer);

            flowDocument.Blocks.Add(graphParagraph);
            flowDocument.Blocks.Last().FontSize = 10;

        }

        private void copyGraph(ref WPFScatterGraph newGraph, WPFScatterGraph oldGraph)
        {
            newGraph = new WPFScatterGraph();
            newGraph.InitializeComponent();
            newGraph.Margin = oldGraph.Margin;
            newGraph.XAxisType = oldGraph.XAxisType;
            newGraph.XAxisTitle = oldGraph.XAxisTitle;
            newGraph.YAxisTitle = oldGraph.YAxisTitle;


            newGraph.MaxYRange = oldGraph.MaxYRange;
            newGraph.MinYRange = oldGraph.MinYRange;

            newGraph.MinXRange = oldGraph.MinXRange;
            newGraph.MaxXRange = oldGraph.MaxXRange;

            newGraph.IntervalYRange = oldGraph.IntervalYRange;
            newGraph.IntervalXRange = oldGraph.IntervalXRange;

            WPFGraphSeries seriesOilTemp = new WPFGraphSeries();
            WPFGraphSeries seriesAmbTemp = new WPFGraphSeries();
            WPFGraphSeries seriesTempRise = new WPFGraphSeries();

            foreach (WPFGraphSeries series in oldGraph.Series)
            {
                newGraph.Series.Add(series);
            }

            newGraph.InitializeComponent();
            newGraph.Refresh();
        }


        private FlowDocument makeAcHotMeasurenmentsDocument(FlowDocument flowDocument)
        {
            flowDocument.Blocks.Clear();
            //Поставување на насловот
            insertTitle(FlowDocumentReportType.AcHotMeasurenments, flowDocument);
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
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Total Samples:      " + dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.NoOfSamples + " \n"));
            //Струја со која се тестира.
            DcColdMeasurenmentsHeaderParagraph.Inlines.Add(new Run("Sample Rate:        " + ReportStringFormater.secondFormater(Convert.ToDouble(dataSource.Root.DcCoolingMeasurenments.RessistanceTransformerChannel.SampleRate)) + "\n"));


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
            //Todo 
            //dataSource.calculateResults();
            //todo treba da sto i presmetka na f(t)
            //dataSource.evalResults();
            ResistanceChn1Paragraph.Inlines.Add(new Bold(new Run("R1 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn1Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + ReportStringFormater.temperatureFormater(dataSource.retFT1()) + " \n"));
            //R1 at time t=0
            ResistanceChn1Paragraph.Inlines.Add(new Run("R1 at time t=0:            " + ReportStringFormater.resistanceFormater(dataSource.retR1T0()) + " \n"));
            //Temp Rise at time t=0 
            ResistanceChn1Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + ReportStringFormater.temperatureFormater(dataSource.retT1Rise()) + " \n"));
            //AOT
            ResistanceChn1Paragraph.Inlines.Add(new Run("AOT:                       " + ReportStringFormater.temperatureFormater(dataSource.retAOT1()) + " \n"));
            //R at AOT
            ResistanceChn1Paragraph.Inlines.Add(new Run("R1 at AOT:                 " + ReportStringFormater.resistanceFormater(dataSource.retR1AtAOT()) + " \n"));

            flowDocument.Blocks.Add(ResistanceChn1Paragraph);
        }
        public void insertResistanceChn2(FlowDocument flowDocument)
        {
            Paragraph ResistanceChn2Paragraph = new Paragraph();

            ResistanceChn2Paragraph.Inlines.Add(new Bold(new Run("R2 - Resistance at Channel 1\n")));
            //Exponential Curve
            ResistanceChn2Paragraph.Inlines.Add(new Run("Exponential Curve f(t):    " + ReportStringFormater.temperatureFormater(dataSource.retFT2()) + " \n"));
            //R1 at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("R2 at time t=0:            " + ReportStringFormater.resistanceFormater(dataSource.retR2T0()) + " \n"));
            //Temp Rise at time t=0
            ResistanceChn2Paragraph.Inlines.Add(new Run("Temp Rise at time t=0:     " + ReportStringFormater.temperatureFormater(dataSource.retT2Rise()) + " \n"));
            //AOT
            ResistanceChn2Paragraph.Inlines.Add(new Run("AOT:                       " + ReportStringFormater.temperatureFormater(dataSource.retAOT2()) + " \n"));
            //R at AOT
            ResistanceChn2Paragraph.Inlines.Add(new Run("R2 at AOT:                 " + ReportStringFormater.resistanceFormater(dataSource.retR2AtAOT()) + " \n"));


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

        #endregion
    }
}
