using System;
using System.Collections;
using Jade.Compiler;

namespace Jade.Expression
{
    public class BooleanUtil
    {
        public static bool convert(Object val)
        {
            if (val == null)
            {
                return false;
            }
            
            Type valueType = val.GetType();

            if (val is ICollection)
            {
                var t = ((ICollection) val).Count != 0;
                return ((ICollection) val).Count != 0;
            }
            else if (val is bool)
            {
                return (bool) val;
            }
            else if (IsType(valueType, typeof(int)))
            {
                return ((int[])val).Length != 0;
            }
            else if (IsType(valueType, typeof(double)))
            {
                return ((double[])val).Length != 0;
            }
            else if (IsType(valueType, typeof(float)))
            {
                return ((float[]) val).Length != 0;
            }
            else if (IsType(valueType, typeof(Object)))
            {
                return ((Object[]) val).Length != 0;
            }
            else if (val is int)
            {
                return ((int) val) != 0;
            }
            else if (val is double)
            {
                return Math.Abs(((double)val)) > 0.0000001;
            }
            else if (val is String)
            {
                return !StringUtils.isEmpty((String) val);
            }
            else
            {
                return true;
            }

        }

        private static bool IsType(Type valueType, Type elementType)
        {
            return valueType.GetElementType() == elementType;
        }
    }
}
