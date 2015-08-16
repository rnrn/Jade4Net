using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jade.Jexl
{
    public class MapContext //: JexlContext
    {
        /**
         * The wrapped variable map.
         */
        protected Dictionary<String, Object> map;

        /**
         * Creates a MapContext on an automatically allocated underlying HashMap.
         */

        public MapContext() //:this(null)
        {
            ;
        }

        /**
         * Creates a MapContext wrapping an existing user provided map.
         * @param vars the variable map
         */

        public MapContext(Dictionary<String, Object> vars)
        {
            ;
            map = vars == null ? new Dictionary<String, Object>() : vars;
        }

        /** {@inheritDoc} */

        public bool has(String name)
        {
            return map.ContainsKey(name);
        }

        /** {@inheritDoc} */

        public Object get(String name)
        {
            return map[name];
        }

        /** {@inheritDoc} */

        public void set(String name, Object value)
        {
            map.Add(name, value);
        }
    }

}
