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
        //protected string colName = string.Empty;
        protected int colIndex = -1;
        protected ColumnMapType mapType;

        protected ColumnMapping()
        {
            this.mapType = ColumnMapType.ExcelHeader;
            PropertyName = string.Empty;
        }
        protected ColumnMapping(ColumnMapType mapType)
        {
            this.mapType = mapType;
            PropertyName = string.Empty;
        }

        /// <summary>
        /// Excel column name，example A, B, AB
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// the column index to Source(A, B, AB)
        /// </summary>
        public int ColumnIndex
        {
            get { return colIndex; }
        }

        /// <summary>
        /// property name of class or column name of datatable
        /// </summary>
        public string PropertyName { get; set; }

        internal void CalculateColumnIndex(string[] dataHeaders)
        {
            if (string.IsNullOrEmpty(ColumnName))
                colIndex = -1;

            if (mapType == ColumnMapType.ExcelHeader)
            {
                string temp = ColumnName.Trim().ToUpper();

                if (2 == temp.Length)
                {
                    //(temp[0] - 65 + 1) * 26 + (temp[1] - 65);
                    colIndex = (temp[0] - 64) * 26 + (temp[1] - 65);
                }
                else
                {
                    colIndex = temp[0] - 65;
                }
            }
            else
            {
                colIndex = Array.IndexOf<string>(dataHeaders, ColumnName.Trim());
            }
        }
        internal string GetExcelColumnName()
        {
            if (mapType == ColumnMapType.ExcelHeader)
                return ColumnName;

            if (colIndex == -1)
                return string.Empty;

            // convert index to col name for dataheader column
            string colName = string.Empty;
            if (colIndex < 26)
            {
                colName = ((char)(colIndex + 65)).ToString();
            }
            else
            {
                char ch1 = (char)(colIndex / 26 + 64);
                char ch2 = (char)(colIndex % 26 + 65);
                colName = string.Format("{0}{1}", ch1, ch2);
            }

            return colName;
        }
    }

    public abstract class ExportColumnMapping : ColumnMapping
    {
        private PropertyType _propType = PropertyType.Normal;

        protected ExportColumnMapping() { }
        protected ExportColumnMapping(ColumnMapType mapType) : base(mapType) { }

        /// <summary>
        /// 
        /// </summary>
        public PropertyType PropertyType
        {
            get { return _propType; }
            set { _propType = value; }
        }
    }

    /// <summary>
    /// Mapping
    /// </summary>
    public class ExportHeaderColumnMapping : ExportColumnMapping
    {
        protected int rowIndex = -1;

        internal ExportHeaderColumnMapping() { }
        //internal ExportHeaderColumnMapping(ColumnMapType mapType) : base(mapType) { }

        /// <summary>
        /// 
        /// </summary>
        public int RowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }

        public bool Offset { get; set; }

        public string Location
        {
            get
            {
                return GetExcelColumnName() + rowIndex.ToString();
            }
        }
    }

    public class ExportBodyColumnMapping : ExportColumnMapping
    {
        private string title = string.Empty;

        internal ExportBodyColumnMapping() { }
        //internal ExportBodyColumnMapping(ColumnMapType mapType) : base(mapType) { }

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
        internal ImportColumnMapping()
        {
            Inherit = false;
            Direction = InheritDirection.Up;
        }

        internal ImportColumnMapping(ColumnMapType mapType) : base(mapType) { }


        /// <summary>
        /// indicate whether set the merged cell value to each model
        /// </summary>
        public bool Inherit { get; set; }

        /// <summary>
        /// if inherit is true, where should the current cell value be copied from
        /// </summary>
        public InheritDirection Direction { get; set; }
    }
}