using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Exchange.Common
{
    public static class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNumeric(object val)
        {
            return (val is Int16 || val is UInt16 ||
                    val is Int32 || val is UInt32 ||
                    val is Int64 || val is UInt64 ||
                    val is SByte || val is Byte ||
                    val is Single || val is Double || val is Decimal);
        }

        public static bool IsFloat(object val)
        {
            return (val is Single || val is Double || val is Decimal);
        }
    }
}
