using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1Antlr.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNumeric(this Type type)
        {
            if (type == typeof(byte))
                return true;
            if (type == typeof (sbyte))
                return true;
            if (type == typeof(short))
                return true;
            if (type == typeof(ushort))
                return true;
            if (type == typeof (int))
                return true;
            if (type == typeof(uint))
                return true;
            if (type == typeof (long))
                return true;
            if (type == typeof(ulong))
                return true;

            if (type == typeof(decimal))
                return true;
            if (type == typeof(float))
                return true;
            if (type == typeof(double))
                return true;

            return false;
        }
    }
}
