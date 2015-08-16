using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Jade.Jexl;
using Jade.Model;

namespace Jade.Expression
{
    public class ExpressionHandler
    {

        private static readonly int MAX_ENTRIES = 5000;

        private static JexlEngine jexl;

        static ExpressionHandler()
        {
            jexl = new JadeJexlEngine();
            jexl.setCache(MAX_ENTRIES);
        }

        public static Boolean? evaluateBooleanExpression(String expression, JadeModel model)
        {
            return BooleanUtil.convert(evaluateExpression(expression, model));
        }

        public static Object evaluateExpression(String expression, JadeModel model)
        {
            try
            {
                return expression;
                Lexer.Tokens.Expression e = jexl.createExpression(expression);
                return null;// e.evaluate(new MapContext(model));
            }
            catch (Exception)
            {
                throw;// new ExpressionException(expression, e);
            }
        }

        public static String evaluateStringExpression(String expression, JadeModel model)
        {
            Object result = evaluateExpression(expression, model);
            return result == null ? "" : result.ToString();
        }

        public static void setCache(bool cache)
        {
            jexl.setCache(cache ? MAX_ENTRIES : 0);
        }

        public static void clearCache()
        {
            jexl.clearCache();
        }
    }

    public class JexlEngine
    {
        public void setCache(int maxEntries)
        {
            //throw new NotImplementedException();
        }

        public void clearCache()
        {
            throw new NotImplementedException();
        }

        public Lexer.Tokens.Expression createExpression(string expression)
        {
            throw new NotImplementedException();
        }
    }
}
