/*******************************************************
 * Filename: Mapping.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	04/10/2015 11:30:15 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;

namespace HKH.Exchange.Configuration
{
    public abstract class ColumnMapping
    {
        protected ColumnMapType mapType;

        protected ColumnMapping()
        {
            this.mapType = ColumnMapType.ExcelHeader;
        }
        protected ColumnMapping(ColumnMapType mapType)
        {
            this.mapType = mapType;
        }

        /// <summary>
        /// Excel column name，example A, B, AB
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// the column index to Source(A, B, AB)
        /// </summary>
        public int ColumnIndex { get; protected set; } = -1;

        /// <summary>
        /// property name of class or column name of datatable
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        internal void CalculateColumnIndex(string[] dataHeaders)
        {
            if (string.IsNullOrEmpty(ColumnName))
                ColumnIndex = -1;

            if (mapType == ColumnMapType.ExcelHeader)
            {
                string temp = ColumnName.Trim().ToUpper();

                if (temp.Length == 2)
                {
                    //(temp[0] - 65 + 1) * 26 + (temp[1] - 65);
                    ColumnIndex = (temp[0] - 64) * 26 + (temp[1] - 65);
                }
                else
                {
                    ColumnIndex = temp[0] - 65;
                }
            }
            else
            {
                ColumnIndex = Array.IndexOf<string>(dataHeaders, ColumnName.Trim());
            }
        }
        internal string GetExcelColumnName()
        {
            if (mapType == ColumnMapType.ExcelHeader)
                return ColumnName;

            if (ColumnIndex == -1)
                return string.Empty;

            // convert index to col name for dataheader column
            string colName = string.Empty;
            if (ColumnIndex < 26)
            {
                colName = ((char)(ColumnIndex + 65)).ToString();
            }
            else
            {
                char ch1 = (char)(ColumnIndex / 26 + 64);
                char ch2 = (char)(ColumnIndex % 26 + 65);
                colName = string.Format("{0}{1}", ch1, ch2);
            }

            return colName;
        }
    }

    public abstract class ExportColumnMapping : ColumnMapping
    {
        protected ExportColumnMapping(Export export)
        {
            this.Export = export;
        }

        public Export Export { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public PropertyType PropertyType { get; set; } = PropertyType.Normal;
    }

    /// <summary>
    /// Mapping
    /// </summary>
    public class ExportHeaderColumnMapping : ExportColumnMapping
    {
        internal ExportHeaderColumnMapping(Export export) : base(export) { }

        /// <summary>
        /// 
        /// </summary>
        public int RowIndex { get; set; } = -1;

        public bool Offset { get; set; }

        public string Location => GetExcelColumnName() + RowIndex.ToString();
    }

    public class ExportBodyColumnMapping : ExportColumnMapping
    {
        private string title = string.Empty;

        internal ExportBodyColumnMapping(Export export) : base(export) { }

        /// <summary>
        /// Column title, defaut is datacolumn name or class proerty name
        /// </summary>
        public string Title
        {
            get { return string.IsNullOrEmpty(title) ? PropertyName : title; }
            set { title = value; }
        }
    }

    public class ImportColumnMapping : ColumnMapping
    {
        internal ImportColumnMapping(Import import) : base(import.ColumnMapType)
        {
        }

        public Import Import { get; private set; }
        /// <summary>
        /// indicate whether set the merged cell value to each model
        /// </summary>
        public bool Inherit { get; set; } = false;

        /// <summary>
        /// if inherit is true, where should the current cell value be copied from
        /// </summary>
        public InheritDirection Direction { get; set; } = InheritDirection.Up;
    }
}