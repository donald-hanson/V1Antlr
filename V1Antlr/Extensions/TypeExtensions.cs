using System;

namespace V1Antlr.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSupportedPrimitiveType(this Type type)
        {
            return type.IsNumeric() || (type == typeof (bool) || type == typeof (DateTime));
        }

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
