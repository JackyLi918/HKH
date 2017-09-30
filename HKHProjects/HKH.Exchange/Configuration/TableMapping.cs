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

namespace HKH.Exchange.Configuration
{
    public interface IImportExportConfiguration
    {
    }

    public class TableMapping
    {
        private ExportCollection _exports = null;
        private ImportCollection _imports = null;

        public TableMapping()
        {
            Id = string.Empty;
            Sheet = string.Empty;
            ClassType = string.Empty;
            _exports = new ExportCollection();
            _imports = new ImportCollection();
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
        /// Class Name or DataTable Name
        /// </summary>
        public string ClassType { get; set; }

        public ExportCollection Exports
        {
            get { return _exports; }
            internal set { _exports = value; }
        }

        public ImportCollection Imports
        {
            get { return _imports; }
            internal set { _imports = value; }
        }
    }

    public class ExportCollection : Dictionary<string, Export>
    {
    }

    public class Export : IImportExportConfiguration
    {
        internal Export()
        {
            Id = string.Empty;
            DateFormat = string.Empty;
            XlsFormat = XlsFormat.Xlsx;
        }

        /// <summary>
        /// key field
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// specified date format
        /// </summary>
        public string DateFormat { get; set; }
        public string NumberFormat { get; set; }

        public XlsFormat XlsFormat { get; set; }

        public BasicExport BasicMapping { get; set; }
        public DetailsExport DetailsMapping { get; set; }
    }

    public class BasicExport : Dictionary<string, BasicExportColumnMapping>
    {
    }

    public class DetailsExport : Dictionary<string, DetailsExportColumnMapping>
    {
        internal DetailsExport()
        {
            OutPutTitle = false;
            FirstRowIndex = 0;
            RowMode = FillRowMode.New;
        }

        /// <summary>
        /// indicate whether output colomn name/title as first row
        /// </summary>
        public bool OutPutTitle { get; set; }

        /// <summary>
        /// the row index to start output detail
        /// </summary>
        public int FirstRowIndex { get; set; }

        /// <summary>
        /// detail row count per page
        /// </summary>
        public int PageSize { get; set; }

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
            Id = string.Empty;
            FirstRowIndex = 0;
            XlsFormat = XlsFormat.Auto;
        }

        /// <summary>
        /// key field
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// the row index to start import
        /// </summary>
        public int FirstRowIndex { get; set; }

        public XlsFormat XlsFormat { get; set; }
    }
}