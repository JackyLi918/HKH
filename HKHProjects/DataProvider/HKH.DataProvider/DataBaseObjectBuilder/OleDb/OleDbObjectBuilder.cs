using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HKH.Data;
using HKH.Data.Configuration;

namespace HKH.Data.OleDb
{
	public class OleDbObjectBuilder : DataBaseObjectBuilder
	{
		#region Constructor

		public OleDbObjectBuilder(HKHConnectionStringElement configuration)
			: base(configuration)
		{
		}

		public OleDbObjectBuilder(string connectionString)
			: base(connectionString)
		{
		}

		#endregion

		#region Base Class Overrides

		public override void BuildCommands(IDbDataAdapter adapter)
		{
			new OleDbCommandBuilder(adapter as OleDbDataAdapter);
		}

		protected override IDbConnection CreateConnection()
		{
			return new OleDbConnection(connectionString);
		}

		protected override IDbCommand CreateCommand()
		{
			return new OleDbCommand();
		}

		protected override IDbDataAdapter CreateDataAdapter()
		{
			return new OleDbDataAdapter();
		}

		public override IDbDataParameter GetParameter(string name)
		{
			OleDbParameter parameter= new OleDbParameter();
			parameter.ParameterName = GetFormattedParameterName(name);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name,string dbTypeName)
		{
			OleDbParameter parameter = GetParameter(name) as OleDbParameter;
			parameter.OleDbType = (OleDbType)Enum.Parse(typeof(OleDbType), dbTypeName, true);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name,int dbTypeCode)
		{
			OleDbParameter parameter = GetParameter(name) as OleDbParameter;
			parameter.OleDbType = (OleDbType)dbTypeCode;
			return parameter;
		}

		private string partten = @"(?<param>\?+)";
		public override IDbDataParameter[] GetParameters(string strSql, params object[] values)
		{
			List<IDbDataParameter> parameters = new List<IDbDataParameter>();
			Regex regParam = new Regex(partten, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
			foreach (Match paramMatch in regParam.Matches(strSql))
			{
				//ignore error match
				if (paramMatch.Value.StartsWith("??"))
					continue;

				parameters.Add(GetParameter(string.Format("@param{0}", parameters.Count + 1)));
			}

			if (parameters.Count != values.Length)
				throw new ArgumentException("The count of parameter does not equal to the count of value.");

			for (int i = 0; i < parameters.Count; i++)
				parameters[i].Value = values[i];

			return parameters.ToArray<IDbDataParameter>();
		}

		protected override string GetFormattedParameterName(string name)
		{
			return string.Format("@{0}", name.TrimStart('@'));
		}

		#endregion

	}
}
