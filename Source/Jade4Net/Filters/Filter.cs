using System.Collections.Generic;

namespace Jade.Filters
{
    public interface Filter
    {
        string convert(string source, Dictionary<string, object> attributes, Dictionary<string, object> model);
    }
}
