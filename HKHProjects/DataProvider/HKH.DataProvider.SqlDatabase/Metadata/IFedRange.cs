using System;

namespace HKH.Data.SqlDatabase.Metadata
{
    public interface IFedRange
    {
        FedKey High { get;  }
        FedKey Low { get; }

        T GetLow<T>();
        T GetHigh<T>();

        string ToString();

        FedKey GetMidRange();
    }
}
