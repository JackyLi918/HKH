using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Data.SqlDatabase.Metadata
{
    /// <summary>
    /// Represents a federation range
    /// </summary>

    public class FedRange : IFedRange
    {
        public FedRange(int low, int high)
        {
            this.Low = new FedKey(low);
            this.High = new FedKey(high);
        }

        public FedRange(long low, long high)
        {
            this.Low = new FedKey(low);
            this.High = new FedKey(high);
        }

        public FedRange(Guid low, Guid high)
        {
            this.Low = new FedKey(low);
            this.High = new FedKey(high);
        }

        public FedRange(byte[] low, byte[] high)
        {
            this.Low = new FedKey(low);
            this.High = new FedKey(high);
        }

        public FedKeyType Type { get { return Low.Type; } }

        public FedKey Low { get; private set; }
        public FedKey High { get; private set; }

        public T GetLow<T>()
        {
            return (T)Low.GetValue<T>();
        }

        public T GetHigh<T>()
        {
            return (T)High.GetValue<T>();
        }

        public override string ToString()
        {
            return string.Format("From {0} to {1}", Low.ToString(), High.ToString());
        }

        public FedKey GetMidRange()
        {
            switch (Type)
            {
                case FedKeyType.fedkeytypeBigInt:
                    return new FedKey((GetHigh<long>() - GetLow<long>()) / 2);
                case FedKeyType.fedkeytypeInt:
                    return new FedKey((GetHigh<int>() - GetLow<int>()) / 2);
                default:
                    return null;
            }
        }
    }
}
