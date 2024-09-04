/*******************************************************
 * Filename: ExportExpression.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	2/22/2013 11:13:01 AM
 * Author:	JackyLi
 * 
*****************************************************/
using System;
using System.Text.RegularExpressions;

namespace HKH.Exchange.Excel
{
	/// <summary>
	/// ExportExpression
	/// </summary>
	public class ExportExpression
	{
		private const string FUN_SrcRowNum = "SrcRowNum";
		private const string FUN_RowNum = "XlsRowNum";
		private const string Pattern_GetFNameAndFParam = @"^\{(?<FName>\w+)\((?<FParam>([+-]?\d+))\)\}$";

		#region Variables

		#endregion

		#region Properties

		#endregion

		#region Methods

		public static string Eval(string exp, int xlsRowIndex, int sourceRowIndex)
		{
			string result = string.Empty;

			foreach (Match m in Regex.Matches(exp, Pattern_GetFNameAndFParam))
			{
				int offset = Convert.ToInt32(m.Groups["FParam"].Value);
				switch (m.Groups["FName"].Value)
				{
					case FUN_SrcRowNum:
						result = SrcRowNum(offset, sourceRowIndex);
						break;
					case FUN_RowNum:
						result = XlsRowNum(offset, xlsRowIndex);
						break;
					default:
						break;
				}
			}

			return result;
		}


		protected static string SrcRowNum(int offset, int sourceRowIndex)
		{
			return (sourceRowIndex + offset).ToString();
		}

		protected static string XlsRowNum(int offset, int xlsRowIndex)
		{
			return (xlsRowIndex + 1 + offset).ToString();
		}

		#endregion

		#region Helper

		#endregion
	}
}
