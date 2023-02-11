namespace System.Data//HKH.Common
{
	public class SafeDataReader : ISafeDataReader
	{
		protected IDataReader innerReader = null;

		public SafeDataReader(IDataReader reader)
		{
			this.innerReader = reader;
		}

		#region ISafeDataReader Members

		public IDataReader DataReader { get { return innerReader; } }

		public virtual bool GetBoolean(int i, bool defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetBoolean(i);
		}

		public virtual byte GetByte(int i, byte defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetByte(i);
		}

		public virtual char GetChar(int i, char defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetChar(i);
		}

		public virtual DateTime GetDateTime(int i, DateTime defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetDateTime(i);
		}

		public virtual decimal GetDecimal(int i, decimal defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetDecimal(i);
		}

		public virtual double GetDouble(int i, double defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetDouble(i);
		}

		public virtual float GetFloat(int i, float defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetFloat(i);
		}

		public virtual Guid GetGuid(int i, Guid defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetGuid(i);
		}

		public virtual short GetInt16(int i, short defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetInt16(i);
		}

		public virtual int GetInt32(int i, int defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetInt32(i);
		}

		public virtual long GetInt64(int i, long defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetInt64(i);
		}

		public virtual string GetString(int i, string defaultValue)
		{
			return innerReader.IsDBNull(i) ? defaultValue : innerReader.GetString(i);
		}

		public bool? GetNullableBoolean(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;
			return innerReader.GetBoolean(i);
		}

		public byte? GetNullableByte(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;
			return innerReader.GetByte(i);
		}

		public char? GetNullableChar(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetChar(i);
		}

		public DateTime? GetNullableDateTime(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetDateTime(i);
		}

		public decimal? GetNullableDecimal(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetDecimal(i);
		}

		public double? GetNullableDouble(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetDouble(i);
		}

		public float? GetNullableFloat(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetFloat(i);
		}

		public Guid? GetNullableGuid(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetGuid(i);
		}

		public short? GetNullableInt16(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetInt16(i);
		}

		public int? GetNullableInt32(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetInt32(i);
		}

		public long? GetNullableInt64(int i)
		{
			if (innerReader.IsDBNull(i))
				return null;

			return innerReader.GetInt64(i);
		}

		//by field name
		public bool GetBoolean(string fieldName)
		{
			return GetBoolean(GetOrdinal(fieldName));
		}

		public byte GetByte(string fieldName)
		{
			return GetByte(GetOrdinal(fieldName));
		}

		public char GetChar(string fieldName)
		{
			return GetChar(GetOrdinal(fieldName));
		}

		public DateTime GetDateTime(string fieldName)
		{
			return GetDateTime(GetOrdinal(fieldName));
		}

		public decimal GetDecimal(string fieldName)
		{
			return GetDecimal(GetOrdinal(fieldName));
		}

		public double GetDouble(string fieldName)
		{
			return GetDouble(GetOrdinal(fieldName));
		}

		public float GetFloat(string fieldName)
		{
			return GetFloat(GetOrdinal(fieldName));
		}

		public Guid GetGuid(string fieldName)
		{
			return GetGuid(GetOrdinal(fieldName));
		}

		public short GetInt16(string fieldName)
		{
			return GetInt16(GetOrdinal(fieldName));
		}

		public int GetInt32(string fieldName)
		{
			return GetInt32(GetOrdinal(fieldName));
		}

		public long GetInt64(string fieldName)
		{
			return GetInt64(GetOrdinal(fieldName));
		}

		public string GetString(string fieldName)
		{
			return GetString(GetOrdinal(fieldName));
		}

		public object GetValue(string fieldName)
		{
			return innerReader.GetValue(GetOrdinal(fieldName));
		}

		//by field name with default value
		public bool GetBoolean(string fieldName, bool defaultValue)
		{
			return GetBoolean(GetOrdinal(fieldName), defaultValue);
		}

		public byte GetByte(string fieldName, byte defaultValue)
		{
			return GetByte(GetOrdinal(fieldName), defaultValue);
		}

		public char GetChar(string fieldName, char defaultValue)
		{
			return GetChar(GetOrdinal(fieldName), defaultValue);
		}

		public DateTime GetDateTime(string fieldName, DateTime defaultValue)
		{
			return GetDateTime(GetOrdinal(fieldName), defaultValue);
		}

		public decimal GetDecimal(string fieldName, decimal defaultValue)
		{
			return GetDecimal(GetOrdinal(fieldName), defaultValue);
		}

		public double GetDouble(string fieldName, double defaultValue)
		{
			return GetDouble(GetOrdinal(fieldName), defaultValue);
		}

		public float GetFloat(string fieldName, float defaultValue)
		{
			return GetFloat(GetOrdinal(fieldName), defaultValue);
		}

		public Guid GetGuid(string fieldName, Guid defaultValue)
		{
			return GetGuid(GetOrdinal(fieldName), defaultValue);
		}

		public short GetInt16(string fieldName, short defaultValue)
		{
			return GetInt16(GetOrdinal(fieldName), defaultValue);
		}

		public int GetInt32(string fieldName, int defaultValue)
		{
			return GetInt32(GetOrdinal(fieldName), defaultValue);
		}

		public long GetInt64(string fieldName, long defaultValue)
		{
			return GetInt64(GetOrdinal(fieldName), defaultValue);
		}

		public string GetString(string fieldName, string defaultValue)
		{
			return GetString(GetOrdinal(fieldName), defaultValue);
		}

		//nullable
		public bool? GetNullableBoolean(string fieldName)
		{
			return GetNullableBoolean(GetOrdinal(fieldName));
		}

		public byte? GetNullableByte(string fieldName)
		{
			return GetNullableByte(GetOrdinal(fieldName));
		}

		public char? GetNullableChar(string fieldName)
		{
			return GetNullableChar(GetOrdinal(fieldName));
		}

		public DateTime? GetNullableDateTime(string fieldName)
		{
			return GetNullableDateTime(GetOrdinal(fieldName));
		}

		public decimal? GetNullableDecimal(string fieldName)
		{
			return GetNullableDecimal(GetOrdinal(fieldName));
		}

		public double? GetNullableDouble(string fieldName)
		{
			return GetNullableDouble(GetOrdinal(fieldName));
		}

		public float? GetNullableFloat(string fieldName)
		{
			return GetNullableFloat(GetOrdinal(fieldName));
		}

		public Guid? GetNullableGuid(string fieldName)
		{
			return GetNullableGuid(GetOrdinal(fieldName));
		}

		public short? GetNullableInt16(string fieldName)
		{
			return GetNullableInt16(GetOrdinal(fieldName));
		}

		public int? GetNullableInt32(string fieldName)
		{
			return GetNullableInt32(GetOrdinal(fieldName));
		}

		public long? GetNullableInt64(string fieldName)
		{
			return GetNullableInt64(GetOrdinal(fieldName));
		}

		public bool IsDBNull(string fieldName)
		{
			return IsDBNull(GetOrdinal(fieldName));
		}

		#endregion

		#region IDataReader Members

		public void Close()
		{
			innerReader.Close();
		}

		public int Depth
		{
			get { return innerReader.Depth; }
		}

		public DataTable GetSchemaTable()
		{
			return innerReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return innerReader.IsClosed; }
		}

		public bool NextResult()
		{
			return innerReader.NextResult();
		}

		public bool Read()
		{
			return innerReader.Read();
		}

		public int RecordsAffected
		{
			get { return innerReader.RecordsAffected; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			innerReader.Dispose();
		}

		#endregion

		#region IDataRecord Members

		public int FieldCount
		{
			get { return innerReader.FieldCount; }
		}

		public virtual bool GetBoolean(int i)
		{
			return GetBoolean(i, false);
		}

		public virtual byte GetByte(int i)
		{
			return GetByte(i, 0);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public virtual char GetChar(int i)
		{
			return GetChar(i, '\0');
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public IDataReader GetData(int i)
		{
			return innerReader.GetData(i);
		}

		public string GetDataTypeName(int i)
		{
			return innerReader.GetDataTypeName(i);
		}

		public virtual DateTime GetDateTime(int i)
		{
			return GetDateTime(i, DateTime.MaxValue);
		}

		public virtual decimal GetDecimal(int i)
		{
			return GetDecimal(i, decimal.Zero);
		}

		public virtual double GetDouble(int i)
		{
			return GetDouble(i, 0.0);
		}

		public Type GetFieldType(int i)
		{
			return innerReader.GetFieldType(i);
		}

		public virtual float GetFloat(int i)
		{
			return GetFloat(i, 0.0f);
		}

		public virtual Guid GetGuid(int i)
		{
			return GetGuid(i, Guid.Empty);
		}

		public virtual short GetInt16(int i)
		{
			return GetInt16(i, 0);
		}

		public virtual int GetInt32(int i)
		{
			return GetInt32(i, 0);
		}

		public virtual long GetInt64(int i)
		{
			return GetInt64(i, 0);
		}

		public string GetName(int i)
		{
			return innerReader.GetName(i);
		}

		public int GetOrdinal(string name)
		{
			return innerReader.GetOrdinal(name);
		}

		public virtual string GetString(int i)
		{
			return GetString(i, string.Empty);
		}

		public object GetValue(int i)
		{
			return innerReader.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return innerReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return innerReader.IsDBNull(i);
		}

		public object this[string name]
		{
			get { return innerReader[name]; }
		}

		public object this[int i]
		{
			get { return innerReader[i]; }
		}

		#endregion
	}
}
