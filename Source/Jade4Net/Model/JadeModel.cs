using System;
using System.Collections.Generic;
using Jade.Filters;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Util;

namespace Jade.Model
{
    public class JadeModel : Dictionary<String, Object>
    {
        private static readonly String LOCALS = "locals";

        private LinkedList<Dictionary<String, Object>> scopes = new LinkedList<Dictionary<String, Object>>();
        private Dictionary<String, MixinNode> mixins = new Dictionary<String, MixinNode>();
        private Dictionary<String, Filter> filter = new Dictionary<String, Filter>();

        public JadeModel(Dictionary<String, Object> defaults)
        {
            Dictionary<String, Object> rootScope = new Dictionary<String, Object>();
            scopes.AddLast(rootScope);

            if (defaults != null)
            {
                putAll(defaults);
            }

            put(LOCALS, this);
        }

        public void pushScope()
        {
            Dictionary<String, Object> scope = new Dictionary<String, Object>();
            scopes.AddLast(scope);
        }

        public void popScope()
        {
            scopes.RemoveLast();
        }

        public void setMixin(String name, MixinNode node)
        {
            mixins.Add(name, node);
        }

        public MixinNode getMixin(String name)
        {
            return mixins[name];
        }


        public void Clear()
        {
            base.Clear();
            scopes.Clear();
            scopes.AddLast(new Dictionary<String, Object>());
        }


        public bool ContainsKey(String key)
        {
            foreach (var scope in scopes.Reverse())
            {
                if (scope.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }


        public bool ContainsValue(Object value)
        {
            foreach (var scope in scopes.Reverse())
            {
                if (scope.ContainsValue(value))
                {
                    return true;
                }
            }
            return false;
        }


        public Dictionary<String, Object> entrySet()
        {
            var map = new Dictionary<String, Object>();
            foreach (var key in keySet())
            {
                map.Add(key, get(key));
            }
            return map;
        }


        // adds the object to the highest scope
        public Object get(Object key)
        {
            foreach (var scope in scopes.Reverse())
            {
                if (scope.ContainsKey((string) key))
                {
                    return scope[(string) key];
                }
            }
            return null;
        }


        public bool isEmpty()
        {
            return keySet().Count < 1;
        }


        // returns a set of unique keys
        public HashSet<String> keySet()
        {
            HashSet<String> keys = new HashSet<String>();
            foreach (var scope in scopes.Reverse())
                foreach (var key in scope.Keys)
                    keys.Add(key);
            return keys;
        }


        // adds the object to the current scope
        public Object put(String key, Object value)
        {
            Object currentValue = get(key);
            scopes.Last.Value.Add(key, value);
            return currentValue;
        }


        // addes all map entries to the current scope map
        public void putAll(Dictionary<String, Object> m)
        {
            foreach (var o in m)
            {
                scopes.Last.Value.Add(o.Key, o.Value);
            }
        }


        // removes the scopes first object with the given key
        public Object remove(Object key)
        {
            foreach (var scope in scopes.Reverse())
            {
                if (scope.ContainsKey((string) key))
                {
                    var val = scope[(string) key];
                    scope.Remove((string) key);
                    return val;
                }
            }
            return null;
        }


        // returns the size of all unique keys
        public int size()
        {
            return keySet().Count;
        }


        // returns the size of all unique keys
        public ICollection<Object> values()
        {
            List<Object> values = new List<Object>();
            foreach (String key in keySet())
            {
                values.Add(get(key));
            }
            return values;
        }

        public Filter getFilter(String name)
        {
            return filter[name];
        }

        public void addFilter(String name, Filter filter)
        {
            this.filter[name] = filter;
        }
    }
}