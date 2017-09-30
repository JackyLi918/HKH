using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Data.SqlDatabase.Metadata
{
	public enum FedKeyType
	{
		fedkeytypeInt,
		fedkeytypeBigInt,
		fedkeytypeGuid,
		fedkeytypeVarbin
	}

	/// <summary>
	/// Represents a federation key
	/// </summary>
	public class FedKey : IFedKey, IComparable
	{
		public FedKey(int value)
		{
			this.Value = value;
			this.Type = FedKeyType.fedkeytypeInt;
		}

		public FedKey(long value)
		{
			this.Value = value;
			this.Type = FedKeyType.fedkeytypeBigInt;
		}

		public FedKey(Guid value)
		{
			this.Value = value;
			this.Type = FedKeyType.fedkeytypeGuid;
		}

		public FedKey(byte[] value)
		{
			this.Value = value;
			this.Type = FedKeyType.fedkeytypeVarbin;
		}

		public FedKeyType Type { get; private set; }

		public object Value { get; private set; }

		private static readonly Dictionary<Type, FedKeyType> _typeMap = new Dictionary<Type, FedKeyType>()
        {
            { typeof(int), FedKeyType.fedkeytypeInt },
            { typeof(long), FedKeyType.fedkeytypeBigInt },
            { typeof(Guid), FedKeyType.fedkeytypeGuid},
            { typeof(byte[]), FedKeyType.fedkeytypeVarbin },
        };

		public T GetValue<T>()
		{
			if (!_typeMap.ContainsKey(typeof(T)))
				throw new ArgumentException("Type {0} is not a supported federation key type", typeof(T).Name);

			var requestedType = _typeMap[typeof(T)];
			if (requestedType != Type)
				throw new ArgumentException(String.Format("This federation key type {0} not of requested type {1}",
					Type, requestedType));

			return (T)Value;
		}

		public override string ToString()
		{
			if (null != Value)
			{
				switch (Type)
				{
					case FedKeyType.fedkeytypeInt:
					case FedKeyType.fedkeytypeBigInt:
					case FedKeyType.fedkeytypeGuid:
					default:
						return Convert.ToString(Value);
					case FedKeyType.fedkeytypeVarbin:
						byte[] buf = (byte[])Value;
						if (buf.Length > 0)
						{
							string strHex = buf.ToHexEncodedString();
							if (buf.Length > 10)
								return strHex.Substring(0, 10 * 2) + "...";
							else
								return strHex;
						}
						else
							return "0x00";
				}
			}
			else
				return "Max";
		}

		public string ToFormattedString(bool bBareFormat = false)
		{
			if (null != Value)
			{
				switch (Type)
				{
					case FedKeyType.fedkeytypeInt:
					case FedKeyType.fedkeytypeBigInt:
						return this.ToString();
					case FedKeyType.fedkeytypeVarbin:
						return ((byte[])Value).ToHexEncodedString();
					case FedKeyType.fedkeytypeGuid:
						if (bBareFormat)
							return this.ToString();
						else
							return "'" + this.ToString() + "'";
				}
			}

			return null;
		}

		public static FedKey CreateFedKey(object value)
		{
			if (value == null)
				return null;

			Type type = value.GetType();
			if (!_typeMap.ContainsKey(type))
				throw new ArgumentException("Type {0} is not a supported federation key type", type.Name);

			FedKeyType fedkeytype = _typeMap[type];

			switch (fedkeytype)
			{
				case FedKeyType.fedkeytypeInt:
					return new FedKey(Convert.ToInt32(value));
				case FedKeyType.fedkeytypeBigInt:
					return new FedKey(Convert.ToInt64(value));
				case FedKeyType.fedkeytypeGuid:
					return new FedKey((Guid)value);
				case FedKeyType.fedkeytypeVarbin:
					return new FedKey((System.Byte[])value);
				default:
					return null;
			}
		}

		public static FedKey CreateFedKey(FedKeyType fedkeytype, object value, Type type = null)
		{
			if (null != type && !_typeMap.ContainsKey(type))
				throw new ArgumentException("Type {0} is not a supported federation key type", type.Name);

			if (value.GetType() == typeof(string))
				return CreateFedKey(fedkeytype, (string)value);

			switch (fedkeytype)
			{
				case FedKeyType.fedkeytypeInt:
					return new FedKey(Convert.ToInt32(value));
				case FedKeyType.fedkeytypeBigInt:
					return new FedKey(Convert.ToInt64(value));
				case FedKeyType.fedkeytypeGuid:
					return new FedKey((Guid)value);
				case FedKeyType.fedkeytypeVarbin:
					return new FedKey((System.Byte[])value);
				default:
					return null;
			}
		}

		public static FedKey CreateFedKey(FedKeyType fedkeytype, string value)
		{
			switch (fedkeytype)
			{
				case FedKeyType.fedkeytypeInt:
					int ivalue;
					if (int.TryParse(value, out ivalue))
						return new FedKey(ivalue);
					break;
				case FedKeyType.fedkeytypeBigInt:
					long lvalue;
					if (long.TryParse(value, out lvalue))
						return new FedKey(lvalue);
					break;
				case FedKeyType.fedkeytypeGuid:
					Guid giudvalue;
					if (Guid.TryParse(value, out giudvalue))
						return new FedKey(giudvalue);
					break;
				case FedKeyType.fedkeytypeVarbin:
					return new FedKey(value.ToByteArray());
				default:
					return null;
			}

			return null;
		}

		public int CompareTo(object obj)
		{
			if (obj is FedKey)
			{
				var compare = obj as FedKey;

				switch (Type)
				{
					case FedKeyType.fedkeytypeGuid:
						var guidA = GetValue<Guid>();
						var guidB = compare.GetValue<Guid>();
						return guidA.CompareTo(guidB);
					case FedKeyType.fedkeytypeVarbin:
						return 0;
					case FedKeyType.fedkeytypeBigInt:
						var longA = GetValue<long>();
						var longB = compare.GetValue<long>();
						return longA.CompareTo(longB);
					case FedKeyType.fedkeytypeInt:
						var intA = GetValue<int>();
						var intB = compare.GetValue<int>();
						return intA.CompareTo(intB);
					default:
						return 0;
				}
			}
			else
			{
				throw new ArgumentException("Object is not a Fedkey");
			}
		}
	}
}
