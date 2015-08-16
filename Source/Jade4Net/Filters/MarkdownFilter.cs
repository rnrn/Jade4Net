using System.Collections.Generic;
using MarkdownDeep;

namespace Jade.Filters
{
    public class MarkdownFilter : CachingFilter
    {
        private Markdown pegdown = new Markdown();

        public override string convert(string source, Dictionary<string, object> attributes) {
            return pegdown.Transform(source);
        }
    }
}
