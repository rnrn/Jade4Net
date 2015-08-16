using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jade.Expression;

namespace Jade.Jexl
{

    public class JadeJexlEngine : JexlEngine
    {

        /*
         * using a semi strict interpreter and non strict arithmetic
         */
        public JadeJexlEngine() //: base(new JadeIntrospect(null), new JadeJexlArithmetic(true), null, null)
        {
            ;
            //setStrict(false);
        }

        //protected override Interpreter createInterpreter(JexlContext context, boolean strictFlag, boolean silentFlag) {
        //    // always use strict
        //    strictFlag = true;
        //    return new JadeJexlInterpreter(this, context == null ? EMPTY_CONTEXT : context, strictFlag, silentFlag);
        //}
    }

}
