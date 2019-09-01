/*******************************************************
 * Filename: ImportBase.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	05/26/2015 5:06:25 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKH.Common;
using HKH.Exchange.Configuration;

namespace HKH.Exchange.Common
{
	/// <summary>
	/// ImportBase
	/// </summary>
	public abstract class ImportBase<Row, Sheet, T, TList> : ImportExportBase<T, TList>, IImportable<T, TList>
		where Row : class
		where Sheet : class
		where T : class
		where TList : class
	{
		protected TList successList = null;
		protected TList failList = null;
		protected Sheet sheet = null;

		#region Constructor

		protected ImportBase(string configurationFile, string tableID)
			: this(configurationFile, tableID, "1")
		{
		}

		protected ImportBase(string configurationFile, string tableID, string importId)
			: base(configurationFile, tableID)
		{
			Reset();
			this.ImportId = importId;
		}

		#endregion

		public string ImportId { get; set; }

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceFile"></param>
		/// <param name="successList"></param>
		/// <param name="failList">ignore fail record if null</param>
		public void Import(string sourceFile, TList successList, TList failList)
		{
			if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
				throw Error.ArgumentNotValid("sourceFile");
			if (successList == null)
				throw Error.ArgumentNull("successList");

			this.successList = successList;
			this.failList = failList;

			try
			{
				Import import = GetImport(ImportId);

				PrepareWorkSheet(import, sourceFile, _tableMapping.Sheet);

				//cache data to fill the merged cell
				Row cachRow = GetRow(0);

                for (int i = import.FirstRowIndex; i <= GetLastRowIndex(sheet); i++)
				{
					Row row = GetRow(i);

					//process data before importing, not to import if false is returned
					if (!ValidateSourceData(row, sheet))
					{
						continue;
					}

					T t = GetTInstance();

					foreach (ImportColumnMapping columnMapping in import.Values)
					{
						if (IsValidCell (row,columnMapping .ColumnIndex ))
						{
							//allow to fill
							if (columnMapping.Inherit)
							{
								//process multicolumns merged
								if (columnMapping.Direction == InheritDirection.Left)
								{
									//cache left cell value
									SetCellData(cachRow, columnMapping.ColumnIndex, GetCellData(row, columnMapping.ColumnIndex -1));
								}
								//if multirows merged, do nothing,because the cacheRow is newest value.

								//set value with newest cache
								SetValue(t, columnMapping.PropertyName, GetCellData(cachRow , columnMapping.ColumnIndex ));
							}
							else
								SetValue(t, columnMapping.PropertyName, null);
						}
						else
						{
							//update cache
							SetCellData(cachRow, columnMapping.ColumnIndex, GetCellData(row, columnMapping.ColumnIndex));
							//set value with current value
							SetValue(t, columnMapping.PropertyName, GetCellData(row, columnMapping.ColumnIndex));
						}
					}

					//process after importing, not to save if false is returned
					if (ValidateTargetData(t, successList))
						AppendToTList(t, successList);
					else
						AppendToTList(t, failList);
				}
			}
			finally
			{
				Reset();
			}
		}

		#endregion

		#region Protected Virtual Methods

		#region To override by CSVImport / ExcelImport
		protected abstract Row GetRow(int index);

        protected abstract int GetLastRowIndex(Sheet sheet);
        
		protected abstract bool IsValidCell(Row row, int colIndex);

		protected abstract object GetCellData(Row row, int colIndex);
		protected abstract void SetCellData(Row row, int colIndex, object value);

		#endregion

		/// <summary>
		/// Load the excel file to NPOI worksheet
		/// </summary>
		/// <param name="xlsFile"></param>
		/// <param name="sheetName"></param>
		protected abstract void PrepareWorkSheet(Import import, string xlsFile, string sheetName);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected abstract T GetTInstance();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected abstract void SetValue(T tObj, string propertyName, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="tList"></param>
		protected abstract void AppendToTList(T tObj, TList tList);

		/// <summary>
		/// Handle before importing
		/// </summary>
		/// <param name="excelRow"></param>
		/// <param name="dtExcel"></param>
		protected virtual bool ValidateSourceData(Row row, Sheet sheet)
		{
			return true;
		}

		/// <summary>
		///  Handle after importing
		/// </summary>
		/// <param name="t"></param>
		protected virtual bool ValidateTargetData(T tObj, TList tList)
		{
			return true;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		protected virtual void Reset()
		{
			successList = null;
			failList = null;
			sheet = null;
		}

		#endregion
	}
}
