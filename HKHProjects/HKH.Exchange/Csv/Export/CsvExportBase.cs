/*******************************************************
 * Filename: CsvExportBase.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	06/19/2015 12:08:01 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.IO;
using HKH.CSV;
using HKH.Exchange.Common;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.CSV
{
    /// <summary>
    /// CSVExportBase
    /// </summary>
    public abstract class CSVExportBase<TBody, TBodyList> : ExportBase<TBody, TBodyList>
        where TBody : class
        where TBodyList : class
    {

        #region Protected Variable

        CSVWriter _writer = null;

        #endregion

        #region Constructor

        protected CSVExportBase() { }

        protected CSVExportBase(string configurationFile, string tableID)
            : this(configurationFile, tableID, "1")
        {
        }

        protected CSVExportBase(string configurationFile, string tableID, string exportId)
            : base(configurationFile, tableID)
        {
            this.ExportId = exportId;
            Reset();
        }

        #endregion

        #region Public Methods

        protected override void ExportCore(Stream stream, TBodyList tList)
        {
            _writer = new CSVWriter(stream);

            OutputDetails(tList);

            _writer.Flush();
        }

        #endregion

        #region Protected Virtual Methods

        public override int NextRowNum()
        {
            curEIndex++;
            return Setting.Body.FirstRowIndex + curEIndex;
        }

        #endregion

        #region Private Methods

        private void OutputDetails(TBodyList tList)
        {
            //write columns' title
            if (mode == ExportMode.Export && Setting.Body.OutPutTitle)
            {
                foreach (ExportBodyColumnMapping columnMapping in Setting.Body.Values)
                {
                    //write column title
                    _writer.Write(columnMapping.Title);
                }
                _writer.WriteNewLine();
            }

            TBody tObj = null;

            int rowIndex = 0;
            while (NextTObject(tList, out tObj))
            {
                if (ValidateSourceData(tObj, tList))
                {
                    //write data
                    foreach (ExportBodyColumnMapping columnMapping in Setting.Body.Values)
                    {
                        object val = GetValue(tObj, columnMapping.PropertyName);
                        if (val is DateTime)
                            _writer.Write(Convert.ToDateTime(val).ToString(dateFormatString));
                        else if (Utility.IsFloat(val))
                            _writer.Write(Convert.ToDouble(val).ToString(numberFormatSTring));
                        else
                            _writer.Write(val == null ? string.Empty : val.ToString());
                    }

                    _writer.WriteNewLine();
                    rowIndex++;
                }
            }
        }

        protected override void Reset()
        {
            base.Reset();

            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }

        #endregion
    }
}
