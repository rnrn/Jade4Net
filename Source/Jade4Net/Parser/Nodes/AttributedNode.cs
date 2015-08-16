using System;
using System.Collections.Generic;
using Jade.Model;

namespace Jade.Parser.Nodes
{
    public abstract class AttributedNode : Node
    {

        protected Dictionary<String, Object> attributes = new Dictionary<String, Object>();
        protected Dictionary<String, List<Object>> preparedAttributeValues = new Dictionary<String, List<Object>>();
        protected bool inheritsAttributes = false;

        public void addAttribute(String key, Object value) {
            if ("attributes".Equals(key))
            {
                inheritsAttributes = true;
            }
            else
            {
                addAttribute(attributes, key, value);
            }
        }

        public String getAttribute(String key) {
            return (String)attributes[key];
        }

        public void addAttributes(Dictionary<String, Object> attributeMap) {
            foreach (String key in attributeMap.Keys)
            {
                addAttribute(key, attributeMap[key]);
            }
        }

        public Dictionary<String, Object> getAttributes() {
            return attributes;
        }

        protected Dictionary<String, Object> mergeInheritedAttributes(JadeModel model) {
            Dictionary<String, Object> mergedAttributes = this.attributes;

            if (inheritsAttributes)
            {
                Object o = model.get("attributes");
                if (o != null && o is Dictionary<String, Object>) {
                    Dictionary<String, Object> inheritedAttributes = (Dictionary<String, Object>)o;

                    foreach (var entry in inheritedAttributes)
                    {
                        addAttribute(mergedAttributes, (String)entry.Key, entry.Value);
                    }
                }
            }
            return mergedAttributes;
        }

        /**
         * Puts the specified key-value pair in the specified map. Provides special
         * processing in the case of the "class" attribute.
         */
        private void addAttribute(Dictionary<String, Object> map, String key, Object newValue) {
            if ("class".Equals(key) && attributes.ContainsKey(key))
            {
                String value1 = attributeValueToString(attributes[key]);
                String value2 = attributeValueToString(newValue);
                attributes.Add(key, value1 + " " + value2);

            }
            else
            {
                attributes.Add(key, newValue);
            }
        }

        private String attributeValueToString(Object value) {
            if (value is ExpressionString) {
                String expression = ((ExpressionString)value).getValue();
                return "#{" + expression + "}";
            }
            return value.ToString();
        }

        public override object Clone()// throws CloneNotSupportedException
        {
            AttributedNode clone = (AttributedNode)base.Clone();

            // shallow copy
            if (this.attributes != null)
            {
                clone.attributes = new Dictionary<String, Object>(this.attributes);
            }

            // clear prepared attribute values, will be rebuilt on execute
            clone.preparedAttributeValues = new Dictionary<String, List<Object>>();

            return clone;
        }

    }
}
