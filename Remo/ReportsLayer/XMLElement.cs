using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml.Linq;
using System.Text;
using DataAccessLayer;
using EntityLayer;
using System.Xml;

namespace ReportsLayer.XMLElement
{
    public enum CellTypesEnum { _double, _int, _decimal, _string, _number };

    class Workbook : XElement
    {

        public Workbook(TransformerProperties transformerProperties)
            : base("{urn:schemas-microsoft-com:office:spreadsheet}Workbook")
        {
            this.Add(new XAttribute("xmlns:o", "urn:schemas-microsoft-com:office:office"));
            this.Add(new XAttribute("xmlns:x", "urn:schemas-microsoft-com:office:excel"));
            this.Add(new XAttribute("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet"));
            this.Add(new XAttribute("xmlns:html", "http://www.w3.org/TR/REC-html40"));

            this.Add(new XElement("{urn:schemas-microsoft-com:office:office}DocumentProperties",
                new XElement("Author", transformerProperties.PresentAtTest), new XElement("LastAuthor", transformerProperties.PresentAtTest), new XElement("Created", DateTime.Now.ToLongDateString()), new XElement("Company", "EMO"), new XElement("Version", "12")));

            this.Add(new ExcelWorkbook());
            this.Add(new XElement("Styles"));
        }
        void addStyle(CellStyle cellStyle) 
        {
            this.Element("Styles").Add(cellStyle);
        }
        void addWorkSheet(Worksheet worksheet)
        {
            this.Add(worksheet);
        }
        /*
        public class ExcelWorkbookElement:XStreamingElement
        {
            public int WindowHeight {get;set;}
            public int WindowWidth { get; set; }
            public int WindowTopX { get; set; }
            public int WindowTopY { get; set; }
            public bool ProtectStructure { get; set; }
            public bool ProtectWindows { get; set; }

            public ExcelWorkbookElement() : base("{urn:schemas-microsoft-com:office:excel}ExcelWorkbook") 
            {
                this.Add(new XStreamingElement("WindowHeight", WindowHeight), new XStreamingElement("WindowWidth", WindowWidth), new XStreamingElement("WindowWidth", WindowWidth));
            }
        }
        */
    }
    class ExcelWorkbook:XElement 
    {
        public ExcelWorkbook():base("{urn:schemas-microsoft-com:office:excel}ExcelWorkbook")
        {
            this.Add(@"<WindowHeight>10170</WindowHeight>
                <WindowWidth>20895</WindowWidth>
                <WindowTopX>360</WindowTopX>
                <WindowTopY>15</WindowTopY>
                <ProtectStructure>False</ProtectStructure>
                <ProtectWindows>False</ProtectWindows>");
        }
        public ExcelWorkbook(int windowHeight, int windowWidth, int windowTopX, int windowTopY, int protectStructure, int protectWindows)
            : base("{urn:schemas-microsoft-com:office:excel}ExcelWorkbook")
        {
            this.Add(new XElement("WindowHeight",windowHeight),new XElement("WindowWidth",windowWidth),new XElement("WindowTopX",windowTopX),new XElement("WindowTopY",windowTopY),new XElement("ProtectStructure",protectStructure),new XElement("ProtectWindows",protectWindows));

        }
    }
    public static class Utils
    {

        public static string CellTypeString(CellTypesEnum cellTypesEnum)
        {
            switch (cellTypesEnum) 
            {
                case CellTypesEnum._decimal: return "Decimal";
                case CellTypesEnum._double: return "Double";
                case CellTypesEnum._int: return "Integer";
                case CellTypesEnum._string: return "String";
                default: return "String";
            }
        }
        public static CellTypesEnum typeToEnum(Type type)
        {
            switch (type.ToString()) 
            {
                case "Int32": return CellTypesEnum._number;
                case "Double": return CellTypesEnum._number;
                case "Decimal": return CellTypesEnum._number;
                case "String": return CellTypesEnum._string;
                default: return CellTypesEnum._string;
            }
        }
    }
    class CellStyle:XElement
    {
        public string styleName { get; set; }
        public CellStyle() : base("") { }
    }
    class Cell : XElement 
    {
        /// <summary>
        /// Конструктор на водечка келија.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index"></param>
        /// <param name="StyleName"></param>
        /// <param name="cellTypesEnum"></param>
        public Cell(string content,int index, string StyleName,CellTypesEnum cellTypesEnum):base("Cell")
        {
            this.Add(new XAttribute("ss:Index", index));
            this.Add(new XAttribute("ss:StyleID", StyleName));
            this.Add(new XElement("Data"));
            this.Element("Data").Add(new XAttribute("ss:Type", Utils.CellTypeString(cellTypesEnum)));
            this.Element("Data").Value = content;
        }
        /// <summary>
        /// КОнструктор на останти келии.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="StyleName"></param>
        /// <param name="cellTypesEnum"></param>
        public Cell(string content, string StyleName, CellTypesEnum cellTypesEnum)
            : base("Cell")
        {
            this.Add(new XAttribute("ss:StyleID", StyleName));
            this.Add(new XElement("Data"));
            this.Element("Data").Add(new XAttribute("ss:Type", Utils.CellTypeString(cellTypesEnum)));
            this.Element("Data").Value = content;
        }
    }
    class Table:XElement 
    {
        public Table(int expandedColumnCount, int expandedRowCount, int fullColumns, int fullRows, int defaultRowHeight)
            : base("Table") 
        {
            this.Add(new XAttribute("ss:ExpandedColumnCount", expandedColumnCount));
            this.Add(new XAttribute("ss:ExpandedRowCount", expandedRowCount));
            this.Add(new XAttribute("x:FullColumns", fullColumns));
            this.Add(new XAttribute("x:FullRows", fullRows));
            this.Add(new XAttribute("ss:DefaultRowHeight", defaultRowHeight));
        }

