using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jade4Net.Jexl.introspection
{
    public interface Uberspect
    {
        /** Sets the class loader to use when getting a constructor with
         * a class name parameter.
         * @param loader the class loader
         */
        void setClassLoader(ClassLoader loader);

        /**
         * Returns a class constructor.
         * @param ctorHandle a class or class name
         * @param args constructor arguments
         * @param info contextual information
         * @return a {@link Constructor}
         */
        //@Deprecated
        //Constructor<?> getConstructor(Object ctorHandle, Object[] args, JexlInfo info);

        /**
         * Returns a class constructor wrapped in a JexlMethod.
         * @param ctorHandle a class or class name
         * @param args constructor arguments
         * @param info contextual information
         * @return a {@link Constructor}
         * @since 2.1
         */
        JexlMethod getConstructorMethod(Object ctorHandle, Object[] args, JexlInfo info);

        /**
         * Returns a JexlMethod.
         * @param obj the object
         * @param method the method name
         * @param args method arguments
         * @param info contextual information
         * @return a {@link JexlMethod}
         */
        JexlMethod getMethod(Object obj, String method, Object[] args, JexlInfo info);

        /**
         * Property getter.
         * <p>Returns JexlPropertyGet appropos for ${bar.woogie}.
         * @param obj the object to get the property from
         * @param identifier property name
         * @param info contextual information
         * @return a {@link JexlPropertyGet}
         */
        JexlPropertyGet getPropertyGet(Object obj, Object identifier, JexlInfo info);

        /**
         * Property setter.
         * <p>returns JelPropertySet appropos for ${foo.bar = "geir"}</p>.
         * @param obj the object to get the property from.
         * @param identifier property name
         * @param arg value to set
         * @param info contextual information
         * @return a {@link JexlPropertySet}.
         */
        JexlPropertySet getPropertySet(Object obj, Object identifier, Object arg, JexlInfo info);

        /**
         * Gets an iterator from an object.
         * @param obj to get the iterator for
         * @param info contextual information
         * @return an iterator over obj
         */
        Iterator<?> getIterator(Object obj, JexlInfo info);

    }
}
