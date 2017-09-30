using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.Common
{
    public static class Error
    {
        // Methods
        public static Exception ArgumentArrayHasTooManyElements(string paramName)
        {
            return new ArgumentException("ArgumentArrayHasTooManyElements:" + paramName);
        }

        public static Exception ArgumentNotIEnumerableGeneric(string paramName)
        {
            return new ArgumentException("ArgumentNotIEnumerableGeneric:" + paramName);
        }

        public static Exception ArgumentNotLambda(string paramName)
        {
            return new ArgumentException("ArgumentNotLambda:" + paramName);
        }

        public static Exception ArgumentNotSequence()
        {
            return new ArgumentException("ArgumentNotSequence");
        }

        public static Exception ArgumentNotValid(string paramName)
        {
            return new ArgumentException("ArgumentNotValid:" + paramName);
        }

        public static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static Exception ArgumentOutOfRange(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        public static Exception IncompatibleElementTypes()
        {
            return new ArgumentException("Incompatible ElementTypes");
        }

        public static Exception InvalidOperation()
        {
            return new InvalidOperationException();
        }

        public static Exception NoElements()
        {
            return new InvalidOperationException("NoElements");
        }

        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }
    }
}
