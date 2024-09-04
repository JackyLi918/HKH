using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using HKH.Data.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace HKH.Data.Oracle
{
    public class OracleObjectBuilder : DataBaseObjectBuilder
    {
        #region Constructor

        public OracleObjectBuilder(HKHConnectionString configuration)
            : base(configuration)
        {
        }

        public OracleObjectBuilder(string connectionString)
            : base(connectionString)
        {
        }

        #endregion

        #region Base Class Overrides

        public override void BuildCommands(IDbDataAdapter adapter)
        {
            new OracleCommandBuilder(adapter as OracleDataAdapter);
        }

        protected override IDbConnection CreateConnection()
        {
            return new OracleConnection(connectionString);
        }

        protected override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        protected override IDbDataAdapter CreateDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public override IDbDataParameter GetParameter(string name)
        {
            var parameter = new OracleParameter();
            parameter.ParameterName = GetFormattedParameterName(name);
            return parameter;
        }

        public override IDbDataParameter GetParameter(string name, string dbTypeName)
        {
            var parameter = GetParameter(name) as OracleParameter;
            parameter.OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), dbTypeName, true);
            return parameter;
        }

        public override IDbDataParameter GetParameter(string name, int dbTypeCode)
        {
            var parameter = GetParameter(name) as OracleParameter;
            parameter.OracleDbType = (OracleDbType)dbTypeCode;
            return parameter;
        }

        private string partten = @"(?<param>:+[A-Za-z0-9_]+)";
        public override IDbDataParameter[] GetParameters(string strSql, params object[] values)
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            Regex regParam = new Regex(partten, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            foreach (Match paramMatch in regParam.Matches(strSql))
            {
                //oracle is IgnoreCase, it will auto upcase while running.
                if (parameters.FirstOrDefault(param => param.ParameterName.Equals(paramMatch.Value, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    parameters.Add(GetParameter(paramMatch.Value));
                }
            }

            if (parameters.Count != values.Length)
                throw new ArgumentException("The count of parameter does not equal to the count of value.");

            for (int i = 0; i < parameters.Count; i++)
                parameters[i].Value = values[i];

            return parameters.ToArray<IDbDataParameter>();
        }

        protected override string GetFormattedParameterName(string name)
        {
            return string.Format("@{0}", name.TrimStart(':'));
        }

        #endregion

    }
}
