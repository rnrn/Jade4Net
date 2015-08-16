using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jade.Jexl
{
    public class JadeIntrospect //: UberspectImpl
    {

        public JadeIntrospect(Log runtimeLogger) //: base(runtimeLogger)
        {
            ;
        }

        /**
         * Overwriting method to replace "getGetExecutor" call with "getJadeGetExecutor"
         */

        //public JexlPropertyGet getPropertyGet(Object obj, Object identifier, JexlInfo info) {
        //    JexlPropertyGet get = getJadeGetExecutor(obj, identifier);
        //    if (get == null && obj != null && identifier != null)
        //    {
        //        get = getIndexedGet(obj, identifier.toString());
        //        if (get == null)
        //        {
        //            Field field = getField(obj, identifier.toString(), info);
        //            if (field != null)
        //            {
        //                return new FieldPropertyGet(field);
        //            }
        //        }
        //    }
        //    return get;
        //}

        /**
         * Identical to getGetExecutor, but does check for map first. Mainly to avoid problems with 'class' properties.
         */
        //public AbstractExecutor.Get getJadeGetExecutor(Object obj, Object identifier) {
        //    final Class<?> claz = obj.getClass();
        //    final String property = toString(identifier);
        //    AbstractExecutor.Get executor;
        //    // let's see if we are a map...
        //    executor = new MapGetExecutor(this, claz, identifier);
        //    if (executor.isAlive())
        //    {
        //        return executor;
        //    }
        //    // first try for a getFoo() type of property (also getfoo() )
        //    if (property != null)
        //    {
        //        executor = new PropertyGetExecutor(this, claz, property);
        //        if (executor.isAlive())
        //        {
        //            return executor;
        //        }
        //        // }
        //        // look for boolean isFoo()
        //        // if (property != null) {
        //        executor = new BooleanGetExecutor(this, claz, property);
        //        if (executor.isAlive())
        //        {
        //            return executor;
        //        }
        //    }
        //    // let's see if we can convert the identifier to an int,
        //    // if obj is an array or a list, we can still do something
        //    Integer index = toInteger(identifier);
        //    if (index != null)
        //    {
        //        executor = new ListGetExecutor(this, claz, index);
        //        if (executor.isAlive())
        //        {
        //            return executor;
        //        }
        //    }
        //    // if that didn't work, look for set("foo")
        //    executor = new DuckGetExecutor(this, claz, identifier);
        //    if (executor.isAlive())
        //    {
        //        return executor;
        //    }
        //    // if that didn't work, look for set("foo")
        //    executor = new DuckGetExecutor(this, claz, property);
        //    if (executor.isAlive())
        //    {
        //        return executor;
        //    }
        //    return null;
        //}
    }

    public class Log
    {
    }
}