        public Table(int expandedColumnCount, int expandedRowCount, int fullColumns, int fullRows, int defaultRowHeight,int indexR,int indexC, DataTable dataTable)
            :this(expandedColumnCount, expandedRowCount, fullColumns, fullRows, defaultRowHeight)
        {
            int indexRow=indexR;
            List<Cell> cells=new List<Cell>();
            CellStyle cellStyle=new CellStyle();
            CellStyle headerStyle=new CellStyle();
            int numberColumns=dataTable.Columns.Count;
            Row row;

            //Првата келија од хедер редицата. 
            cells.Add(new Cell(dataTable.Columns[0].ColumnName, indexRow++, headerStyle.styleName, CellTypesEnum._string));
            //Внесуванје на хедерот на табелата
            for (int i = 1; i < dataTable.Columns.Count;i++)
            {
                cells.Add(new Cell(dataTable.Columns[i].ColumnName, headerStyle.styleName, CellTypesEnum._string));
            }
            //Внесување на хедер редицата
            row=new Row(this,cells,indexC);

            //Венсување на редиците во табелата.
            foreach (DataRow dRows in dataTable.Rows) 
            {
                cells = new List<Cell>();
                //Првата келија од хедер редицата. 
                cells.Add(new Cell(dRows[0].ToString(), indexRow++, cellStyle.styleName, Utils.typeToEnum(dataTable.Columns[0].GetType())));
                //Дефинирање на келијата.
                for (int j = 1; j < numberColumns; j++) 
                {
                    cells.Add(new Cell(dRows[j].ToString(), cellStyle.styleName, Utils.typeToEnum(dataTable.Columns[j].GetType())));
                }
                //Додавање на редицата.
                row = new Row(this, cells);
            }
        }
    }
    class Row : XElement 
    {
        /// <summary>
        /// Конструктор за водечката редица во табелата.
        /// </summary>
        /// <param name="table">Табелата на која припаѓа редицата.</param>
        /// <param name="cells">Келиите кои ги содржи.</param>
        /// <param name="index">Индексот.</param>
        /// <param name="height">Висината на редицата во табелите.</param>
        public Row(Table table, List<Cell> cells,int index,int height)
            : this(table, cells, index)
        {
            //Висината на редицата.
            this.Add(new XAttribute("ss:Height",height));
        }
        /// <summary>
        /// КОнструктор за останатите редици.
        /// </summary>
        /// <param name="table">Табелата на која припаѓа редицата.</param>
        /// <param name="cells">Келиите кои ги содржи.</param>
        public Row(Table table,List<Cell> cells) : base("Row") 
        {
            //Додавање на келиите.
            this.Add(cells);
            //Додавање на редицата на табелата.
            table.Add(this);
        }

        public Row(Table table, List<Cell> cells, int index)
            : this(table, cells)
        {
            //Индексот
            this.Add(new XAttribute("ss:Index", index));
        }
        
    }
    /// <summary>
    /// Хедерот на табелата
    /// </summary>
    class TableHeader : Row
    {
        public TableHeader(Table table, List<Cell> cells,int index,int height): base (table, cells, index, height)
        {
            
        }
    }
    class Worksheet:XElement 
    {
        public Worksheet(string sheetName):base("Worksheet")
        {
            this.Add(new XAttribute("ss:Name", sheetName));
        }
        public void addWorksheetOptions(bool selected)
        {
            this.Add(new WorksheetOptions(selected));
        }
    }
    class WorksheetOptions : XElement 
    {
        public WorksheetOptions(bool selected) : base("{urn:schemas-microsoft-com:office:excel}WorksheetOptions") 
        {
            this.Add("<PageSetup><Header x:Margin=\"0.3\"/><Footer x:Margin=\"0.3\"/><PageMargins x:Bottom=\"0.75\" x:Left=\"0.7\" x:Right=\"0.7\" x:Top=\"0.75\"/></PageSetup>");
            if (selected)
            {
                this.Add("<Selected/>");
                this.Add(@"<Panes>
    <Pane>
     <Number>3</Number>
     <ActiveRow>10</ActiveRow>
     <ActiveCol>11</ActiveCol>
    </Pane>
   </Panes>");
            }
            this.Add(@"<ProtectObjects>False</ProtectObjects>
   <ProtectScenarios>False</ProtectScenarios>");
        }
    }
}