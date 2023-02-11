using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using HKH.Data.Configuration;

namespace HKH.Data.Odbc
{
	public class OdbcObjectBuilder : DataBaseObjectBuilder
	{
		#region Constructor

		public OdbcObjectBuilder(HKHConnectionString configuration)
			: base(configuration)
		{
		}

		public OdbcObjectBuilder(string connectionString)
			: base(connectionString)
		{
		}

		#endregion

		#region Base Class Overrides

		public override void BuildCommands(IDbDataAdapter adapter)
		{
			new OdbcCommandBuilder(adapter as OdbcDataAdapter);
		}

		protected override IDbConnection CreateConnection()
		{
			return new OdbcConnection(connectionString);
		}

		protected override IDbCommand CreateCommand()
		{
			return new OdbcCommand();
		}

		protected override IDbDataAdapter CreateDataAdapter()
		{
			return new OdbcDataAdapter();
		}

		public override IDbDataParameter GetParameter(string name)
		{
			OdbcParameter parameter = new OdbcParameter();
			parameter.ParameterName = GetFormattedParameterName(name);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name, string dbTypeName)
		{
			OdbcParameter parameter = GetParameter(name) as OdbcParameter;
			parameter.OdbcType = (OdbcType)Enum.Parse(typeof(OdbcType), dbTypeName, true);
			return parameter;
		}

		public override IDbDataParameter GetParameter(string name, int dbTypeCode)
		{
			OdbcParameter parameter = GetParameter(name) as OdbcParameter;
			parameter.OdbcType = (OdbcType)dbTypeCode;
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
