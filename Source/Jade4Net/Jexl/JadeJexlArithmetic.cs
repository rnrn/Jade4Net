using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Jade.Jexl
{
    public class JadeJexlArithmetic //: JexlArithmetic
    {
        public JadeJexlArithmetic(bool lenient) //: base(lenient)
        {
            ;
        }

        /**
         * using the original implementation
         * added check for empty lists
         * defaulting to "true"
         */

        public bool toBoolean(Object val)
        {
            if (val == null)
            {
                //controlNullOperand();
                return false;
            }
            else if (val is Boolean)
            {
                return ((Boolean) val);
            }
            else if (val is int)
            {
                double number = Double.Parse(val.ToString());
                return !Double.IsNaN(number) && Math.Abs(number) > 0.000000000001;
            }
            else if (val is String)
            {
                String strval = val.ToString();
                return strval.Length > 0 && !"false".Equals(strval);
            }
            else if (val is ICollection)
            {
                return (val as ICollection).Count > 0;
            }

            return true;
        }
    }
}
