using System;
using System.Collections.Generic;

namespace Jade.Filters
{
    public class PlainFilter : Filter
    {
        public String convert(String source, Dictionary<String, Object> attributes, Dictionary<String, Object> model)
        {
            return source;
        }
    }
}
