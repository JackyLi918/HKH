using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace System.Data//HKH.Common
{
	public interface ISafeDataReader : IDataReader
	{
		IDataReader DataReader { get; }

		bool GetBoolean(int i, bool defaultValue);

		byte GetByte(int i, byte defaultValue);

		char GetChar(int i, char defaultValue);

		DateTime GetDateTime(int i, DateTime defaultValue);

		decimal GetDecimal(int i, decimal defaultValue);

		double GetDouble(int i, double defaultValue);

		float GetFloat(int i, float defaultValue);

		Guid GetGuid(int i, Guid defaultValue);

		short GetInt16(int i, short defaultValue);

		int GetInt32(int i, int defaultValue);

		long GetInt64(int i, long defaultValue);

		string GetString(int i, string defaultValue);

		//nullable value
		bool? GetNullableBoolean(int i);

		byte? GetNullableByte(int i);

		char? GetNullableChar(int i);

		DateTime? GetNullableDateTime(int i);

		decimal? GetNullableDecimal(int i);

		double? GetNullableDouble(int i);

		float? GetNullableFloat(int i);

		Guid? GetNullableGuid(int i);

		short? GetNullableInt16(int i);

		int? GetNullableInt32(int i);

		long? GetNullableInt64(int i);

		//by field name
		bool GetBoolean(string fName);

		byte GetByte(string fName);

		char GetChar(string fName);

		DateTime GetDateTime(string fName);

		decimal GetDecimal(string fName);

		double GetDouble(string fName);

		float GetFloat(string fName);

		Guid GetGuid(string fName);

		short GetInt16(string fName);

		int GetInt32(string fName);

		long GetInt64(string fName);

		string GetString(string fName);

		object GetValue(string fieldName);

		//by field name with default value
		bool GetBoolean(string fName, bool defaultValue);

		byte GetByte(string fName, byte defaultValue);

		char GetChar(string fName, char defaultValue);

		DateTime GetDateTime(string fName, DateTime defaultValue);

		decimal GetDecimal(string fName, decimal defaultValue);

		double GetDouble(string fName, double defaultValue);

		float GetFloat(string fName, float defaultValue);

		Guid GetGuid(string fName, Guid defaultValue);

		short GetInt16(string fName, short defaultValue);

		int GetInt32(string fName, int defaultValue);

		long GetInt64(string fName, long defaultValue);

		string GetString(string fName, string defaultValue);

		//nullable
		bool? GetNullableBoolean(string fName);

		byte? GetNullableByte(string fName);

		char? GetNullableChar(string fName);

		DateTime? GetNullableDateTime(string fName);

		decimal? GetNullableDecimal(string fName);

		double? GetNullableDouble(string fName);

		float? GetNullableFloat(string fName);

		Guid? GetNullableGuid(string fName);

		short? GetNullableInt16(string fName);

		int? GetNullableInt32(string fName);

		long? GetNullableInt64(string fName);

		//isdbnull
		bool IsDBNull(string fieldName);
	}
}
