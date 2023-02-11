using System;
using System.Collections.Generic;
using System.Data;

namespace HKH.Data
{
	[Serializable ]
	public class SqlInfo
	{
		public string Text { get; set; }
		public CommandType Type { get; set; }
		public IDbDataParameter[] Parameters { get; set; }
	}

	public class SqlInfoList : List<SqlInfo>
	{
		public void Add(string strSql, CommandType type, params IDbDataParameter[] parameters)
		{
			Add(new SqlInfo { Text =strSql , Type =type , Parameters =parameters  });
		}
	}
}
