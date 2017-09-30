using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Data.SqlDatabase.Metadata
{
    public interface IFedKey
    {
        object Value { get; }
        
        T GetValue<T>();

        string ToString();
    }
}
