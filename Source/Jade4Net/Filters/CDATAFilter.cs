using System.Collections.Generic;

namespace Jade.Filters
{
    public class CDATAFilter : Filter
    {
        public string convert(string source, Dictionary<string, object> attributes, Dictionary<string, object> model)
        {
            return "<![CDATA[\n" + source + "\n]]>";
        }
    }
}