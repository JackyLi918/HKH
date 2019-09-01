using HKH.Exchange.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.Common
{
    public abstract class ExportBase<TBody, TBodyList> : ImportExportBase<TBody, TBodyList>, IExportable
        where TBody : class
        where TBodyList : class
    {
        protected const string DEFAULTDATEFORMATSTRING = "MM/dd/yyyy";
        protected const string DEFAULTNUMBERFORMATSTRING = "0.00";

		#region Protected Variable

        protected Export export = null;
		protected int curTIndex;
		protected int rowCount = 0;
		protected string dateFormatString = DEFAULTDATEFORMATSTRING;
        protected string numberFormatSTring = DEFAULTNUMBERFORMATSTRING;
		protected short? dateFormat = null;
		protected ExportMode mode = ExportMode.Export;
		protected int curEIndex;

		#endregion

		#region Constructor

		protected ExportBase(string configurationFile, string tableID)
			: this(configurationFile, tableID, "1")
		{
		}

        protected ExportBase(string configurationFile, string tableID, string exportId)
			: base(configurationFile, tableID)
		{
			this.ExportId = exportId;
			Reset();
		}

		#endregion

		public string ExportId { get; set; }
        public string DateFormatString { get { return dateFormatString; } }
        public string NumberFormatString { get { return numberFormatSTring; } }
		public short? ExcelDateFormat { get { return dateFormat; } }

		#region Public Methods

		/// <summary>
		/// Export data to Excel File
		/// </summary>
		/// <param name="targetFile"></param>
		/// <param name="tList"></param>
		public void Export(string targetFile, TBodyList tList)
		{
			using (Stream stream = File.Create(targetFile))
			{
				Export(stream, tList);
				if (stream.CanWrite)
				{
					stream.Flush();
					stream.Close();
				}
			}
		}

		/// <summary>
		/// Export data to a stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="tList"></param>
		public void Export(Stream stream, TBodyList tList)
		{
            mode = ExportMode.Export;

            export = GetExport(ExportId);
            dateFormatString = string.IsNullOrEmpty(export.DateFormat) ? DEFAULTDATEFORMATSTRING : export.DateFormat;
            numberFormatSTring = string.IsNullOrEmpty(export.NumberFormat) ? DEFAULTNUMBERFORMATSTRING : export.NumberFormat;

			ExportCore(stream,tList);
			Reset();
		}

        protected abstract void ExportCore(Stream stream,TBodyList tList);
 
		#endregion

		#region Protected Virtual Methods

		public abstract  int NextRowNum();
        
		/// <summary>
		/// read next object from source list
		/// </summary>
		/// <param name="tList"></param>
		/// <param name="tObject"></param>
		protected bool NextTObject(TBodyList tList, out TBody tObject)
		{
			curTIndex++;
			if (curTIndex < GetCount(tList))
			{
				tObject = GetTObject(tList, curTIndex);
				return true;
			}

			tObject = null;
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tList"></param>
		/// <returns></returns>
		protected abstract int GetCount(TBodyList tList);

		/// <summary>
		/// get object from source list by index
		/// </summary>
		/// <param name="tList"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		protected abstract TBody GetTObject(TBodyList tList, int index);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected abstract object GetValue(TBody tObj, string propertyName);

    	/// <summary>
		///  Handle before exporting
		/// </summary>
		/// <param name="t"></param>
		/// <param name="tList"></param>
		protected virtual bool ValidateSourceData(TBody tObj, TBodyList tList)
		{
			return true;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// release resource
		/// </summary>
		protected virtual  void Reset()
		{
			curTIndex = -1;
			curEIndex = -1;			
		}

		#endregion

    }
}
