/*******************************************************
 * Filename: TableMapping.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/10/2015 11:55:24 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Collections.Generic;
using System.Linq;

namespace HKH.Exchange.Configuration
{
    public interface IImportExportConfiguration
    {
        void CalculateColumnIndex(string[] dataHeaders);
    }

    public class TableMapping
    {
        public TableMapping()
        {
            Id = string.Empty;
            ClassType = string.Empty;
        }

        /// <summary>
        /// key field
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Class Name or DataTable Name
        /// </summary>
        public string ClassType { get; set; }

        public ExportCollection Exports { get; internal set; } = new ExportCollection();

        public ImportCollection Imports { get; internal set; } = new ImportCollection();
    }

    public class ExportCollection : Dictionary<string, Export>
    {
    }

    public class Export : IImportExportConfiguration
    {
        internal Export()
        {
            Id = string.Empty;
            Sheet = "Sheet1";
            DateFormat = string.Empty;
            XlsFormat = XlsFormat.Xlsx;
        }

        /// <summary>
        /// key field
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Excel Source(TableName or TableIndex)
        /// </summary>
        public string Sheet { get; set; }
        /// <summary>
        /// specified date format
        /// </summary>
        public string DateFormat { get; set; }
        public string NumberFormat { get; set; }

        public XlsFormat XlsFormat { get; set; }

        public ExportHeader Header { get; set; }
        public ExportBody Body { get; set; }

        public void CalculateColumnIndex(string[] dataHeaders)
        {
            if (this.Header != null && Header.Count > 0)
                this.Header.Values.Do(cm => cm.CalculateColumnIndex(dataHeaders));

            if (this.Body != null && Body.Count > 0)
            {
                this.Body.Values.Do(cm => cm.CalculateColumnIndex(dataHeaders));
                Body.MaxColumnIndex = Body.Max(c => c.Value.ColumnIndex);
            }
        }
    }

    public class ExportHeader : Dictionary<string, ExportHeaderColumnMapping>
    {
        internal ExportHeader(Export export)
        {
            this.Export = export;
        }

        public Export Export { get; private set; }
    }

    public class ExportBody : Dictionary<string, ExportBodyColumnMapping>
    {
        internal ExportBody(Export export)
        {
            OutPutTitle = false;
            FirstRowIndex = 0;
            RowMode = FillRowMode.New;
            this.Export = export;
        }
        public Export Export { get; private set; }
        /// <summary>
        /// indicate whether output colomn name/title as first row
        /// </summary>
        public bool OutPutTitle { get; set; }

        /// <summary>
        /// the row index to start output detail
        /// </summary>
        public int FirstRowIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FillRowMode RowMode { get; set; }

        /// <summary>
        /// the max column index after exported
        /// </summary>
        public int MaxColumnIndex { get; set; }
    }

    public class ImportCollection : Dictionary<string, Import>
    {
    }

    public class Import : Dictionary<string, ImportColumnMapping>, IImportExportConfiguration
    {
        internal Import()
        {
        }
        internal Import(ColumnMapType mapType)
        {
            Id = string.Empty;
            Sheet = "0";
            FirstRowIndex = 0;
            XlsFormat = XlsFormat.Auto;
            this.ColumnMapType = mapType;
        }
        /// <summary>
        /// key field
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Excel Source(TableName or TableIndex)
        /// </summary>
        public string Sheet { get; set; }
        /// <summary>
        /// the row index to start import
        /// </summary>
        public int FirstRowIndex { get; set; }

        public XlsFormat XlsFormat { get; set; }

        public ColumnMapType ColumnMapType { get; internal set; } = ColumnMapType.ExcelHeader;

        public void CalculateColumnIndex(string[] dataHeaders)
        {
            this.Values.Do(cm => cm.CalculateColumnIndex(dataHeaders));
        }
    }
}